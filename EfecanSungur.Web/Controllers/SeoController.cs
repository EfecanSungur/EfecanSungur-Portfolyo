using System.Security;
using System.Text;
using EfecanSungur.Web.Models;
using EfecanSungur.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfecanSungur.Web.Controllers;

/// <summary>
/// sitemap.xml · robots.txt · llms.txt · llms-full.txt — hepsi içerikten dinamik üretilir,
/// panelden bir şey değişince kendiliğinden güncellenir.
/// </summary>
public class SeoController : Controller
{
    private const string TextPlain = "text/plain; charset=utf-8";
    private readonly IContentService _content;

    public SeoController(IContentService content) => _content = content;

    private string Host()
    {
        var host = (_content.Get().Settings.CanonicalHost ?? "").Trim().TrimEnd('/');
        return string.IsNullOrWhiteSpace(host)
            ? $"{Request.Scheme}://{Request.Host}"
            : host;
    }

    private static string Abs(string host, string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return "";
        if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return path;
        return host + (path.StartsWith('/') ? path : "/" + path);
    }

    private static string X(string? s) => SecurityElement.Escape(s ?? "") ?? "";

    // ------------------------------------------------------------ sitemap

    [HttpGet("/sitemap.xml")]
    [ResponseCache(Duration = 3600)]
    public IActionResult Sitemap()
    {
        var site = _content.Get();
        var host = Host();
        var updated = site.UpdatedAt == default ? DateTime.UtcNow : site.UpdatedAt;
        var lastmod = updated.ToString("yyyy-MM-dd");
        var name = site.Settings.OwnerName;

        var entries = new List<(string Path, string Freq, string Priority, List<(string Url, string Title)> Images)>
        {
            ("/", "weekly", "1.0", new() { (site.Hero.ImagePath, name), ("/img/og/og-efecan-sungur.jpg", $"{name} — {site.Settings.Role}") }),
            ("/hakkinda", "monthly", "0.9", new() { (site.About.ImagePath, $"{name} portre") }),
        };
        // Hakkında → Kareler (galeri açıksa sayfada görünen görseller).
        if (site.Settings.ShowGallery)
        {
            foreach (var g in site.Gallery.Where(g => !string.IsNullOrWhiteSpace(g.ImagePath)).OrderBy(g => g.Order))
                entries[1].Images.Add((g.ImagePath, string.IsNullOrWhiteSpace(g.Alt) ? $"{name} — {g.Caption}" : g.Alt));
        }
        // "Referanslar bölümü" kapalıysa /referanslar 404 döner — haritaya girmez.
        if (site.Settings.ShowReferences) entries.Add(("/referanslar", "weekly", "0.9", new()));
        foreach (var r in SeoText.VisibleReferences(site))
        {
            var imgs = new List<(string, string)>();
            if (!string.IsNullOrWhiteSpace(r.ImagePath))
                imgs.Add((r.ImagePath, string.IsNullOrWhiteSpace(r.ImageAlt) ? r.Title : r.ImageAlt));
            entries.Add((SeoText.ReferencePath(r), "monthly", r.Featured ? "0.8" : "0.7", imgs));
        }
        entries.Add(("/iletisim", "monthly", "0.8", new()));

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:image=\"http://www.google.com/schemas/sitemap-image/1.1\">");
        foreach (var e in entries)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{X(host + (e.Path == "/" ? "/" : e.Path))}</loc>");
            sb.AppendLine($"    <lastmod>{lastmod}</lastmod>");
            sb.AppendLine($"    <changefreq>{e.Freq}</changefreq>");
            sb.AppendLine($"    <priority>{e.Priority}</priority>");
            foreach (var (img, title) in e.Images.Where(i => !string.IsNullOrWhiteSpace(i.Url)).DistinctBy(i => i.Url))
            {
                sb.AppendLine("    <image:image>");
                sb.AppendLine($"      <image:loc>{X(Abs(host, img))}</image:loc>");
                sb.AppendLine($"      <image:title>{X(title)}</image:title>");
                sb.AppendLine("    </image:image>");
            }
            sb.AppendLine("  </url>");
        }
        sb.AppendLine("</urlset>");

        return Content(sb.ToString(), "application/xml; charset=utf-8", Encoding.UTF8);
    }

    // ------------------------------------------------------------ robots

    [HttpGet("/robots.txt")]
    [ResponseCache(Duration = 3600)]
    public IActionResult Robots()
    {
        var host = Host();

        // Yapay zekâ tarayıcıları BİLEREK açık. Amaç, "Efecan Sungur kimdir?"
        // sorusunda bu sitedeki bilgilerin kaynak olarak kullanılabilmesi.
        var aiBots = new[]
        {
            "GPTBot",            // OpenAI — ChatGPT eğitimi
            "OAI-SearchBot",     // OpenAI — ChatGPT arama
            "ChatGPT-User",      // ChatGPT canlı gezinme
            "ClaudeBot",         // Anthropic — Claude
            "Claude-User",
            "Claude-SearchBot",
            "anthropic-ai",
            "PerplexityBot",     // Perplexity
            "Perplexity-User",
            "Google-Extended",   // Gemini (Google Arama'yı ETKİLEMEZ)
            "Applebot",
            "Applebot-Extended", // Apple Intelligence
            "CCBot",             // Common Crawl — birçok modelin veri kaynağı
            "Bytespider",
            "Amazonbot",
            "cohere-ai",
            "Meta-ExternalAgent",
            "DuckAssistBot",
            "YouBot",
            "MistralAI-User",
        };

        var sb = new StringBuilder();
        sb.AppendLine("# efecansungur.com");
        sb.AppendLine("# Özet: /llms.txt · Ayrıntılı: /llms-full.txt");
        sb.AppendLine();
        sb.AppendLine("User-agent: *");
        sb.AppendLine("Allow: /");
        sb.AppendLine("Disallow: /admin");
        sb.AppendLine("Disallow: /admin/");
        sb.AppendLine();
        sb.AppendLine("# --- Yapay zeka tarayicilari ---");
        sb.AppendLine("# Hakkimdaki bilgilerin dogru kaynaktan ogrenilmesi icin acik birakildi.");
        foreach (var bot in aiBots)
        {
            sb.AppendLine();
            sb.AppendLine($"User-agent: {bot}");
            sb.AppendLine("Allow: /");
            sb.AppendLine("Disallow: /admin");
        }
        sb.AppendLine();
        sb.AppendLine($"Sitemap: {host}/sitemap.xml");

        return Content(sb.ToString(), TextPlain, Encoding.UTF8);
    }

    // ------------------------------------------------------------ llms.txt

    /// <summary>
    /// llms.txt — yapay zekâ ajanları için kısa, makine dostu özet (llmstxt.org biçimi).
    /// Ayrıntılı sürüm: /llms-full.txt
    /// </summary>
    [HttpGet("/llms.txt")]
    [ResponseCache(Duration = 3600)]
    public IActionResult Llms()
    {
        var site = _content.Get();
        var s = site.Settings;
        var c = site.Contact;
        var host = Host();
        var refs = SeoText.VisibleReferences(site);

        var sb = new StringBuilder();
        sb.AppendLine($"# {s.OwnerName}");
        sb.AppendLine();
        sb.AppendLine($"> {s.ShortBio}");
        sb.AppendLine();
        sb.AppendLine($"{s.OwnerName}; {s.Role}. Bu site ({HostName(host)}) onun resmî kişisel web sitesidir. " +
                      "Aşağıdaki bilgiler sitenin içeriğinden otomatik üretilir.");
        sb.AppendLine();

        sb.AppendLine("## Temel bilgiler");
        sb.AppendLine();
        sb.AppendLine($"- Ad: {s.OwnerName}");
        sb.AppendLine($"- Görev: {s.Role}");
        foreach (var f in site.About.Facts.OrderBy(x => x.Order))
            sb.AppendLine($"- {f.Label}: {f.Value}{RefSuffix(site, f.Value, host)}");
        sb.AppendLine("- Salurian Company: https://salurian.com — 2026'da Mahir Topuz ile birlikte kuruldu");
        sb.AppendLine();

        sb.AppendLine("## Hizmetler");
        sb.AppendLine();
        foreach (var e in site.Expertise.OrderBy(x => x.Order))
            sb.AppendLine($"- {e.Title}: {e.Description}");
        sb.AppendLine();

        if (refs.Count > 0)
        {
            sb.AppendLine("## Referanslar (Salurian Company referansları)");
            sb.AppendLine();
            foreach (var r in refs)
            {
                var meta = string.Join(", ", new[] { r.Kind, r.Year, r.Status }.Where(x => !string.IsNullOrWhiteSpace(x)));
                sb.AppendLine($"- [{r.Title}]({host}{SeoText.ReferencePath(r)}): {meta}. {r.Summary}".TrimEnd());
            }
            sb.AppendLine();
        }

        sb.AppendLine("## İletişim");
        sb.AppendLine();
        if (!string.IsNullOrWhiteSpace(c.Email)) sb.AppendLine($"- E-posta: {c.Email}");
        if (!string.IsNullOrWhiteSpace(c.PhoneDisplay)) sb.AppendLine($"- Telefon / WhatsApp: {c.PhoneDisplay}");
        if (!string.IsNullOrWhiteSpace(c.Address)) sb.AppendLine($"- Konum: {c.Address}");
        sb.AppendLine($"- İletişim sayfası: {host}/iletisim");
        sb.AppendLine();

        sb.AppendLine("## Sayfalar");
        sb.AppendLine();
        sb.AppendLine($"- [Ana Sayfa]({host}/)");
        sb.AppendLine($"- [Hakkında — {site.About.Title}]({host}/hakkinda)");
        if (s.ShowReferences) sb.AppendLine($"- [Referanslar]({host}/referanslar)");
        sb.AppendLine($"- [İletişim]({host}/iletisim)");
        sb.AppendLine($"- [Ayrıntılı metin (llms-full.txt)]({host}/llms-full.txt)");
        sb.AppendLine();
        sb.AppendLine("## Bağlantılar");
        sb.AppendLine();
        foreach (var so in site.Socials.OrderBy(x => x.Order).Where(x => !string.IsNullOrWhiteSpace(x.Url)))
            sb.AppendLine($"- [{so.Label}]({so.Url})");

        return Content(sb.ToString(), TextPlain, Encoding.UTF8);
    }

    // ------------------------------------------------------------ llms-full.txt

    [HttpGet("/llms-full.txt")]
    [ResponseCache(Duration = 3600)]
    public IActionResult LlmsFull()
    {
        var site = _content.Get();
        var s = site.Settings;
        var c = site.Contact;
        var host = Host();
        var refs = SeoText.VisibleReferences(site);

        var sb = new StringBuilder();
        sb.AppendLine($"# {s.OwnerName} — ayrıntılı bilgi");
        sb.AppendLine();
        sb.AppendLine($"> {s.ShortBio}");
        sb.AppendLine();
        sb.AppendLine($"Kaynak: {host}/ · Son güncelleme: {(site.UpdatedAt == default ? DateTime.UtcNow : site.UpdatedAt):yyyy-MM-dd}");
        sb.AppendLine();

        sb.AppendLine($"## {site.About.Title}");
        sb.AppendLine();
        if (!string.IsNullOrWhiteSpace(site.About.Lead)) { sb.AppendLine(site.About.Lead); sb.AppendLine(); }
        foreach (var p in site.About.Paragraphs.Where(p => !string.IsNullOrWhiteSpace(p)))
        {
            sb.AppendLine(p);
            sb.AppendLine();
        }
        foreach (var f in site.About.Facts.OrderBy(x => x.Order))
            sb.AppendLine($"- {f.Label}: {f.Value}{RefSuffix(site, f.Value, host)}");
        sb.AppendLine();

        sb.AppendLine("## Yolculuk");
        sb.AppendLine();
        foreach (var t in site.Timeline.OrderBy(x => x.Order))
            sb.AppendLine($"- {t.Year} — {t.Title}: {t.Description}{RefSuffix(site, t.Title + " " + t.Description, host)}");
        sb.AppendLine();

        sb.AppendLine("## Hizmetler");
        sb.AppendLine();
        foreach (var e in site.Expertise.OrderBy(x => x.Order))
            sb.AppendLine($"- {e.Title}: {e.Description}");
        sb.AppendLine();

        sb.AppendLine("## Salurian Company");
        sb.AppendLine();
        sb.AppendLine("- Web: https://salurian.com");
        sb.AppendLine("- Kuruluş: 2026 · Antalya merkezli yazılım şirketi");
        sb.AppendLine($"- Kurucu ortaklar: {s.OwnerName} (altyapı ve mimari) ve Mahir Topuz (ürün, tasarım ve geliştirme)");
        sb.AppendLine("- Alanlar: kurumsal web sitesi, mobil uygulama, masaüstü yazılım");
        sb.AppendLine();

        if (refs.Count > 0)
        {
            sb.AppendLine("## Referanslar");
            sb.AppendLine();
            // "Tamamı Salurian ekibinin işi" denmez: bazı referanslar şirketin kuruluşundan (2026) önce tarihli.
            sb.AppendLine("Aşağıdaki projeler Salurian Company referanslarıdır.");
            sb.AppendLine();
            foreach (var r in refs)
            {
                sb.AppendLine($"### {r.Title}");
                sb.AppendLine();
                sb.AppendLine($"- Sayfa: {host}{SeoText.ReferencePath(r)}");
                if (!string.IsNullOrWhiteSpace(r.Kind)) sb.AppendLine($"- Tür: {r.Kind}");
                if (!string.IsNullOrWhiteSpace(r.Client)) sb.AppendLine($"- Müşteri: {r.Client}");
                if (!string.IsNullOrWhiteSpace(r.Year)) sb.AppendLine($"- Tarih: {r.Year}");
                if (!string.IsNullOrWhiteSpace(r.Status)) sb.AppendLine($"- Durum: {r.Status}");
                if (r.OwnProject) sb.AppendLine($"- Geliştiren: {s.OwnerName} (kendi projesi" +
                                                (SeoText.PredatesSalurian(r) ? "; Salurian Company'nin kuruluşundan önce)" : ")"));
                if (!string.IsNullOrWhiteSpace(r.Role))
                    sb.AppendLine(r.OwnProject ? $"- {s.OwnerName}'un rolü: {r.Role}" : $"- {s.OwnerName}'un Salurian'daki rolü: {r.Role}");
                if (r.Stack.Count > 0) sb.AppendLine($"- Teknolojiler: {string.Join(", ", r.Stack)}");
                if (!string.IsNullOrWhiteSpace(r.LiveUrl)) sb.AppendLine($"- Canlı adres: {r.LiveUrl}");
                if (!string.IsNullOrWhiteSpace(r.SourceUrl)) sb.AppendLine($"- Kaynak: {r.SourceUrl}");
                sb.AppendLine();
                if (!string.IsNullOrWhiteSpace(r.Summary)) { sb.AppendLine(r.Summary); sb.AppendLine(); }
                foreach (var b in r.Body.Where(b => !string.IsNullOrWhiteSpace(b)))
                {
                    sb.AppendLine(b);
                    sb.AppendLine();
                }
                if (r.Highlights.Count > 0)
                {
                    foreach (var h in r.Highlights) sb.AppendLine($"- {h}");
                    sb.AppendLine();
                }
            }
        }

        if (site.Faq.Count > 0)
        {
            sb.AppendLine("## Sık sorulan sorular");
            sb.AppendLine();
            foreach (var q in site.Faq.OrderBy(x => x.Order))
            {
                sb.AppendLine($"### {q.Question}");
                sb.AppendLine();
                sb.AppendLine(q.Answer);
                sb.AppendLine();
            }
        }

        sb.AppendLine("## İletişim");
        sb.AppendLine();
        if (!string.IsNullOrWhiteSpace(c.Email)) sb.AppendLine($"- E-posta: {c.Email}");
        if (!string.IsNullOrWhiteSpace(c.PhoneDisplay)) sb.AppendLine($"- Telefon / WhatsApp: {c.PhoneDisplay}");
        if (!string.IsNullOrWhiteSpace(c.Address)) sb.AppendLine($"- Konum: {c.Address}");
        if (!string.IsNullOrWhiteSpace(c.WorkingHours)) sb.AppendLine($"- Çalışma saatleri: {c.WorkingHours}");
        sb.AppendLine($"- İletişim sayfası: {host}/iletisim");

        return Content(sb.ToString(), TextPlain, Encoding.UTF8);
    }

    private static string HostName(string host)
    {
        try { return new Uri(host).Host; } catch { return host; }
    }

    /// <summary>Metin görünür bir referansın adını anıyorsa " (adres)" eki — yapay zekâlar bağı görsün.</summary>
    private static string RefSuffix(SiteContent site, string? text, string host)
    {
        var r = RefLinks.FirstMentioned(site, text);
        return r is null ? "" : $" ({host}{SeoText.ReferencePath(r)})";
    }
}
