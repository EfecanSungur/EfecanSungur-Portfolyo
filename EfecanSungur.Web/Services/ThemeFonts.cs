using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// Tema fontları → Google Fonts css2 sorguları. Panel seçenekleri ve _Layout'taki
/// font bağlantısı buradan beslenir; listede olmayan bir ad seçilirse o yuvanın
/// varsayılanı kullanılır (geçersiz bir css2 isteği tüm fontları düşürür).
/// </summary>
public static class ThemeFonts
{
    public const string DefaultDisplay = "Inter Tight";
    public const string DefaultBody = "Inter";
    public const string DefaultAccent = "Instrument Serif";
    public const string DefaultMono = "JetBrains Mono";

    /// <summary>Font adı → css2 "family=" değeri (eksen/ağırlıklar Google'da gerçekten var olanlar).</summary>
    public static readonly IReadOnlyDictionary<string, string> GoogleQuery = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        // Sans — başlık / metin
        ["Inter Tight"]        = "Inter+Tight:ital,wght@0,300;0,400;0,500;0,600;0,700;0,800;1,400",
        ["Inter"]              = "Inter:ital,wght@0,300;0,400;0,500;0,600;0,700;1,400",
        ["Manrope"]            = "Manrope:wght@300;400;500;600;700;800",
        ["DM Sans"]            = "DM+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700;1,400",
        ["Space Grotesk"]      = "Space+Grotesk:wght@300;400;500;600;700",
        ["Work Sans"]          = "Work+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700;1,400",
        ["Jost"]               = "Jost:ital,wght@0,300;0,400;0,500;0,600;0,700;1,400",
        // Serif — vurgu / eski tema
        ["Instrument Serif"]   = "Instrument+Serif:ital@0;1",
        ["Fraunces"]           = "Fraunces:ital,wght@0,400;0,500;0,600;0,700;1,400;1,500",
        ["Newsreader"]         = "Newsreader:ital,wght@0,400;0,500;0,600;1,400;1,500",
        ["Playfair Display"]   = "Playfair+Display:ital,wght@0,400;0,500;0,600;0,700;0,800;0,900;1,400;1,500;1,600",
        ["Cormorant Garamond"] = "Cormorant+Garamond:ital,wght@0,300;0,400;0,500;0,600;0,700;1,300;1,400;1,500",
        ["Libre Baskerville"]  = "Libre+Baskerville:ital,wght@0,400;0,700;1,400",
        ["DM Serif Display"]   = "DM+Serif+Display:ital@0;1",
        ["Marcellus"]          = "Marcellus",
        // Mono
        ["JetBrains Mono"]     = "JetBrains+Mono:wght@400;500;600",
        ["IBM Plex Mono"]      = "IBM+Plex+Mono:wght@400;500;600",
        ["Space Mono"]         = "Space+Mono:ital,wght@0,400;0,700;1,400",
    };

    public static readonly string[] DisplayOptions =
        { "Inter Tight", "Manrope", "DM Sans", "Space Grotesk", "Inter", "Fraunces", "Playfair Display" };
    public static readonly string[] BodyOptions =
        { "Inter", "Manrope", "DM Sans", "Work Sans", "Jost", "Inter Tight" };
    public static readonly string[] AccentOptions =
        { "Instrument Serif", "Fraunces", "Newsreader", "Playfair Display", "Cormorant Garamond", "DM Serif Display" };
    public static readonly string[] MonoOptions =
        { "JetBrains Mono", "IBM Plex Mono", "Space Mono" };

    /// <summary>Bilinen bir font adı döndürür; bilinmiyorsa yuvanın varsayılanı.</summary>
    public static string Resolve(string? name, string fallback) =>
        !string.IsNullOrWhiteSpace(name) && GoogleQuery.ContainsKey(name.Trim()) ? name.Trim() : fallback;

    /// <summary>Tek bir fontun css2 "family=" değeri.</summary>
    public static string Query(string? name, string fallback) => GoogleQuery[Resolve(name, fallback)];

    /// <summary>
    /// Temadaki dört font (Display, Body, Accent, Mono) için tek css2 adresi.
    /// Aynı font iki yuvada seçilmişse bir kez istenir.
    /// </summary>
    public static string CssUrl(ThemeSettings? t)
    {
        t ??= new ThemeSettings();
        var families = new[]
            {
                Resolve(t.DisplayFont, DefaultDisplay),
                Resolve(t.BodyFont, DefaultBody),
                Resolve(t.AccentFont, DefaultAccent),
                Resolve(t.MonoFont, DefaultMono),
            }
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(n => "family=" + GoogleQuery[n]);

        return "https://fonts.googleapis.com/css2?" + string.Join("&", families) + "&display=swap";
    }

    /// <summary>CSS font-family değeri için güvenli ad (tırnak içine konacak).</summary>
    public static string CssName(string? name, string fallback) => Resolve(name, fallback);
}
