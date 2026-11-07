using System.ComponentModel.DataAnnotations;
using EfecanSungur.Web.Services;

namespace EfecanSungur.Web.Models;

/// <summary>Panel formlarında tek bir alanı çizmek için kullanılır.</summary>
public class FieldRenderModel
{
    public FieldDef Field { get; init; } = new();
    public object Target { get; init; } = new();

    public string Value => ContentSchema.ReadString(Target, Field.Name);
    public bool BoolValue => ContentSchema.Read(Target, Field.Name) is true;
}

public class ContactFormModel
{
    [Required(ErrorMessage = "Adınızı yazar mısınız?")]
    [StringLength(80, ErrorMessage = "Ad en fazla 80 karakter olabilir.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "E-posta adresiniz gerekli.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi yazın.")]
    [StringLength(120, ErrorMessage = "E-posta en fazla 120 karakter olabilir.")]
    public string Email { get; set; } = "";

    // İsteğe bağlı alanlar NULLABLE olmalı: <Nullable>enable</Nullable> ile null olamayan string
    // MVC'de örtük [Required] sayılır ve boş bırakılan Telefon/Konu formu reddettirir.
    [StringLength(30, ErrorMessage = "Telefon en fazla 30 karakter olabilir.")]
    public string? Phone { get; set; }

    [StringLength(120, ErrorMessage = "Konu en fazla 120 karakter olabilir.")]
    public string? Subject { get; set; }

    [Required(ErrorMessage = "Mesajınızı yazmayı unuttunuz.")]
    [StringLength(3000, MinimumLength = 5, ErrorMessage = "Mesaj en az 5 karakter olmalı.")]
    public string Message { get; set; } = "";

    /// <summary>Bot tuzağı — insanlar bu alanı görmez, doldurulmuşsa mesaj sessizce yutulur.</summary>
    public string? Website { get; set; }
}

public class LoginModel
{
    [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Şifre gerekli.")]
    public string Password { get; set; } = "";
}

public class ErrorViewModel
{
    public int Code { get; set; } = 500;
    public string Title { get; set; } = "Bir şeyler ters gitti";
    public string Message { get; set; } = "Beklenmeyen bir hata oluştu.";
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

public record BreadcrumbItem(string Name, string Url);

public class ReferenceDetailViewModel
{
    public SiteContent Content { get; set; } = new();
    public ReferenceItem Item { get; set; } = new();
    public ReferenceItem? Previous { get; set; }
    public ReferenceItem? Next { get; set; }
}
