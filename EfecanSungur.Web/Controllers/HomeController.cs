using System.Diagnostics;
using EfecanSungur.Web.Models;
using EfecanSungur.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EfecanSungur.Web.Controllers;

public class HomeController : Controller
{
    private readonly IContentService _content;

    public HomeController(IContentService content) => _content = content;

    [HttpGet("/")]
    public IActionResult Index()
    {
        var site = _content.Get();
        SeoText.Home(site).ApplyTo(ViewData);
        return View(site);
    }

    // Her yöntemi kabul eder: UseStatusCodePagesWithReExecute başarısız bir POST'u (ör. süresi
    // dolmuş form) POST olarak yeniden çalıştırır; yalnız GET olsaydı boş bir 405 dönerdi.
    [Route("/Home/Error")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public IActionResult Error(int? code)
    {
        var status = code ?? 500;
        var vm = new ErrorViewModel
        {
            Code = status,
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        (vm.Title, vm.Message) = status switch
        {
            404 => ("Sayfa bulunamadı", "Aradığınız sayfa taşınmış ya da hiç var olmamış olabilir."),
            403 => ("Erişim yok", "Bu sayfayı görüntüleme yetkiniz bulunmuyor."),
            _   => ("Bir şeyler ters gitti", "Beklenmeyen bir hata oluştu. Kısa süre sonra tekrar deneyin.")
        };

        Response.StatusCode = status;
        return View(vm);
    }
}
