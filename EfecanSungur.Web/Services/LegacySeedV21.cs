namespace EfecanSungur.Web.Services;

/// <summary>
/// v2.1 tohumunun (24 Ekim 2026, Huawei bağı kurulmadan önceki hâli) "2026-10-24-huawei-bag"
/// migrasyonunun değiştirdiği değerleri, birebir. Migrasyon bir alanı YALNIZCA hâlâ buradaki
/// değere eşitse yeni değere çevirir. DEĞİŞTİRME — karşılaştırma tabanıdır.
/// (Bu sürümle kurulmuş bir örnek: tools/fixtures/content-2026-10-v21.json)
/// </summary>
public static class LegacySeedV21
{
    public const string ShortBio =
        "Efecan Sungur, Antalya'da çalışan bir yazılım geliştirici (backend developer) ve 2026'da Mahir Topuz ile " +
        "kurduğu Salurian Company'nin kurucu ortağıdır. 2022'den bu yana mobil, masaüstü ve web projeleri " +
        "geliştirip pazarlıyor; Huawei Türkiye için geliştirdiği mobil uygulamayla Antalya çevresinde tanındı. " +
        "Piksel Akademi'de Yazılım Uzmanlığı Eğitimi aldı.";

    /// <summary>Hakkında 3. paragraf (liste içinde öğe olarak aranır).</summary>
    public const string HuaweiParagraph =
        "Huawei Türkiye için geliştirdiğim mobil uygulama benim için bir dönüm noktası oldu; Antalya ve çevresinde adım bu uygulamayla duyulmaya başladı.";

    public const string FactLabel = "Öne çıkan";
    public const string FactValue = "Huawei Türkiye mobil uygulaması";

    /// <summary>Zaman çizgisi maddesi başlıkla bulunur; yıl ve açıklama ayrı ayrı karşılaştırılır.</summary>
    public const string TimelineTitle = "Huawei Türkiye";
    public const string TimelineYear = "Dönüm";
    public const string TimelineDescription =
        "Huawei Türkiye için geliştirdiğim mobil uygulama, Antalya ve çevresinde adımın duyulmasını sağladı.";

    public static List<(string Question, string Answer)> Faq() => new()
    {
        ("Efecan Sungur kimdir?", "Efecan Sungur, Antalya'da çalışan bir yazılım geliştirici (backend developer) ve Salurian Company'nin kurucu ortağıdır. 2022'den bu yana mobil, masaüstü ve web projeleri geliştiriyor. Huawei Türkiye için geliştirdiği mobil uygulamayla Antalya çevresinde tanındı."),
        ("Efecan Sungur hangi projeleri geliştirdi?", "Salurian Company referansları arasında Vianoria e-ticaret sitesi; Vetmassan, Acay Kuyumculuk, Hegza Tasarım, Atiye Tezcan ve Ayşe Güler Kuaför web siteleri; Huawei Hedef Takip ve Promotör Tecno Türkiye saha uygulamaları bulunuyor. Tam liste Referanslar sayfasında."),
    };

    // --- huawei-hedef-takip referansı
    public const string HuaweiSlug = "huawei-hedef-takip";
    public const string HuaweiClient = "Saha ekibi";
    public const string HuaweiYear = "2026";
    public const string HuaweiSummary =
        "Saha promotörlerinin hedef ve prim takibi için geliştirilen mobil uygulama; komisyon hesap motoru ve gün sonu WhatsApp raporu içeriyor.";
    public const string HuaweiRole = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın";

    public static List<string> HuaweiBody() => new()
    {
        "Huawei Hedef Takip, saha promotörlerinin hedeflerini ve primlerini takip etmesi için geliştirilmiş bir mobil uygulama.",
        "Uygulamada bir komisyon hesap motoru ve gün sonunda WhatsApp üzerinden raporlama bulunuyor. ASP.NET Core 9 ve saf JavaScript ile geliştirildi; PWA ve APK sürümleri bulunuyor.",
        "Proje bugün Salurian Company referansları arasında yer alıyor."
    };

    public static List<string> HuaweiHighlights() => new()
    {
        "Promotör hedef ve prim takibi",
        "Komisyon hesap motoru",
        "Gün sonu WhatsApp raporlaması",
        "PWA ve APK dağıtımı"
    };
}
