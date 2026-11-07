using EfecanSungur.Web.Models;
using EfecanSungur.Web.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace EfecanSungur.Web.Controllers;

public class IletisimController : Controller
{
    private readonly IContentService _content;
    private readonly IAntiforgery _antiforgery;

    public IletisimController(IContentService content, IAntiforgery antiforgery)
    {
        _content = content;
        _antiforgery = antiforgery;
    }

    [HttpGet("/iletisim")]
    public IActionResult Index()
    {
        var site = _content.Get();
        ViewData["Content"] = site;
        SeoText.Contact(site).ApplyTo(ViewData);
        return View(new ContactFormModel());
    }

    /// <summary>
    /// Antiforgery burada ELLE doğrulanır: önbellekten açılmış ya da uzun süre açık kalmış bir
    /// sayfadan gönderilen formda belirteç geçersizse ziyaretçi boş bir 400 sayfası görmesin —
    /// yazdıkları korunarak form yeniden gösterilir ve tekrar göndermesi istenir.
    /// </summary>
    [HttpPost("/iletisim")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Index(ContactFormModel form)
    {
        var content = _content.Get();
        ViewData["Content"] = content;
        SeoText.Contact(content).ApplyTo(ViewData);

        if (!await _antiforgery.IsRequestValidAsync(HttpContext))
        {
            ViewData["FormError"] = "Sayfa uzun süre açık kaldığı için güvenlik doğrulaması yenilendi. Mesajınız duruyor — lütfen bir kez daha gönderin.";
            ModelState.Clear();
            return View(form);
        }

        // Honeypot dolduysa bot demektir: başarı göster, kaydetme.
        if (!string.IsNullOrWhiteSpace(form.Website))
        {
            TempData["ContactSuccess"] = content.Contact.FormSuccessMessage;
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
            return View(form);

        _content.AddMessage(new ContactMessage
        {
            Name = form.Name.Trim(),
            Email = form.Email.Trim(),
            Phone = (form.Phone ?? "").Trim(),
            Subject = string.IsNullOrWhiteSpace(form.Subject) ? "Konu belirtilmedi" : form.Subject.Trim(),
            Message = form.Message.Trim(),
            CreatedAt = DateTime.Now
        });

        TempData["ContactSuccess"] = content.Contact.FormSuccessMessage;
        return RedirectToAction(nameof(Index));
    }
}
