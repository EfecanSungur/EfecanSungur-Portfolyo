using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// Tüm site içeriğini tek bir JSON dosyasında tutar (App_Data/content.json).
///
/// - Bellekte önbelleklenir. Okuyucular (sayfalar) kilitsiz okur; bu yüzden önbellekteki
///   nesne ASLA yerinde değiştirilmez: her kayıt bir kopya üzerinde yapılır, yazılır ve
///   önbellek tek atamayla yenisine çevrilir (copy-on-write). Böylece bir yönetici kaydı
///   ya da gelen mesaj, o anda listeyi dolaşan bir sayfayı "Collection was modified" ile düşürmez.
/// - Yazma kilitli + atomik (temp → File.Replace + .bak).
/// - content.json BOZUKSA üzerine asla yazılmaz: dosya (ve .bak) zaman damgalı kopyaya
///   alınır, site son yedekten (yoksa tohumdan) servis edilir ve servis salt okunur olur
///   (CANLI VERİ KURALI). Yönetici yedek geri yükleyince / sıfırlayınca kilit kalkar.
/// </summary>
public class JsonContentService : IContentService
{
    private readonly string _path;
    private readonly object _lock = new();
    private SiteContent? _cache;
    private volatile bool _readOnly;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Disk yazılamıyorsa (Plesk/IIS'te App_Data izni yoksa) ya da content.json okunamadıysa
    /// burası dolar. Site çalışmaya devam eder; panel bu uyarıyı her sayfada gösterir.
    /// </summary>
    public string? StorageWarning { get; private set; }

    public bool ReadOnly => _readOnly;

    public JsonContentService(IWebHostEnvironment env)
    {
        var dir = Path.Combine(env.ContentRootPath, "App_Data");
        // Klasör açılamazsa uygulama ÇÖKMEMELİ — IIS'te bu 500.30 demektir.
        try { Directory.CreateDirectory(dir); }
        catch (Exception ex) { StorageWarning = $"App_Data klasörü hazırlanamadı: {ex.Message}"; }
        _path = Path.Combine(dir, "content.json");
    }

    public SiteContent Get()
    {
        var cached = Volatile.Read(ref _cache);
        if (cached is not null) return cached;
        lock (_lock)
        {
            if (_cache is not null) return _cache;
            Volatile.Write(ref _cache, Load());
            return _cache!;
        }
    }

    private SiteContent Load()
    {
        if (!File.Exists(_path))
        {
            var seed = SeedData.Build();
            TryWrite(seed);   // yazamazsa bellekten servis etmeye devam eder
            return seed;
        }

        SiteContent? loaded = null;
        string? error = null;
        try
        {
            loaded = JsonSerializer.Deserialize<SiteContent>(File.ReadAllText(_path), JsonOpts);
            if (loaded is null) error = "dosya boş ya da null";
        }
        catch (Exception ex)
        {
            error = $"{ex.GetType().Name}: {ex.Message}";
        }

        if (loaded is not null)
        {
            Prepare(loaded, write: true);
            return loaded;
        }

        // ---- Bozuk dosya: üzerine YAZILMAZ. Önce kanıtı sakla, sonra salt okunur servis et.
        _readOnly = true;
        var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var kept = new List<string>();
        var bak = _path + ".bak";
        if (TryCopy(_path, $"content.corrupt-{stamp}.json")) kept.Add($"content.corrupt-{stamp}.json");
        if (File.Exists(bak) && TryCopy(bak, $"content.bak-{stamp}.json")) kept.Add($"content.bak-{stamp}.json");

        SiteContent? fallback = null;
        try { if (File.Exists(bak)) fallback = JsonSerializer.Deserialize<SiteContent>(File.ReadAllText(bak), JsonOpts); }
        catch { fallback = null; }

        string source;
        if (fallback is not null)
        {
            Prepare(fallback, write: false);
            source = "son yedekten (content.json.bak)";
        }
        else
        {
            fallback = SeedData.Build();
            source = "fabrika içeriğinden";
        }

        StorageWarning =
            $"content.json okunamadı ({error}). Site {source} servis ediliyor ve diske hiçbir şey yazılmıyor; " +
            "panelde yapılan değişiklikler ve yeni mesajlar yalnızca bellekte tutulur. " +
            (kept.Count > 0 ? $"Dosyalar App_Data'da {string.Join(" ve ", kept)} olarak saklandı. " : "") +
            "Düzeltmek için panelden bir yedek geri yükleyin (ya da sunucuda dosyayı onarıp uygulamayı yeniden başlatın).";
        return fallback;
    }

    /// <summary>Diskten okunan içeriği güncel sürüme taşır (idempotent; yalnız eski tohum değerleri değişir).</summary>
    private void Prepare(SiteContent loaded, bool write)
    {
        try
        {
            EnsureLists(loaded);
            if (ContentMigrations.Run(loaded))
            {
                Normalize(loaded);
                if (write) TryWrite(loaded);
            }
        }
        catch
        {
            // Migrasyon katmanı asla siteyi düşürmez.
        }
    }

    public void Save(Action<SiteContent> mutate) => SaveCore(mutate, touch: true);

    public void AllowWrites()
    {
        lock (_lock)
        {
            _readOnly = false;
            StorageWarning = null;
        }
    }

    /// <param name="touch">
    /// true: UpdatedAt güncellenir (sitemap lastmod, ProfilePage dateModified bunu kullanır).
    /// Gelen iletişim mesajı içerik değişikliği sayılmaz.
    /// </param>
    private void SaveCore(Action<SiteContent> mutate, bool touch)
    {
        lock (_lock)
        {
            // Kopya üzerinde çalış: okuyucuların elindeki nesne hiç değişmez.
            var copy = Clone(Get());
            mutate(copy);
            EnsureLists(copy);
            Normalize(copy);
            if (touch) copy.UpdatedAt = DateTime.UtcNow;
            TryWrite(copy);
            Volatile.Write(ref _cache, copy);
        }
    }

    private static SiteContent Clone(SiteContent source) =>
        JsonSerializer.Deserialize<SiteContent>(JsonSerializer.Serialize(source, JsonOpts), JsonOpts) ?? SeedData.Build();

    /// <summary>
    /// Diske yazmayı dener. Başarısız olursa istisna fırlatmaz — değişiklik bellekte
    /// kalır ve StorageWarning dolar. Böylece izin sorunu siteyi düşürmez.
    /// Salt okunur kipte (bozuk content.json) hiç yazmaz.
    /// </summary>
    private void TryWrite(SiteContent content)
    {
        if (_readOnly) return;
        try
        {
            WriteToDisk(content);
            StorageWarning = null;
        }
        catch (Exception ex)
        {
            StorageWarning =
                $"İçerik diske yazılamadı ({ex.GetType().Name}: {ex.Message}). " +
                "Değişiklikler yalnızca bellekte — uygulama yeniden başlayınca kaybolur. " +
                "Sunucuda App_Data klasörüne yazma izni verin.";
        }
    }

    private bool TryCopy(string source, string targetName)
    {
        try
        {
            var dir = Path.GetDirectoryName(_path)!;
            File.Copy(source, Path.Combine(dir, targetName), overwrite: false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void AddMessage(ContactMessage message)
    {
        SaveCore(c => c.Messages.Insert(0, message), touch: false);
    }

    private static void Normalize(SiteContent c)
    {
        Reorder(c.Nav); Reorder(c.Stats); Reorder(c.Expertise); Reorder(c.Timeline);
        Reorder(c.Quotes); Reorder(c.Gallery); Reorder(c.Socials); Reorder(c.Faq);
        Reorder(c.About.Facts); Reorder(c.References);
        if (c.Messages.Count > 500) c.Messages = c.Messages.Take(500).ToList();
    }

    /// <summary>JSON'da "null" yazan listeler sayfaları düşürmesin.</summary>
    private static void EnsureLists(SiteContent c)
    {
        c.Settings ??= new(); c.Theme ??= new(); c.Hero ??= new(); c.About ??= new(); c.Contact ??= new();
        c.Nav ??= new(); c.Stats ??= new(); c.Expertise ??= new(); c.Timeline ??= new();
        c.Quotes ??= new(); c.Gallery ??= new(); c.Socials ??= new(); c.Faq ??= new();
        c.Messages ??= new(); c.References ??= new(); c.AppliedMigrations ??= new();
        c.About.Facts ??= new(); c.About.Paragraphs ??= new(); c.About.Skills ??= new();
        foreach (var r in c.References)
        {
            r.Body ??= new(); r.Stack ??= new(); r.Highlights ??= new();
        }
    }

    private static void Reorder<T>(List<T> list) where T : ContentItem
    {
        var ordered = list.OrderBy(i => i.Order).ToList();
        for (var i = 0; i < ordered.Count; i++) ordered[i].Order = i;
        list.Clear();
        list.AddRange(ordered);
    }

    private void WriteToDisk(SiteContent content)
    {
        var json = JsonSerializer.Serialize(content, JsonOpts);
        var tmp = _path + ".tmp";
        File.WriteAllText(tmp, json);

        if (File.Exists(_path))
            File.Replace(tmp, _path, _path + ".bak", ignoreMetadataErrors: true);
        else
            File.Move(tmp, _path);
    }
}
