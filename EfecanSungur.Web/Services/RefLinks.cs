using EfecanSungur.Web.Models;
using Microsoft.AspNetCore.Html;

namespace EfecanSungur.Web.Services;

/// <summary>
/// Düz metindeki referans adlarını (ör. "Huawei Hedef Takip") o referansın sayfasına bağlar.
/// Bağ içerikten türetilir: metin panelden değişirse bağ da değişir; referans gizlenir ya da
/// Referanslar bölümü kapatılırsa (SeoText.VisibleReferences boş) bağ kurulmaz — 404'e bağlantı olmaz.
/// Eşleşme büyük/küçük harfe duyarlı, kelime sınırında; her referans metin başına en fazla bir kez bağlanır.
/// </summary>
public static class RefLinks
{
    private static List<ReferenceItem> Candidates(SiteContent site) =>
        SeoText.VisibleReferences(site)
            .Where(r => !string.IsNullOrWhiteSpace(r.Title) && r.Title.Trim().Length >= 4)
            .OrderByDescending(r => r.Title.Trim().Length)
            .ToList();

    /// <summary>Metinde adı geçen ilk görünür referans (metindeki konuma göre); yoksa null.</summary>
    public static ReferenceItem? FirstMentioned(SiteContent site, string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        return Matches(text, Candidates(site)).Select(m => m.Ref).FirstOrDefault();
    }

    /// <summary>Metni HTML olarak kodlar; görünür referans adlarını &lt;a href="/referanslar/{slug}"&gt; ile sarar.</summary>
    public static IHtmlContent Linkify(SiteContent site, string? text)
    {
        var builder = new HtmlContentBuilder();
        if (string.IsNullOrEmpty(text)) return builder;

        var pos = 0;
        foreach (var (index, length, r) in Matches(text, Candidates(site)))
        {
            if (index > pos) builder.Append(text[pos..index]);
            builder.AppendHtml("<a href=\"");
            builder.Append(SeoText.ReferencePath(r));
            builder.AppendHtml("\">");
            builder.Append(text.Substring(index, length));
            builder.AppendHtml("</a>");
            pos = index + length;
        }
        if (pos < text.Length) builder.Append(text[pos..]);
        return builder;
    }

    /// <summary>Çakışmayan eşleşmeler, metindeki sıraya göre (uzun ad önce denenir).</summary>
    private static List<(int Index, int Length, ReferenceItem Ref)> Matches(string text, List<ReferenceItem> refs)
    {
        var found = new List<(int Index, int Length, ReferenceItem Ref)>();
        foreach (var r in refs)
        {
            var title = r.Title.Trim();
            var from = 0;
            while (from < text.Length)
            {
                var i = text.IndexOf(title, from, StringComparison.Ordinal);
                if (i < 0) break;
                var end = i + title.Length;
                var boundary = (i == 0 || !char.IsLetterOrDigit(text[i - 1]))
                               && (end >= text.Length || !char.IsLetterOrDigit(text[end]));
                var overlaps = found.Any(f => i < f.Index + f.Length && f.Index < end);
                if (boundary && !overlaps)
                {
                    found.Add((i, title.Length, r));
                    break; // her referans bir kez
                }
                from = i + 1;
            }
        }
        return found.OrderBy(f => f.Index).ToList();
    }
}
