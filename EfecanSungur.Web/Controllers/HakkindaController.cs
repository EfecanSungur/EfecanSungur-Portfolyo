using EfecanSungur.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfecanSungur.Web.Controllers;

public class HakkindaController : Controller
{
    private readonly IContentService _content;

    public HakkindaController(IContentService content) => _content = content;

    [HttpGet("/hakkinda")]
    public IActionResult Index()
    {
        var site = _content.Get();
        SeoText.About(site).ApplyTo(ViewData);
        return View(site);
    }
}
