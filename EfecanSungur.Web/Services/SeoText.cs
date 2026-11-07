using System.Globalization;
using System.Text;
using EfecanSungur.Web.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EfecanSungur.Web.Services;

/// <summary>Bir sayfanın arama motoru / paylaşım bilgileri. Denetleyiciler ViewData'ya basar.</summary>
public sealed class SeoMeta
{
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    /// <summary>Kök göreli yol ("/", "/hakkinda" ...). Sonunda / yok (kök hariç).</summary>
    public string Canonical { get; init; } = "/";
    public string OgImage { get; init; } = SeoText.DefaultOgImage;
    public string OgImageAlt { get; init; } = "";
    public int OgImageWidth { get; init; }
    public int OgImageHeight { get; init; }
    public string OgType { get; init; } = "website";
    /// <summary>home · about · references · reference · contact</summary>
    public string PageType { get; init; } = "home";
    public List<BreadcrumbItem>? Breadcrumbs { get; init; }
    public ReferenceItem? Reference { get; init; }

    /// <summary>
    /// ViewData anahtarları: Title, Description, Canonical, OgImage, OgImageAlt,
    /// OgImageWidth, OgImageHeight, OgType, PageType, Breadcrumbs, Reference.
    /// _SeoHead ve _StructuredData bunları okur.
    /// </summary>
    public void ApplyTo(ViewDataDictionary vd)
    {
        vd["Title"] = Title;
        vd["Description"] = Description;
        vd["Canonical"] = Canonical;
        vd["OgImage"] = OgImage;
        vd["OgImageAlt"] = OgImageAlt;
        if (OgImageWidth > 0 && OgImageHeight > 0)
        {
            vd["OgImageWidth"] = OgImageWidth;
            vd["OgImageHeight"] = OgImageHeight;
        }
        vd["OgType"] = OgType;
        vd["PageType"] = PageType;
        if (Breadcrumbs is not null) vd["Breadcrumbs"] = Breadcrumbs;
        if (Reference is not null) vd["Reference"] = Reference;
    }
}

/// <summary>
/// Başlık / açıklama / slug üreticileri. Tüm herkese açık sayfaların SEO metni buradan çıkar;
/// metinlerin hepsi içerikten (panelden düzenlenebilen veriden) türetilir.
/// </summary>
public static class SeoText
{
    public const string Dash = " — ";
    public const int TitleMax = 60;
    public const int DescMin = 120;
    public const int DescMax = 160;

    public const string DefaultOgImage = "/img/og/og-efecan-sungur.jpg";
    public const int OgWidth = 1200;
    public const int OgHeight = 630;

    /// <summary>Program.cs başlangıçta doldurur; referans OG görselinin diskte olup olmadığına bakmak için.</summary>
    public static string? WebRootPath { get; set; }

    // ------------------------------------------------------------ sayfalar

    public static SeoMeta Home(SiteContent site)
    {
        var name = Name(site);
        return new SeoMeta
        {
            Title = FitTitle($"{name}{Dash}Yazılım Geliştirici, Antalya"),
            Description = CompleteDescription(site.Settings.MetaDescription,
                $"{name}; Salurian Company kurucu ortağı.",
                "Web, mobil ve masaüstü yazılım projeleri."),
            Canonical = "/",
            OgImageAlt = $"{name}{Dash}{site.Settings.Role}",
            OgImageWidth = OgWidth, OgImageHeight = OgHeight,
            PageType = "home",
        };
    }

    public static SeoMeta About(SiteContent site)
    {
        var name = Name(site);
        var title = string.IsNullOrWhiteSpace(site.About.Title) ? $"{name} kimdir?" : site.About.Title.Trim();
        return new SeoMeta
        {
            Title = FitTitle($"{title}{Dash}Salurian Kurucu Ortağı", title),
            Description = CompleteDescription(
                $"{name} kimdir? 2022'den beri yazılım geliştiren, Piksel Akademi'de eğitim almış backend developer ve Mahir Topuz ile Salurian Company'nin kurucu ortağı.",
                "Antalya."),
            Canonical = "/hakkinda",
            OgImageAlt = $"{name}{Dash}{site.Settings.Role}",
            OgImageWidth = OgWidth, OgImageHeight = OgHeight,
            OgType = "profile",
            PageType = "about",
            Breadcrumbs = Crumbs(("Ana Sayfa", "/"), ("Hakkında", "/hakkinda")),
        };
    }

    public static SeoMeta References(SiteContent site)
    {
        var name = Name(site);
        var visible = VisibleReferences(site);
        var names = visible.OrderByDescending(r => r.Featured).ThenBy(r => r.Order)
                           .Select(r => ShortName(r)).Distinct().Take(4).ToList();
        // "ekibinin projesi" denmez: bazı referanslar Salurian'ın kuruluşundan (2026) önce tarihli.
        var lead = visible.Count > 0
            ? $"{name} ve Salurian Company referansları: {string.Join(", ", names)} dahil {visible.Count} proje."
            : $"{name} ve Salurian Company referansları.";
        return new SeoMeta
        {
            Title = FitTitle($"{name} Referansları{Dash}Salurian Projeleri"),
            Description = CompleteDescription(lead,
                "Web siteleri ve mobil uygulamalar.",
                "Antalya merkezli yazılım geliştirme."),
            Canonical = "/referanslar",
            OgImageAlt = $"{name} referansları",
            OgImageWidth = OgWidth, OgImageHeight = OgHeight,
            PageType = "references",
            Breadcrumbs = Crumbs(("Ana Sayfa", "/"), ("Referanslar", "/referanslar")),
        };
    }

    public static SeoMeta Reference(SiteContent site, ReferenceItem r)
    {
        var name = Name(site);
        var path = ReferencePath(r);
        var title = ReferenceTitle(r.Title, name);

        var baseText = string.IsNullOrWhiteSpace(r.Summary) ? $"{r.Title.Trim()}." : r.Summary.Trim();

        var extras = new List<string>();
        if (!string.IsNullOrWhiteSpace(r.Kind) && !string.IsNullOrWhiteSpace(r.Year)
            && !baseText.Contains(r.Kind.Trim(), StringComparison.OrdinalIgnoreCase))
            extras.Add($"{r.Kind.Trim()}, {r.Year.Trim()}.");
        if (r.OwnProject) extras.Add($"Geliştiren: {name}.");
        extras.Add(r.OwnProject || PredatesSalurian(r) ? "Salurian Company referansı." : $"{name} ve Salurian Company ekibinin projesi.");
        if (r.Stack.Count > 0) extras.Add($"Teknolojiler: {string.Join(", ", r.Stack.Take(4))}.");
        if (!string.IsNullOrWhiteSpace(r.Client)) extras.Add($"Müşteri: {r.Client.Trim()}.");

        var (og, w, h) = ReferenceOgImage(r);
        return new SeoMeta
        {
            Title = title,
            Description = CompleteDescription(baseText, extras.ToArray()),
            Canonical = path,
            OgImage = og,
            OgImageWidth = w, OgImageHeight = h,
            OgImageAlt = string.IsNullOrWhiteSpace(r.ImageAlt) ? r.Title : r.ImageAlt,
            OgType = "article",
            PageType = "reference",
            Breadcrumbs = Crumbs(("Ana Sayfa", "/"), ("Referanslar", "/referanslar"), (r.Title, path)),
            Reference = r,
        };
    }

    public static SeoMeta Contact(SiteContent site)
    {
        var name = Name(site);
        var c = site.Contact;
        var bits = new List<string>();
        if (!string.IsNullOrWhiteSpace(c.PhoneDisplay)) bits.Add(c.PhoneDisplay.Trim());
        if (!string.IsNullOrWhiteSpace(c.Email)) bits.Add(c.Email.Trim());
        var lead = bits.Count > 0
            ? $"{name} ile iletişim: {string.Join(" · ", bits)}."
            : $"{name} ile iletişim.";
        return new SeoMeta
        {
            Title = FitTitle($"{name} İletişim{Dash}Antalya Yazılım Geliştirici"),
            Description = CompleteDescription(lead,
                "Web, mobil ve masaüstü projeleriniz için WhatsApp, telefon ya da form üzerinden ulaşın.",
                "Antalya merkezli, uzaktan ve yerinde çalışma."),
            Canonical = "/iletisim",
            OgImageAlt = $"{name} iletişim",
            OgImageWidth = OgWidth, OgImageHeight = OgHeight,
            PageType = "contact",
            Breadcrumbs = Crumbs(("Ana Sayfa", "/"), ("İletişim", "/iletisim")),
        };
    }

    // ------------------------------------------------------------ referans yardımcıları

    /// <summary>
    /// Sitede yayımlanan referanslar. Panelde "Referanslar bölümü" kapalıysa BOŞ döner:
    /// sitemap, llms.txt/llms-full.txt, JSON-LD ve /referanslar sayfaları buna uyar.
    /// </summary>
    public static List<ReferenceItem> VisibleReferences(SiteContent site) =>
        site.Settings?.ShowReferences == false
            ? new List<ReferenceItem>()
            : (site.References ?? new()).Where(r => r.Visible && !string.IsNullOrWhiteSpace(r.Slug))
                                        .OrderBy(r => r.Order).ToList();

    /// <summary>Salurian Company'nin kuruluş yılı (salurian.com, Efecan'ın beyanı).</summary>
    public const int SalurianFounded = 2026;

    /// <summary>
    /// Salurian ekibinin en erken tarihli referansları Temmuz 2026 (Atiye Tezcan, CheckList); salurian.com
    /// Temmuz 2026 sonunda yayına girdi. Daha önceki AYLI 2026 tarihleri (ör. Huawei Hedef Takip, "Haziran 2026")
    /// kuruluştan öncedir. Yalnız "2026" yazan (ayı belirsiz) işler Salurian dönemi sayılır.
    /// </summary>
    public const int SalurianFoundedMonth = 7;

    /// <summary>Tarihi Salurian'ın kuruluşundan önceyse true (ör. "Ağustos 2025", "Haziran 2026"). Tarih çözülemezse false.</summary>
    public static bool PredatesSalurian(ReferenceItem r)
    {
        var iso = IsoDate(r.Year);
        if (iso is null || !int.TryParse(iso[..4], out var y)) return false;
        if (y != SalurianFounded) return y < SalurianFounded;
        return iso.Length == 7 && int.TryParse(iso[5..], out var m) && m < SalurianFoundedMonth;
    }

    public static string ReferencePath(ReferenceItem r) => "/referanslar/" + (r.Slug ?? "").Trim().ToLowerInvariant();

    /// <summary>"Vianoria Tekstil, Giyim &amp; Aksesuar" → "Vianoria" gibi kısa ad (açıklamalarda).</summary>
    public static string ShortName(ReferenceItem r)
    {
        // Müşteri adı genelde en kısa ve tanınır ad: "Acay Kuyumculuk · Döşemealtı" → "Acay Kuyumculuk"
        var client = (r.Client ?? "").Split('·')[0].Trim();
        if (client.Length > 0 && !client.Contains("Salurian", StringComparison.OrdinalIgnoreCase)
            && !client.Contains("ekib", StringComparison.OrdinalIgnoreCase)
            && client.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length <= 3)
            return client;

        var t = (r.Title ?? "").Trim();
        foreach (var sep in new[] { " — ", " - ", " | ", "," })
        {
            var i = t.IndexOf(sep, StringComparison.Ordinal);
            if (i > 0) t = t[..i];
        }
        var words = t.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length > 3 ? string.Join(' ', words.Take(2)) : t;
    }

    /// <summary>
    /// "{Proje} — Efecan Sungur referansı" — her referansta AYNI ek. "projesi" denmez: işler
    /// Salurian ekibinindir, başlık tek başına yazarlık iddiası taşımasın. 60'a sığmazsa proje
    /// adı sondan kelime kelime kısaltılır (üç nokta yok; sondaki "ve", "&amp;", virgül atılır).
    /// </summary>
    public static string ReferenceTitle(string projectTitle, string name)
    {
        var t = Collapse(projectTitle ?? "");
        var suffix = $"{Dash}{name} referansı";
        if (t.Length + suffix.Length <= TitleMax) return t + suffix;

        var words = t.Split(' ').ToList();
        while (words.Count > 1 && string.Join(' ', words).Length + suffix.Length > TitleMax) words.RemoveAt(words.Count - 1);
        while (words.Count > 1 && words[^1].TrimEnd(',') is "ve" or "&" or "" ) words.RemoveAt(words.Count - 1);
        var shortTitle = string.Join(' ', words).TrimEnd(',', ' ');
        if (shortTitle.Length + suffix.Length <= TitleMax) return shortTitle + suffix;
        return Truncate(t, TitleMax - Dash.Length - name.Length) + Dash + name;
    }

    private static (string Path, int W, int H) ReferenceOgImage(ReferenceItem r)
    {
        var slug = (r.Slug ?? "").Trim().ToLowerInvariant();
        var ogPath = $"/img/og/ref-{slug}.jpg";
        if (!string.IsNullOrEmpty(slug) && WebRootPath is not null)
        {
            try
            {
                if (File.Exists(Path.Combine(WebRootPath, "img", "og", $"ref-{slug}.jpg")))
                    return (ogPath, OgWidth, OgHeight);
            }
            catch { /* disk okunamazsa varsayılan */ }
        }
        // Panelden yeni eklenmiş referans: kapak görselinin kendisi (boyutu bilinmiyor).
        if (!string.IsNullOrWhiteSpace(r.ImagePath)) return (r.ImagePath.Trim(), 0, 0);
        return (DefaultOgImage, OgWidth, OgHeight);
    }

    // ------------------------------------------------------------ metin araçları

    private static string Name(SiteContent site) =>
        string.IsNullOrWhiteSpace(site.Settings.OwnerName) ? "Efecan Sungur" : site.Settings.OwnerName.Trim();

    private static List<BreadcrumbItem> Crumbs(params (string Name, string Url)[] items) =>
        items.Select(i => new BreadcrumbItem(i.Name, i.Url)).ToList();

    /// <summary>≤ 60 karakter. Sığmazsa önce yedek başlık, sonra kelime sınırından kısaltma.</summary>
    public static string FitTitle(string title, string? fallback = null)
    {
        title = Collapse(title);
        if (title.Length <= TitleMax) return title;
        if (!string.IsNullOrWhiteSpace(fallback) && Collapse(fallback).Length <= TitleMax) return Collapse(fallback);
        return Truncate(title, TitleMax);
    }

    /// <summary>Kelime sınırından kısaltır ve "…" ekler (sonuç ≤ max).</summary>
    public static string Truncate(string text, int max)
    {
        text = Collapse(text);
        if (text.Length <= max) return text;
        var cut = text[..(max - 1)];
        var sp = cut.LastIndexOf(' ');
        if (sp > max / 2) cut = cut[..sp];
        return cut.TrimEnd(' ', ',', ';', ':', '—', '-', '·') + "…";
    }

    /// <summary>
    /// Açıklama 120–160 karakter: metin uzunsa cümle/kelime sınırından kısaltılır;
    /// kısaysa ek cümleler (yalnız veriden gelenler) bütünüyle sığdıkça eklenir.
    /// </summary>
    public static string CompleteDescription(string? text, params string[] extras)
    {
        var d = Collapse(text ?? "");
        if (d.Length > DescMax) return TrimDescription(d);

        foreach (var extra in extras)
        {
            if (d.Length >= DescMin) break;
            var e = Collapse(extra);
            if (e.Length == 0 || d.Contains(e, StringComparison.OrdinalIgnoreCase)) continue;
            var candidate = d.Length == 0 ? e : $"{EndSentence(d)} {e}";
            if (candidate.Length <= DescMax) d = candidate;
        }
        return d;
    }

    public static string TrimDescription(string text)
    {
        text = Collapse(text);
        if (text.Length <= DescMax) return text;
        // Önce cümle sınırı (en az 120 karakterde)
        var window = text[..DescMax];
        var dot = window.LastIndexOfAny(new[] { '.', '!', '?' });
        if (dot >= DescMin - 1) return window[..(dot + 1)];
        return Truncate(text, DescMax);
    }

    private static string EndSentence(string s)
    {
        s = s.TrimEnd();
        return s.Length > 0 && ".!?…".Contains(s[^1]) ? s : s + ".";
    }

    private static string Collapse(string s) =>
        string.Join(' ', (s ?? "").Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    // ------------------------------------------------------------ slug

    /// <summary>Türkçe harfleri katlar, küçük harf + tire: "Çiçek Şöleni" → "cicek-soleni".</summary>
    public static string Slugify(string? text)
    {
        var sb = new StringBuilder();
        foreach (var ch in (text ?? "").Trim())
        {
            var c = ch switch
            {
                'ç' or 'Ç' => 'c', 'ğ' or 'Ğ' => 'g', 'ı' or 'I' or 'İ' or 'i' => 'i',
                'ö' or 'Ö' => 'o', 'ş' or 'Ş' => 's', 'ü' or 'Ü' => 'u',
                'â' or 'Â' => 'a', 'î' or 'Î' => 'i', 'û' or 'Û' => 'u',
                _ => char.ToLowerInvariant(ch)
            };
            if (c is >= 'a' and <= 'z' or >= '0' and <= '9') sb.Append(c);
            else if (sb.Length > 0 && sb[^1] != '-') sb.Append('-');
        }
        var slug = sb.ToString().Trim('-');
        if (slug.Length > 80) slug = slug[..80].Trim('-');
        return slug;
    }

    /// <summary>Slug'ı diğer kayıtlara göre benzersiz yapar (-2, -3 ...).</summary>
    public static string UniqueSlug(string slug, IEnumerable<string> taken)
    {
        var set = new HashSet<string>(taken.Where(t => !string.IsNullOrWhiteSpace(t)), StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrEmpty(slug)) slug = "proje";
        if (!set.Contains(slug)) return slug;
        for (var i = 2; ; i++)
        {
            var candidate = $"{slug}-{i}";
            if (!set.Contains(candidate)) return candidate;
        }
    }

    // ------------------------------------------------------------ tarih

    private static readonly string[] TrMonths =
        { "ocak", "şubat", "mart", "nisan", "mayıs", "haziran", "temmuz", "ağustos", "eylül", "ekim", "kasım", "aralık" };

    /// <summary>"Eylül 2026" → "2026-09", "2026" → "2026"; çözülemezse null.</summary>
    public static string? IsoDate(string? year)
    {
        if (string.IsNullOrWhiteSpace(year)) return null;
        var parts = year.Trim().ToLower(new CultureInfo("tr-TR")).Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var y = parts.Select(p => int.TryParse(p, out var n) ? n : 0).FirstOrDefault(n => n is >= 1990 and <= 2100);
        if (y == 0) return null;
        var m = parts.Select(p => Array.IndexOf(TrMonths, p)).FirstOrDefault(i => i >= 0, -1);
        return m >= 0 ? $"{y:0000}-{m + 1:00}" : $"{y:0000}";
    }
}
