using EfecanSungur.Web.Models;
using EfecanSungur.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfecanSungur.Web.Controllers;

public class ReferanslarController : Controller
{
    private readonly IContentService _content;

    public ReferanslarController(IContentService content) => _content = content;

    [HttpGet("/referanslar")]
    public IActionResult Index()
    {
        var site = _content.Get();
        // Panelde "Referanslar bölümü" kapalıysa bölüm tamamen yayından kalkar.
        if (!site.Settings.ShowReferences) return NotFound();
        SeoText.References(site).ApplyTo(ViewData);
        return View(site);
    }

    [HttpGet("/referanslar/{slug}")]
    public IActionResult Detail(string slug)
    {
        var site = _content.Get();
        // Görünür liste SeoText.VisibleReferences ile aynı (bölüm kapalıysa boş → 404).
        var visible = SeoText.VisibleReferences(site);

        var index = visible.FindIndex(r =>
            string.Equals(r.Slug, slug, StringComparison.OrdinalIgnoreCase));
        if (index < 0) return NotFound();

        SeoText.Reference(site, visible[index]).ApplyTo(ViewData);
        return View(new ReferenceDetailViewModel
        {
            Content = site,
            Item = visible[index],
            Previous = index > 0 ? visible[index - 1] : null,
            Next = index < visible.Count - 1 ? visible[index + 1] : null,
        });
    }
}
