namespace EfecanSungur.Web.Services;

/// <summary>
/// Panelden gelen tema renklerini güvenli hâle getirir. _Layout (:root), _SeoHead
/// (theme-color), _AdminTheme ve _AdminLayout aynı normalleştirmeyi kullanır: geçersiz bir
/// değer (ör. "red;}&lt;/style&gt;") asla stil bloğuna ya da meta etikete ulaşmaz.
/// </summary>
public static class ThemeColors
{
    /// <summary>"#abc", "abc", "#AABBCC" → "#AABBCC"; başka her şey → <paramref name="fallback"/>.</summary>
    public static string Hex(string? value, string fallback)
    {
        var x = (value ?? "").Trim().TrimStart('#');
        if (x.Length == 3) x = string.Concat(x.Select(ch => $"{ch}{ch}"));
        return x.Length == 6 && x.All(Uri.IsHexDigit) ? "#" + x.ToUpperInvariant() : fallback;
    }

    /// <summary>İki #RRGGBB rengin karışımı: p = a'nın payı (0–1). Her tarayıcıda aynı sonuç.</summary>
    public static string Mix(string a, string b, double p)
    {
        static int C(string h, int i) => Convert.ToInt32(h.Substring(1 + i * 2, 2), 16);
        return "#" + string.Concat(Enumerable.Range(0, 3)
            .Select(i => ((int)Math.Round(C(a, i) * p + C(b, i) * (1 - p))).ToString("X2")));
    }
}
