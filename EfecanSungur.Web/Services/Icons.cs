using Microsoft.AspNetCore.Html;

namespace EfecanSungur.Web.Services;

/// <summary>
/// Tek satırlık inline SVG ikon seti. Harici ikon kütüphanesi yok —
/// hepsi 24×24 viewBox, currentColor ile boyanır.
/// </summary>
public static class Icons
{
    private static readonly Dictionary<string, string> Paths = new(StringComparer.OrdinalIgnoreCase)
    {
        // --- genel arayüz
        ["arrow"] = "<path d='M5 12h14M13 6l6 6-6 6'/>",
        ["arrow-down"] = "<path d='M12 5v14M6 13l6 6 6-6'/>",
        ["arrow-up-right"] = "<path d='M7 17 17 7M8 7h9v9'/>",
        ["chevron"] = "<path d='m9 6 6 6-6 6'/>",
        ["chevron-down"] = "<path d='m6 9 6 6 6-6'/>",
        ["menu"] = "<path d='M4 7h16M4 12h16M4 17h16'/>",
        ["close"] = "<path d='M6 6l12 12M18 6 6 18'/>",
        ["home"] = "<path d='M4 11 12 4l8 7'/><path d='M6 10v9h12v-9'/>",
        ["user"] = "<circle cx='12' cy='8' r='3.6'/><path d='M4.5 20a7.5 7.5 0 0 1 15 0'/>",
        ["plus"] = "<path d='M12 5v14M5 12h14'/>",
        ["check"] = "<path d='m5 13 4.5 4.5L19 7'/>",
        ["edit"] = "<path d='M4 20h4l10-10-4-4L4 16v4Z'/><path d='m14 6 4 4'/>",
        ["trash"] = "<path d='M4 7h16M9 7V5h6v2M6 7l1 13h10l1-13'/>",
        ["up"] = "<path d='M12 19V6M6 12l6-6 6 6'/>",
        ["down"] = "<path d='M12 5v13M6 12l6 6 6-6'/>",
        ["save"] = "<path d='M5 4h11l3 3v13H5z'/><path d='M8 4v5h7V4M8 20v-6h8v6'/>",
        ["back"] = "<path d='M19 12H5M11 18l-6-6 6-6'/>",
        ["download"] = "<path d='M12 4v11M7 11l5 5 5-5'/><path d='M5 19h14'/>",
        ["upload"] = "<path d='M12 20V8M7 12l5-5 5 5'/><path d='M5 4h14'/>",
        ["refresh"] = "<path d='M20 12a8 8 0 1 1-2.4-5.7'/><path d='M20 4v5h-5'/>",
        ["external"] = "<path d='M14 5h5v5'/><path d='M19 5 10 14'/><path d='M18 14v5H5V6h5'/>",
        ["logout"] = "<path d='M14 5H6v14h8'/><path d='m17 9 3 3-3 3M20 12h-9'/>",
        ["eye"] = "<path d='M2.5 12S6 5.5 12 5.5 21.5 12 21.5 12 18 18.5 12 18.5 2.5 12 2.5 12Z'/><circle cx='12' cy='12' r='3'/>",
        ["search"] = "<circle cx='11' cy='11' r='6'/><path d='m20 20-4.5-4.5'/>",

        // --- iletişim
        ["phone"] = "<path d='M7 3.5 9.5 9l-2 1.6a12 12 0 0 0 5.9 5.9L15 14.5l5.5 2.5v3A1.5 1.5 0 0 1 19 21.4 18 18 0 0 1 2.6 5 1.5 1.5 0 0 1 4 3.5Z'/>",
        ["mail"] = "<rect x='3' y='5' width='18' height='14' rx='2'/><path d='m3.5 7 8.5 6 8.5-6'/>",
        ["pin"] = "<path d='M12 21s7-6.2 7-11a7 7 0 1 0-14 0c0 4.8 7 11 7 11Z'/><circle cx='12' cy='10' r='2.6'/>",
        ["clock"] = "<circle cx='12' cy='12' r='8.5'/><path d='M12 7.5V12l3 2'/>",
        ["whatsapp"] = "<path d='M20.5 11.6A8.4 8.4 0 0 1 7.9 19l-4.4 1.2 1.2-4.3A8.4 8.4 0 1 1 20.5 11.6Z'/><path d='M9 8.7c.3-.1.6 0 .8.3l.8 1.4c.1.3.1.6-.1.8l-.5.6c.6 1.2 1.5 2 2.6 2.6l.6-.6c.2-.2.5-.3.8-.1l1.4.8c.3.2.4.5.3.8-.2.7-.9 1.3-1.7 1.3-3 0-6-3-6-6 0-.8.5-1.5 1-1.9Z'/>",

        // --- uzmanlık
        ["spark"] = "<path d='M12 3.5 13.9 9 19.5 11l-5.6 2L12 18.5 10.1 13 4.5 11 10.1 9Z'/>",
        ["compass"] = "<circle cx='12' cy='12' r='8.5'/><path d='m15.5 8.5-2 5-5 2 2-5Z'/>",
        ["layers"] = "<path d='m12 3.5 8.5 4.5L12 12.5 3.5 8Z'/><path d='m3.5 12.5 8.5 4.5 8.5-4.5'/>",
        ["target"] = "<circle cx='12' cy='12' r='8.5'/><circle cx='12' cy='12' r='4.5'/><circle cx='12' cy='12' r='1'/>",
        ["pen"] = "<path d='M4 20h4L20 8l-4-4L4 16Z'/><path d='m14 6 4 4'/>",
        ["chart"] = "<path d='M4 20h16'/><path d='M7 20v-6M12 20V7M17 20v-9'/>",
        ["message"] = "<path d='M20.5 12a7.5 7.5 0 0 1-11.2 6.5L4 20l1.5-4.6A7.5 7.5 0 1 1 20.5 12Z'/>",
        ["shield"] = "<path d='M12 3.5 19 6v6c0 4.2-3 7.4-7 8.5-4-1.1-7-4.3-7-8.5V6Z'/>",
        ["globe"] = "<circle cx='12' cy='12' r='8.5'/><path d='M3.5 12h17'/><path d='M12 3.5c2.4 2.6 3.6 5.4 3.6 8.5S14.4 18.4 12 20.5c-2.4-2.1-3.6-5.4-3.6-8.5S9.6 6.1 12 3.5Z'/>",
        ["users"] = "<circle cx='9' cy='8.5' r='3.2'/><path d='M3 19.5a6 6 0 0 1 12 0'/><path d='M16 6.2a3.2 3.2 0 0 1 0 6.1M17.5 19.5a5.6 5.6 0 0 0-2-4'/>",
        ["quote"] = "<path d='M9.5 6.5C7 7.5 5.5 9.8 5.5 12.6V17h5v-5H8c0-1.7.6-2.9 2-3.6Z'/><path d='M18.5 6.5c-2.5 1-4 3.3-4 6.1V17h5v-5H17c0-1.7.6-2.9 2-3.6Z'/>",
        ["grid"] = "<rect x='4' y='4' width='7' height='7' rx='1.5'/><rect x='13' y='4' width='7' height='7' rx='1.5'/><rect x='4' y='13' width='7' height='7' rx='1.5'/><rect x='13' y='13' width='7' height='7' rx='1.5'/>",
        ["timeline"] = "<path d='M7 4v16'/><circle cx='7' cy='8' r='2'/><circle cx='7' cy='16' r='2'/><path d='M11 8h9M11 16h6'/>",
        ["image"] = "<rect x='3.5' y='5' width='17' height='14' rx='2'/><circle cx='9' cy='10' r='1.6'/><path d='m4.5 17 4.5-4.5 3.5 3.5 3-2.5 4 3.5'/>",
        ["info"] = "<circle cx='12' cy='12' r='8.5'/><path d='M12 11v5.5'/><circle cx='12' cy='7.9' r='.9' fill='currentColor' stroke='none'/>",
        ["help"] = "<circle cx='12' cy='12' r='8.5'/><path d='M9.7 9.4A2.4 2.4 0 0 1 14.4 10c0 1.6-2.4 1.9-2.4 3.4'/><circle cx='12' cy='16.6' r='.9' fill='currentColor' stroke='none'/>",
        ["settings"] = "<circle cx='12' cy='12' r='3'/><path d='M19.4 14.5a1.6 1.6 0 0 0 .3 1.8l.1.1a2 2 0 1 1-2.8 2.8l-.1-.1a1.6 1.6 0 0 0-1.8-.3 1.6 1.6 0 0 0-1 1.5v.2a2 2 0 1 1-4 0v-.1a1.6 1.6 0 0 0-1-1.5 1.6 1.6 0 0 0-1.8.3l-.1.1a2 2 0 1 1-2.8-2.8l.1-.1a1.6 1.6 0 0 0 .3-1.8 1.6 1.6 0 0 0-1.5-1H3a2 2 0 1 1 0-4h.1a1.6 1.6 0 0 0 1.5-1 1.6 1.6 0 0 0-.3-1.8l-.1-.1a2 2 0 1 1 2.8-2.8l.1.1a1.6 1.6 0 0 0 1.8.3H9a1.6 1.6 0 0 0 1-1.5V3a2 2 0 1 1 4 0v.1a1.6 1.6 0 0 0 1 1.5 1.6 1.6 0 0 0 1.8-.3l.1-.1a2 2 0 1 1 2.8 2.8l-.1.1a1.6 1.6 0 0 0-.3 1.8V9a1.6 1.6 0 0 0 1.5 1h.2a2 2 0 1 1 0 4h-.1a1.6 1.6 0 0 0-1.5 1Z'/>",
        ["palette"] = "<path d='M12 3.5a8.5 8.5 0 1 0 0 17c1.4 0 2-1 1.5-1.9-.6-1.1.2-2.3 1.5-2.3h1.6a3.9 3.9 0 0 0 3.9-3.9c0-4.9-3.9-8.9-8.5-8.9Z'/><circle cx='8' cy='11' r='1' fill='currentColor' stroke='none'/><circle cx='11' cy='7.5' r='1' fill='currentColor' stroke='none'/><circle cx='15' cy='8.5' r='1' fill='currentColor' stroke='none'/>",
        ["star"] = "<path d='m12 4 2.4 5 5.6.8-4 3.9 1 5.5-5-2.7-5 2.7 1-5.5-4-3.9 5.6-.8Z'/>",
        ["list"] = "<path d='M8 7h12M8 12h12M8 17h12'/><circle cx='4.5' cy='7' r='1' fill='currentColor' stroke='none'/><circle cx='4.5' cy='12' r='1' fill='currentColor' stroke='none'/><circle cx='4.5' cy='17' r='1' fill='currentColor' stroke='none'/>",
        ["share"] = "<circle cx='17.5' cy='6' r='2.5'/><circle cx='6.5' cy='12' r='2.5'/><circle cx='17.5' cy='18' r='2.5'/><path d='m8.8 10.8 6.4-3.5M8.8 13.2l6.4 3.5'/>",
        ["inbox"] = "<path d='M4 13h4l1.5 3h5L16 13h4'/><path d='M4 13 6 5h12l2 8v6H4Z'/>",

        // --- sosyal
        ["instagram"] = "<rect x='4' y='4' width='16' height='16' rx='4.5'/><circle cx='12' cy='12' r='3.6'/><circle cx='17' cy='7' r='.9' fill='currentColor' stroke='none'/>",
        ["linkedin"] = "<rect x='4' y='4' width='16' height='16' rx='2.5'/><path d='M8 10.5V17M8 7.6v.1'/><path d='M12 17v-3.6a2 2 0 0 1 4 0V17'/><path d='M12 10.5V17'/>",
        ["x"] = "<path d='m5 5 14 14M19 5 5 19'/>",
        ["youtube"] = "<rect x='3' y='6' width='18' height='12' rx='3.5'/><path d='m10.5 9.5 4.5 2.5-4.5 2.5Z'/>",
        ["facebook"] = "<path d='M14.5 8.5h2V5.5h-2.2a3.3 3.3 0 0 0-3.3 3.3v2.2H9v3h2v6h3v-6h2.2l.5-3H14v-1.7c0-.5.3-.8.5-.8Z'/>",
        ["link"] = "<path d='M10.5 13.5a3.5 3.5 0 0 0 5 0l2.5-2.5a3.5 3.5 0 0 0-5-5l-1.2 1.2'/><path d='M13.5 10.5a3.5 3.5 0 0 0-5 0L6 13a3.5 3.5 0 0 0 5 5l1.2-1.2'/>",
    };

    public static IHtmlContent Svg(string name, string cssClass = "icon", int size = 24)
    {
        if (!Paths.TryGetValue(name ?? "", out var body))
            body = Paths["spark"];

        return new HtmlString(
            $"<svg class=\"{cssClass}\" width=\"{size}\" height=\"{size}\" viewBox=\"0 0 24 24\" fill=\"none\" " +
            "stroke=\"currentColor\" stroke-width=\"1.5\" stroke-linecap=\"round\" stroke-linejoin=\"round\" " +
            $"aria-hidden=\"true\" focusable=\"false\">{body}</svg>");
    }
}
