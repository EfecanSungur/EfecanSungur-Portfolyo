using System.Collections;
using System.Security.Claims;
using System.Text.Json;
using EfecanSungur.Web.Models;
using EfecanSungur.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EfecanSungur.Web.Controllers;

[Authorize]
[Route("admin")]
public class AdminController : Controller
{
    // Tek kullanıcılı sistem — Identity/veritabanı yok.
    private const string AdminUser = "efecan";
    private const string AdminPass = "rx9Avwyc";

    private static readonly string[] AllowedImageExtensions =
        { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".avif", ".svg" };

    private readonly IContentService _content;
    private readonly IWebHostEnvironment _env;

    public AdminController(IContentService content, IWebHostEnvironment env)
    {
        _content = content;
        _env = env;
    }

    // ------------------------------------------------------------------ auth

    [AllowAnonymous]
    [HttpGet("giris")]
    public IActionResult Giris(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(Index));

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginModel());
    }

    [AllowAnonymous]
    [HttpPost("giris")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Giris(LoginModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        if (model.Username?.Trim() != AdminUser || model.Password != AdminPass)
        {
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            return View(model);
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, AdminUser),
            new Claim(ClaimTypes.Role, "Admin")
        }, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = true });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("cikis")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cikis()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Giris));
    }

    // ------------------------------------------------------------- dashboard

    [HttpGet("")]
    public IActionResult Index() => View(_content.Get());

    // --------------------------------------------------------- tekil bölümler

    [HttpGet("bolum/{key}")]
    public IActionResult Bolum(string key)
    {
        var section = ContentSchema.Section(key);
        if (section is null) return NotFound();

        ViewData["Section"] = section;
        return View("Section", _content.Get());
    }

    [HttpPost("bolum/{key}")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(21_000_000)]
    public IActionResult Bolum(string key, IFormCollection form)
    {
        var section = ContentSchema.Section(key);
        if (section is null) return NotFound();

        _content.Save(content =>
        {
            var target = section.Target(content);
            ContentSchema.Apply(target, section.Fields, CollectValues(form, section.Fields));
        });

        TempData["Flash"] = $"{section.Title} kaydedildi.";
        return RedirectToAction(nameof(Bolum), new { key });
    }

    // -------------------------------------------------------------- listeler

    [HttpGet("liste/{key}")]
    public IActionResult Liste(string key)
    {
        var collection = ContentSchema.Collection(key);
        if (collection is null) return NotFound();

        ViewData["Collection"] = collection;
        return View("Collection", _content.Get());
    }

    [HttpGet("liste/{key}/yeni")]
    public IActionResult Yeni(string key)
    {
        var collection = ContentSchema.Collection(key);
        if (collection is null) return NotFound();

        ViewData["Collection"] = collection;
        ViewData["Item"] = Activator.CreateInstance(collection.ItemType)!;
        ViewData["IsNew"] = true;
        return View("ItemForm", _content.Get());
    }

    [HttpGet("liste/{key}/duzenle/{id}")]
    public IActionResult Duzenle(string key, string id)
    {
        var collection = ContentSchema.Collection(key);
        if (collection is null) return NotFound();

        var item = Find(collection, id);
        if (item is null) return NotFound();

        ViewData["Collection"] = collection;
        ViewData["Item"] = item;
        ViewData["IsNew"] = false;
        return View("ItemForm", _content.Get());
    }

    [HttpPost("liste/{key}/kaydet")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(21_000_000)]
    public IActionResult Kaydet(string key, string? id, IFormCollection form)
    {
        var collection = ContentSchema.Collection(key);
        if (collection is null) return NotFound();

        var values = CollectValues(form, collection.Fields);

        _content.Save(content =>
        {
            var list = collection.Target(content);
            ContentItem? item = string.IsNullOrEmpty(id)
                ? null
                : list.Cast<ContentItem>().FirstOrDefault(i => i.Id == id);

            if (item is null)
            {
                item = (ContentItem)Activator.CreateInstance(collection.ItemType)!;
                item.Order = list.Count;
                list.Add(item);
            }

            ContentSchema.Apply(item, collection.Fields, values);

            // Referans adresi: boşsa başlıktan üret, her durumda benzersiz yap.
            if (item is ReferenceItem reference)
            {
                var slug = SeoText.Slugify(string.IsNullOrWhiteSpace(reference.Slug) ? reference.Title : reference.Slug);
                var taken = list.Cast<ReferenceItem>().Where(r => !ReferenceEquals(r, reference)).Select(r => r.Slug);
                reference.Slug = SeoText.UniqueSlug(slug, taken);
                if (string.IsNullOrWhiteSpace(reference.Category)) reference.Category = "web";
            }
        });

        TempData["Flash"] = string.IsNullOrEmpty(id)
            ? $"{collection.Singular} eklendi."
            : $"{collection.Singular} güncellendi.";

        return RedirectToAction(nameof(Liste), new { key });
    }

    [HttpPost("liste/{key}/sil/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Sil(string key, string id)
    {
        var collection = ContentSchema.Collection(key);
        if (collection is null) return NotFound();

        _content.Save(content =>
        {
            var list = collection.Target(content);
            var item = list.Cast<ContentItem>().FirstOrDefault(i => i.Id == id);
            if (item is not null) list.Remove(item);
        });

        TempData["Flash"] = $"{collection.Singular} silindi.";
        return RedirectToAction(nameof(Liste), new { key });
    }

    [HttpPost("liste/{key}/tasi/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Tasi(string key, string id, string yon)
    {
        var collection = ContentSchema.Collection(key);
        if (collection is null) return NotFound();

        _content.Save(content =>
        {
            var list = collection.Target(content);
            var items = list.Cast<ContentItem>().OrderBy(i => i.Order).ToList();
            var index = items.FindIndex(i => i.Id == id);
            if (index < 0) return;

            var target = yon == "yukari" ? index - 1 : index + 1;
            if (target < 0 || target >= items.Count) return;

            (items[index].Order, items[target].Order) = (items[target].Order, items[index].Order);
        });

        return RedirectToAction(nameof(Liste), new { key });
    }

    // -------------------------------------------------------------- mesajlar

    [HttpGet("mesajlar")]
    public IActionResult Mesajlar() => View(_content.Get());

    [HttpPost("mesajlar/{id}/okundu")]
    [ValidateAntiForgeryToken]
    public IActionResult MesajOkundu(string id)
    {
        _content.Save(content =>
        {
            var msg = content.Messages.FirstOrDefault(m => m.Id == id);
            if (msg is not null) msg.IsRead = !msg.IsRead;
        });
        return RedirectToAction(nameof(Mesajlar));
    }

    [HttpPost("mesajlar/{id}/sil")]
    [ValidateAntiForgeryToken]
    public IActionResult MesajSil(string id)
    {
        _content.Save(content => content.Messages.RemoveAll(m => m.Id == id));
        TempData["Flash"] = "Mesaj silindi.";
        return RedirectToAction(nameof(Mesajlar));
    }

    [HttpPost("mesajlar/temizle")]
    [ValidateAntiForgeryToken]
    public IActionResult MesajlariTemizle()
    {
        _content.Save(content => content.Messages.Clear());
        TempData["Flash"] = "Tüm mesajlar silindi.";
        return RedirectToAction(nameof(Mesajlar));
    }

    // ------------------------------------------------------------ yedekleme

    [HttpGet("yedek")]
    public IActionResult Yedek()
    {
        var json = JsonSerializer.Serialize(_content.Get(), new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
        });
        var name = $"efecansungur-yedek-{DateTime.Now:yyyyMMdd-HHmm}.json";
        return File(System.Text.Encoding.UTF8.GetBytes(json), "application/json", name);
    }

    [HttpPost("geri-yukle")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(21_000_000)]
    public async Task<IActionResult> GeriYukle(IFormFile? dosya)
    {
        if (dosya is null || dosya.Length == 0)
        {
            TempData["Flash"] = "Dosya seçilmedi.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            using var reader = new StreamReader(dosya.OpenReadStream());
            var json = await reader.ReadToEndAsync();
            var restored = JsonSerializer.Deserialize<SiteContent>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (restored is null) throw new InvalidOperationException();

            // Yanlış dosya (başka bir sitenin JSON'u, boş nesne…) tüm içeriği boşlarla ezmesin.
            if (restored.Settings is null || restored.Hero is null || restored.About is null
                || restored.Contact is null || restored.Nav is null || restored.Nav.Count == 0)
            {
                TempData["Flash"] = "Bu dosya bir efecansungur.com yedeği değil — içerik değiştirilmedi.";
                return RedirectToAction(nameof(Index));
            }

            // Bozuk content.json yüzünden salt okunur kipteyse: bilinçli geri yükleme kilidi kaldırır.
            _content.AllowWrites();
            _content.Save(content =>
            {
                content.Settings = restored.Settings;
                content.Theme = restored.Theme;
                content.Hero = restored.Hero;
                content.About = restored.About;
                content.Contact = restored.Contact;
                content.Nav = restored.Nav;
                content.Stats = restored.Stats;
                content.Expertise = restored.Expertise;
                content.Timeline = restored.Timeline;
                content.Quotes = restored.Quotes;
                content.Gallery = restored.Gallery;
                content.Socials = restored.Socials;
                content.Faq = restored.Faq;
                content.Messages = restored.Messages;
                content.References = restored.References ?? new();
                content.AppliedMigrations = restored.AppliedMigrations ?? new();
                // Eski (ör. Temmuz 2026) bir yedek geri yüklendiyse onu da güncel sürüme taşı:
                // referanslar boşsa eklenir, yalnız eski tohum değerindeki metinler yenilenir.
                ContentMigrations.Run(content);
            });

            TempData["Flash"] = "Yedek geri yüklendi.";
        }
        catch
        {
            TempData["Flash"] = "Dosya okunamadı — geçerli bir yedek JSON'u seçin.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("sifirla")]
    [ValidateAntiForgeryToken]
    public IActionResult Sifirla()
    {
        var fresh = SeedData.Build();
        _content.AllowWrites();
        _content.Save(content =>
        {
            var messages = content.Messages;
            content.Settings = fresh.Settings;
            content.Theme = fresh.Theme;
            content.Hero = fresh.Hero;
            content.About = fresh.About;
            content.Contact = fresh.Contact;
            content.Nav = fresh.Nav;
            content.Stats = fresh.Stats;
            content.Expertise = fresh.Expertise;
            content.Timeline = fresh.Timeline;
            content.Quotes = fresh.Quotes;
            content.Gallery = fresh.Gallery;
            content.Socials = fresh.Socials;
            content.Faq = fresh.Faq;
            content.References = fresh.References;
            content.AppliedMigrations = fresh.AppliedMigrations;
            content.Messages = messages;
        });

        TempData["Flash"] = "İçerik fabrika ayarlarına döndürüldü (mesajlar korundu).";
        return RedirectToAction(nameof(Index));
    }

    // -------------------------------------------------------------- yardımcı

    private ContentItem? Find(CollectionDef collection, string id) =>
        collection.Target(_content.Get()).Cast<ContentItem>().FirstOrDefault(i => i.Id == id);

    /// <summary>
    /// Form alanlarını okur; Image tipindeki alanlarda dosya yüklendiyse
    /// diske kaydedip yolunu değer olarak döndürür.
    /// </summary>
    private Dictionary<string, string?> CollectValues(IFormCollection form, IEnumerable<FieldDef> fields)
    {
        var values = new Dictionary<string, string?>(StringComparer.Ordinal);

        foreach (var field in fields)
        {
            if (field.Type == FieldType.Image)
            {
                var file = form.Files[$"file_{field.Name}"];
                if (file is { Length: > 0 })
                {
                    var saved = SaveImage(file);
                    if (saved is not null)
                    {
                        values[field.Name] = saved;
                        continue;
                    }
                }
            }

            values[field.Name] = form.TryGetValue(field.Name, out var v) ? v.ToString() : null;
        }

        return values;
    }

    private string? SaveImage(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(ext)) return null;

        var folder = Path.Combine(_env.WebRootPath, "img", "uploads");
        Directory.CreateDirectory(folder);

        var name = $"{DateTime.Now:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString("N")[..6]}{ext}";
        var full = Path.Combine(folder, name);

        using (var stream = System.IO.File.Create(full))
            file.CopyTo(stream);

        return $"/img/uploads/{name}";
    }
}
