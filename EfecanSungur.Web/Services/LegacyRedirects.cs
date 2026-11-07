namespace EfecanSungur.Web.Services;

/// <summary>
/// Bu alan adında daha önce "APG Ajans" sitesi yayındaydı ve Google'da hâlâ
/// o sayfaların bağlantıları duruyor. 301 ile yeni karşılıklarına yönlendiriyoruz:
/// ziyaretçi 404 yerine anlamlı bir sayfaya düşer, Google da eski adresi
/// indeksten çıkarıp yerine hedefi koyar.
/// </summary>
public static class LegacyRedirects
{
    private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["/logo"]              = "/hakkinda",
        ["/logo-tasarim"]      = "/hakkinda",
        ["/hizmetler"]         = "/hakkinda",
        ["/hizmet"]            = "/hakkinda",
        ["/referans"]          = "/referanslar",
        ["/hakkimizda"]        = "/hakkinda",
        ["/hakkimda"]          = "/hakkinda",
        ["/sss"]               = "/iletisim",
        ["/sikca-sorulan-sorular"] = "/iletisim",
        ["/iletisim.html"]     = "/iletisim",
        ["/mobil-uygulama"]    = "/hakkinda",
        ["/web-tasarim"]       = "/hakkinda",
        ["/bulut-tabanli-medya"] = "/hakkinda",
        ["/gizlilik"]          = "/",
        ["/gizlilik-policesi"] = "/",
        ["/panel"]             = "/admin",
        ["/index.html"]        = "/",
        ["/anasayfa"]          = "/",
    };

    static LegacyRedirects()
    {
        // 28 Eki 2026: eski portre dosyaları silindi; Google Görseller'deki ve önbellekteki adresler yenisine gider.
        foreach (var (from, to) in SiteImages.Redirects()) Map[from] = to;
    }

    public static bool TryResolve(string path, out string target)
    {
        target = "";
        if (string.IsNullOrWhiteSpace(path)) return false;

        var key = path.TrimEnd('/');
        if (key.Length == 0) return false;

        if (Map.TryGetValue(key, out target!)) return true;

        // salurian.com biçimi /referans/{slug} → /referanslar/{slug} (tek adımda, yeniden adlandırma dahil).
        // Yeniden adlandırılmış eski slug → yeni slug (LegacySeedV2a.SlugRenames; içerik migrasyonu
        // "2026-10-24-v2-duzeltme" kayıttaki slug'ı da aynı şekilde değiştirir).
        foreach (var prefix in new[] { "/referans/", "/referanslar/" })
        {
            if (!key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;
            var slug = key[prefix.Length..];
            if (slug.Length == 0 || slug.Contains('/')) return false;
            var renamed = LegacySeedV2a.SlugRenames.TryGetValue(slug, out var neu);
            if (prefix == "/referanslar/" && !renamed) return false;   // gerçek adres: dokunma
            target = "/referanslar/" + (renamed ? neu : slug.ToLowerInvariant());
            return true;
        }
        return false;
    }
}
