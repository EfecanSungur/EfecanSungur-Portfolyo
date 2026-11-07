using EfecanSungur.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Razor varsayılanı yalnız Basic Latin'i düz basar; Türkçe harfler "i&#x15F;" gibi varlık
// olarak çıkardı (sayfa şişer, ham HTML okuyan tarayıcı/yapay zekâ araçları için okunaksız).
// Tüm Unicode düz basılır; <, >, &, ", ' yine kodlanır (XSS koruması aynen sürer).
builder.Services.Configure<Microsoft.Extensions.WebEncoders.WebEncoderOptions>(o =>
    o.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(System.Text.Unicode.UnicodeRanges.All));
builder.Services.AddSingleton<IContentService, JsonContentService>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "EfecanAdminAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/admin/giris";
        options.LogoutPath = "/admin/cikis";
        options.AccessDeniedPath = "/admin/giris";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Görsel yüklemeleri için (tek dosya 20 MB)
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 20L * 1024 * 1024;
    o.ValueLengthLimit = int.MaxValue;
});

// Plesk/IIS arkasında SSL çoğunlukla proxy'de sonlanır ve uygulamaya HTTP gelir.
// Bu başlıklar olmadan uygulama isteği "güvensiz" sanır ve HTTPS yönlendirmesi
// sonsuz döngüye girer.
builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    o.KnownNetworks.Clear();
    o.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

// Referans OG görselinin diskte olup olmadığına bakmak için (SeoText).
SeoText.WebRootPath = app.Environment.WebRootPath;

// ── Kanonik adres: http→https ve www→apex TEK ADIMDA 301 ──────────────────
// Salurian'da doğrulanan uygulamanın aynısı. web.config <rewrite> kullanılmıyor
// (Plesk'te URL Rewrite modülü yoksa 500.19). ForwardedHeaders'tan SONRA çalışır:
// outofprocess barındırmada gerçek şema X-Forwarded-Proto'dan gelir.
// Yalnız kendi alan adımız için; localhost ve diğer adlara dokunmaz.
// /.well-known/ muaf: Let's Encrypt doğrulaması http üzerinden gelir.
// Açma/kapama: appsettings.json → "Site:CanonicalRedirect". SSL sertifikası
// çalışmıyorken açılırsa http ziyaretçileri çalışmayan https'e gönderilir.
var canonicalRedirect = builder.Configuration.GetValue("Site:CanonicalRedirect", false);
var canonicalHostName = (builder.Configuration.GetValue<string>("Site:CanonicalHost") ?? "efecansungur.com")
                        .Trim().ToLowerInvariant();
if (canonicalRedirect && canonicalHostName.Length > 0)
{
    app.Use(async (ctx, next) =>
    {
        var req = ctx.Request;
        var rawHost = req.Host.Host.ToLowerInvariant();
        var host = rawHost.TrimEnd('.');
        var ours = host == canonicalHostName || host == "www." + canonicalHostName;

        if (ours &&
            (HttpMethods.IsGet(req.Method) || HttpMethods.IsHead(req.Method)) &&
            !req.Path.StartsWithSegments("/.well-known", StringComparison.OrdinalIgnoreCase) &&
            (!req.IsHttps || rawHost != canonicalHostName))
        {
            var target = "https://" + canonicalHostName + req.PathBase + req.Path + req.QueryString;
            ctx.Response.StatusCode = StatusCodes.Status301MovedPermanently;
            ctx.Response.Headers.Location = target;
            return;
        }

        await next();
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // HTTPS zorlaması BİLEREK varsayılan olarak kapalı: SSL sertifikası hazır
    // olmadan açılırsa site erişilemez hale gelir. Plesk'te SSL'i doğruladıktan
    // sonra appsettings.json içinde "Site:ForceHttps": true yapın.
    if (builder.Configuration.GetValue("Site:ForceHttps", false))
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }
}

// Eski APG Ajans adreslerini kalıcı olarak yeni sayfalara taşı.
// Statik dosyalardan ÖNCE çalışmalı ki /logo gibi yollar burada yakalansın.
app.Use(async (ctx, next) =>
{
    if (LegacyRedirects.TryResolve(ctx.Request.Path.Value ?? "", out var target))
    {
        ctx.Response.Redirect(target, permanent: true);
        return;
    }
    await next();
});

app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// İçerik dosyası ilk istekten önce hazır olsun.
// Burada fırlayan her istisna IIS'te doğrudan "HTTP 500.30 — app failed to start"
// demektir. Ön yükleme kritik değil; başarısız olursa ilk istekte tekrar denenir.
try
{
    app.Services.GetRequiredService<IContentService>().Get();
}
catch (Exception ex)
{
    app.Services.GetRequiredService<ILoggerFactory>()
       .CreateLogger("Startup")
       .LogError(ex, "İçerik ön yüklemesi başarısız — uygulama yine de başlatılıyor.");
}

app.Run();
