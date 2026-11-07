using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// 24 Ekim 2026 ilk v2 tohumunun (düzeltmelerden ÖNCEKİ hâli) değişen değerleri, birebir.
/// ContentMigrations "2026-10-24-v2-duzeltme" bir alanı YALNIZCA hâlâ buradaki değere
/// eşitse yeni değere çevirir. DEĞİŞTİRME — karşılaştırma tabanıdır.
/// (Bu sürümle kurulmuş bir örnek: tools/fixtures/content-2026-10-v2a.json)
/// </summary>
public static class LegacySeedV2a
{
    public const string MetaDescription =
        "Efecan Sungur, Antalya'da web, mobil ve masaüstü yazılım geliştiren backend developer ve Salurian Company kurucu ortağı. Projeleri ve referansları.";

    public const string FooterTagline = "Fikri çalışan bir ürüne çeviriyorum.";

    public const string AboutLead =
        "Antalya'da çalışan bir backend developer ve Salurian Company'nin kurucu ortağı. 2022'den bu yana " +
        "yazılımın hem geliştirme hem pazarlama tarafında.";

    public static List<string> AboutParagraphs() => new()
    {
        "Yazılıma 2022'de başladım. O günden bu yana mobil uygulamalar, masaüstü programlar ve web projeleri geliştirdim; bunları yalnızca yazmakla kalmayıp pazarladım da. Bir ürünün fikirden kullanıcıya uzanan yolunu iki taraftan da tanımak, bugün bir işe baktığımda nerede tıkanacağını önceden görmemi sağlıyor.",
        "Şubat 2024'ten bu yana serbest olarak web projeleri ve içerik yönetim sistemleri geliştiriyorum. Ocak–Ekim 2025 arasında Piksel Akademi'de Yazılım Uzmanlığı Eğitimi aldım; C#, ASP.NET Core, HTML & CSS, SQL ve MSSQL üzerine çalıştım.",
        "Huawei Türkiye için geliştirdiğim mobil uygulama benim için bir dönüm noktası oldu. Promotörlerin hedef ve prim takibini yapan bu uygulamadan sonra Antalya ve çevresinde adım duyulmaya başladı; bugün çalıştığım pek çok markayla o vesileyle tanıştım.",
        "2026'da ortağım Mahir Topuz ile Salurian Company'yi (salurian.com) kurduk. Ekipte altyapı ve mimari tarafındayım: ASP.NET Core ve C# ile veri modelini, yönetim paneli altyapısını, sunucu kurulumunu, yayını ve bakımı üstleniyorum. Mahir ise ürün, tasarım ve geliştirme tarafını yürütüyor.",
        "Markalar bana çoğunlukla yeni açıldıklarında ve müşteriye ihtiyaç duyduklarında ulaşıyor. Hizmetimi Salurian Company üzerinden veriyorum ve yaklaşımım her seferinde aynı: önce işin gerçekte ne olduğunu anlamak, sonra ona en sade ve sağlam çözümü kurmak."
    };

    public static List<(string Title, string Description)> Expertise() => new()
    {
        ("Web Geliştirme", "ASP.NET Core ve C# ile kurumsal siteler, e-ticaret ve yönetim panelli platformlar. Vianoria, Vetmassan ve Acay Kuyumculuk bu işlerden birkaçı."),
        ("Backend ve Mimari", "Veri modeli, API ve iş kuralları. Bir projeye önce veriyi netleştirerek başlıyorum; ekranlar sağlam bir iskeletin üzerine kurulur."),
        ("Mobil Uygulama", "Sahada çalışan PWA ve APK uygulamaları. Huawei Hedef Takip ve Promotör Tecno Türkiye, saha ekipleri için geliştirilen projelerden."),
        ("Masaüstü Uygulama", "İnternet bağlantısına muhtaç olmadan çalışması gereken, işletmenin kendi süreçlerine göre kurgulanan masaüstü programlar."),
        ("Yönetim Paneli ve Yayın", "Sitedeki her içeriği düzenleyen yönetim panelleri, sunucu kurulumu, yayına alma ve teslim sonrası bakım."),
        ("Yeni Marka Kurulumu", "Yeni açılan markaların dijital varlığını Salurian Company üzerinden uçtan uca kurmak: site, uygulama, arama görünürlüğü ve yayın."),
    };

    public static List<(string Year, string Title, string Description)> Timeline() => new()
    {
        ("2022", "İlk satır", "Yazılıma adım attım; mobil, masaüstü ve web projeleri geliştirip pazarlamaya başladım."),
        ("Şubat 2024", "Serbest web geliştirme", "Serbest (freelance) olarak birden fazla web projesi ve içerik yönetim sistemi geliştirdim."),
        ("2025", "Piksel Akademi", "Ocak–Ekim 2025 arasında Piksel Akademi'de Yazılım Uzmanlığı Eğitimi aldım: C#, ASP.NET Core, HTML & CSS, SQL ve MSSQL."),
        ("Dönüm", "Huawei Türkiye", "Huawei Türkiye için geliştirdiğim mobil uygulama, Antalya ve çevresinde adımın duyulmasını sağladı."),
        ("2026", "Salurian Company", "Ortağım Mahir Topuz ile Salurian Company'yi kurduk; kuruluştan bu yana pek çok projeye imza attık."),
        ("Bugün", "Seçili markalarla", "Yeni açılan ve müşteriye ihtiyaç duyan markalarla Salurian Company üzerinden uçtan uca çalışıyorum."),
    };

    public static List<(string Question, string Answer)> Faq() => new()
    {
        ("Efecan Sungur kimdir?", "Efecan Sungur, Antalya'da çalışan bir yazılım geliştirici (backend developer) ve Salurian Company'nin kurucu ortağıdır. 2022'den bu yana mobil, masaüstü ve web projeleri geliştiriyor. Huawei Türkiye için geliştirdiği mobil uygulamayla Antalya çevresinde tanındı."),
        ("Efecan Sungur ne iş yapar?", "Web siteleri, mobil uygulamalar ve masaüstü yazılımlar geliştirir. Salurian Company'de altyapı ve mimari tarafındadır: ASP.NET Core ve C# ile veri modeli, yönetim paneli altyapısı, sunucu kurulumu, yayın ve bakım işlerini üstlenir."),
        ("Salurian Company nedir?", "Salurian Company, Efecan Sungur ve Mahir Topuz'un 2026'da kurduğu, Antalya merkezli bir yazılım şirketidir. Kurumsal web sitesi, mobil uygulama ve masaüstü yazılım geliştirir. Efecan Sungur hizmetlerini bu şirket üzerinden verir; ayrıntılar salurian.com adresinde."),
        ("Efecan Sungur hangi projeleri geliştirdi?", "Salurian Company ekibiyle geliştirilen projeler arasında Vianoria e-ticaret sitesi; Vetmassan, Acay Kuyumculuk, Hegza Tasarım, Atiye Tezcan ve Ayşe Güler Kuaför web siteleri; Huawei Hedef Takip ve Promotör Tecno Türkiye saha uygulamaları bulunuyor. Tam liste Referanslar sayfasında."),
        ("Yeni açılan bir marka için ne yapıyorsunuz?", "Markalar bize çoğunlukla yeni açıldıklarında ve müşteriye ihtiyaç duyduklarında ulaşıyor. Kısa bir tanışma görüşmesinde işi ve hedef kitleyi konuşuyor, ardından ihtiyaca göre site, uygulama ve yayın kapsamını Salurian Company üzerinden teklif olarak çıkarıyoruz."),
        ("Efecan Sungur'a nasıl ulaşılır?", "En hızlı yol WhatsApp ya da telefon: 0531 832 86 78. E-posta adresi efecansungur@icloud.com; iletişim sayfasındaki form da kullanılabilir. Genellikle aynı gün dönüş yapılır."),
        ("Antalya dışında çalışıyor musunuz?", "Evet. Merkez Antalya, ancak süreçlerin büyük bölümü uzaktan yürüyor; Ankara'daki Beyond Gayrimenkul projesi de bunlardan biri. Gerektiğinde toplantı ve teslim aşamalarında yerinde bulunuluyor."),
    };

    /// <summary>İlk v2 tohumundaki bir referansın düzeltilen alanları (null = o alan değişmedi).</summary>
    public sealed record RefOld(
        string Slug,
        string? Summary = null,
        List<string>? Body = null,
        string? Role = null,
        string? Status = null,
        string? LiveUrl = null);

    public static List<RefOld> References() => new()
    {
        new("vianoria-tekstil-giyim-aksesuar",
            Summary: "Vianoria için Salurian Company ekibiyle kurduğumuz, PayTR ödeme entegrasyonlu alışveriş sitesi ve tanıtım platformu.",
            LiveUrl: "https://vianoria.com.tr"),
        new("vetmassan-veteriner-yogun-bakim-ve-masa",
            Summary: "Veteriner yoğun bakım ve masa alanında çalışan Vetmassan'ın, Salurian Company ekibiyle ASP.NET Core 9 üzerinde geliştirdiğimiz web sitesi.",
            LiveUrl: "https://vetmassan.com"),
        new("ayse-guler-kuafor",
            Summary: "Ayşe Güler Kuaför & Güzellik için geliştirilen; takvimli randevu sistemi, hizmet kataloğu ve yönetim paneli bulunan kurumsal site."),
        new("checklist",
            Body: new()
            {
                "CheckList, Salurian Company'nin kendi iç ürünü olan ortak görev ve ajanda uygulaması. Üç katmanlı bir bildirim sistemi var.",
                "Uygulama PWA ve APK olarak dağıtılıyor, ayrıca masaüstü sürümü bulunuyor. ASP.NET Core 9, Electron ve JSON ile geliştirildi.",
                "CheckList ücretsiz ve KVKK kurallarına uygun tasarlandı: kullanıcıların verileri kaydedilmiyor, izlenmiyor ve okunamıyor."
            },
            Status: "Yayında",
            LiveUrl: "https://checklist-vx20.onrender.com/indir"),
        new("huawei-hedef-takip",
            Summary: "Huawei saha promotörlerinin hedef ve prim takibi için geliştirilen mobil uygulama; komisyon hesap motoru ve gün sonu WhatsApp raporu içeriyor.",
            Body: new()
            {
                "Huawei Hedef Takip, promotörlerin hedeflerini ve primlerini takip etmesi için geliştirilmiş bir mobil uygulama. Efecan Sungur'un Huawei Türkiye için geliştirdiği ve Antalya çevresinde tanınmasını sağlayan mobil uygulama bu projedir.",
                "Uygulamada bir komisyon hesap motoru ve gün sonunda WhatsApp üzerinden raporlama bulunuyor. ASP.NET Core 9 ve saf JavaScript ile geliştirildi; PWA ve APK olarak dağıtılıyor.",
                "Proje bugün Salurian Company referansları arasında yer alıyor."
            },
            Role: "Uygulamanın geliştirilmesi: altyapı ve mimari, sunucu kurulumu ve yayın",
            Status: "Yayında",
            LiveUrl: "https://hedef-takip.onrender.com/indir"),
    };

    /// <summary>İlk v2'de "yapım aşamasında" durumunu kalıcı adrese gömen slug → yeni slug.</summary>
    public static readonly IReadOnlyDictionary<string, string> SlugRenames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["beyond-gayrimenkul-yapim-asamasinda"] = "beyond-gayrimenkul",
    };
}
