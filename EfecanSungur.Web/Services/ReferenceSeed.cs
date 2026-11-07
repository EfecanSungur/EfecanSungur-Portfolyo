using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// salurian.com/referanslar sayfasından 24 Ekim 2026'da aktarılan referanslar.
/// Ham tarama: tools/salurian-referanslar-2026-10-24.json — buradaki her bilgi o
/// sayfalarda yazanla sınırlıdır; yeni olgu eklenmedi.
/// Hem yeni kurulumlar (SeedData) hem de canlı sitedeki boş referans listesi
/// (ContentMigrations "2026-10-24-referanslar") bu listeyi kullanır.
/// </summary>
public static class ReferenceSeed
{
    /// <summary>Salurian rol paylaşımına göre Efecan Sungur'un projelerdeki payı.</summary>
    public const string DefaultRole = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın";

    /// <summary>
    /// 24 Ekim 2026'da doğrulanan erişim durumları: CheckList'in Render servisi sahibince
    /// askıya alınmış (503), Hedef Takip "Geçici olarak kapatıldı" sayfası gösteriyor.
    /// Kapalı bir adrese "Canlı siteyi ziyaret et" bağlantısı verilmez (LiveUrl boş).
    /// </summary>
    public const string ClosedNow = "Şu an erişime kapalı";
    public const string TemporarilyClosed = "Geçici olarak kapalı";

    // --- huawei-hedef-takip (ContentMigrations "2026-10-24-huawei-bag" de bu değerleri yazar)
    public const string HuaweiClient = "Huawei Türkiye saha ekibi";
    public const string HuaweiYear = "Haziran 2026";
    public const string HuaweiSummary =
        "Huawei Türkiye için geliştirdiğim, saha promotörlerinin hedef ve prim takibini yapan mobil uygulama; 8 Haziran 2026'da PWA ve Android APK olarak yayına girdi.";
    public const string HuaweiRole = "Uygulamanın geliştirilmesi ve yayına alınması";

    public static List<string> HuaweiBody() => new()
    {
        "Huawei Hedef Takip, Huawei Türkiye için geliştirdiğim mobil uygulama. Saha promotörlerinin hedeflerini ve primlerini takip etmesi için tasarlandı; Antalya ve çevresinde adımın duyulmasını sağlayan proje de bu oldu.",
        "Uygulamada bir komisyon hesap motoru ve gün sonunda WhatsApp üzerinden raporlama bulunuyor. ASP.NET Core 9 ve saf JavaScript ile bir PWA olarak geliştirildi; Android'de APK olarak kurulan bir WebView kabuğuyla çalışıyor.",
        "Uygulama 8 Haziran 2026'da yayına girdi; şu an geçici olarak erişime kapalı."
    };

    public static List<string> HuaweiHighlights() => new()
    {
        "Huawei Türkiye için geliştirdiğim mobil uygulama",
        "Promotör hedef ve prim takibi",
        "Komisyon hesap motoru",
        "Gün sonu WhatsApp raporlaması",
        "PWA ve Android APK (WebView kabuğu)",
        "8 Haziran 2026'da yayına girdi"
    };

    private static string Src(string slug) => "https://salurian.com/referans/" + slug;
    private static string Img(string slug) => "/img/referanslar/" + slug + ".webp";

    public static List<ReferenceItem> Build()
    {
        var list = new List<ReferenceItem>
        {
            new()
            {
                Slug = "vianoria-tekstil-giyim-aksesuar",
                Title = "Vianoria Tekstil, Giyim & Aksesuar",
                Category = "web", Kind = "E-ticaret sitesi",
                Client = "Vianoria", Year = "Eylül 2026", Status = "Yayında",
                Summary = "Vianoria için Salurian Company ekibiyle kurulan, PayTR ödeme entegrasyonlu alışveriş sitesi ve tanıtım platformu.",
                Body = new()
                {
                    "Vianoria; tekstil, giyim ve aksesuar alanında hizmet veren bir marka. Salurian Company olarak markaya, ürünlerin satıldığı bir alışveriş sitesiyle tanıtım platformunu tek çatı altında kurduk.",
                    "Sunucu tarafı C# ve ASP.NET Core ile yazıldı, veriler JSON tabanlı bir yapıda tutuluyor. Ödeme altyapısı olarak PayTR entegre edildi; sistem dosyalarının güvenliği kontrol altında tutuluyor.",
                    "Vianoria ile iş ortaklığımız 10 Eylül 2026'da başladı. Site vianoria.com.tr adresinde yayında."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core", "C#", "JSON" },
                Highlights = new()
                {
                    "PayTR ödeme entegrasyonu",
                    "Alışveriş sitesi ve tanıtım platformu bir arada",
                    "C# backend ve JSON tabanlı veri yapısı",
                    "10 Eylül 2026'dan bu yana iş ortaklığı"
                },
                LiveUrl = "https://www.vianoria.com.tr/",
                SourceUrl = Src("vianoria-tekstil-giyim-aksesuar"),
                ImagePath = Img("vianoria-tekstil-giyim-aksesuar"),
                ImageAlt = "Vianoria logosu",
                Featured = true,
            },
            new()
            {
                Slug = "vetmassan-veteriner-yogun-bakim-ve-masa",
                Title = "Vetmassan Veteriner Yoğun Bakım ve Masa",
                Category = "web", Kind = "Kurumsal web sitesi",
                Client = "Vetmassan", Year = "Eylül 2026", Status = "Yayında",
                Summary = "Veteriner yoğun bakım ve masa alanında çalışan Vetmassan için Salurian Company ekibiyle ASP.NET Core 9 üzerinde geliştirilen web sitesi.",
                Body = new()
                {
                    "Vetmassan, veteriner yoğun bakım ve masa alanında çalışan bir marka. Firma sahibi, internet sitesi için Salurian Company'yi tercih etti.",
                    "Site ASP.NET Core 9, C# ve Razor ile geliştirildi; içerik JSON tabanlı bir yapıda tutuluyor ve arayüz CSS ile hazırlandı.",
                    "Site vetmassan.com adresinde yayında."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core 9", "C#", "JSON", "CSS", "Razor" },
                Highlights = new()
                {
                    "ASP.NET Core 9 ve Razor altyapısı",
                    "JSON tabanlı içerik yapısı",
                    "vetmassan.com adresinde yayında"
                },
                LiveUrl = "https://www.vetmassan.com/",
                SourceUrl = Src("vetmassan-veteriner-yogun-bakim-ve-masa"),
                ImagePath = Img("vetmassan-veteriner-yogun-bakim-ve-masa"),
                ImageAlt = "Vetmassan web sitesinden ürün sayfası ekran görüntüsü",
                Featured = true,
            },
            new()
            {
                Slug = "atiye-tezcan",
                Title = "Atiye Tezcan",
                Category = "web", Kind = "Tanıtım ve medya sitesi",
                Client = "Atiye Tezcan · Antalya", Year = "Temmuz 2026", Status = "Yayında",
                Summary = "Türk halk müziği sanatçısı Atiye Tezcan için, tüm içeriği yönetim panelinden düzenlenebilen tanıtım ve medya sitesi.",
                Body = new()
                {
                    "Atiye Tezcan bir Türk halk müziği sanatçısı. Salurian Company olarak sanatçı için bir tanıtım ve medya sitesi geliştirdik.",
                    "Sitedeki tüm içerik yönetim panelinden düzenlenebiliyor. Proje ASP.NET Core 9 MVC, özel yazılmış CSS ve JSON tabanlı veri yapısıyla hazırlandı.",
                    "Site atiyetezcan.com adresinde yayında."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core 9", "MVC", "Custom CSS", "JSON" },
                Highlights = new()
                {
                    "Tüm içeriği düzenleyen yönetim paneli",
                    "Tanıtım ve medya odaklı yapı",
                    "atiyetezcan.com adresinde yayında"
                },
                LiveUrl = "https://atiyetezcan.com/",
                SourceUrl = Src("atiye-tezcan"),
                ImagePath = Img("atiye-tezcan"),
                ImageAlt = "Atiye Tezcan, arka planda bağlama ve geleneksel süslemelerle",
                Featured = true,
            },
            new()
            {
                Slug = "ayse-guler-kuafor",
                Title = "Ayşe Güler Kuaför",
                Category = "web", Kind = "Randevu sistemli kurumsal site",
                Client = "Ayşe Güler Kuaför & Güzellik", Year = "2026", Status = "Yayında",
                Summary = "Ayşe Güler Kuaför & Güzellik için geliştirilen, takvimli randevu sistemi, hizmet kataloğu ve yönetim paneli bulunan kurumsal site.",
                Body = new()
                {
                    "Ayşe Güler Kuaför & Güzellik için Salurian Company ekibiyle kurumsal bir web sitesi geliştirdik.",
                    "Sitede müşterilerin takvim üzerinden randevu alabildiği bir randevu sistemi, salonun hizmet kataloğu ve içeriği yöneten bir yönetim paneli bulunuyor. Proje ASP.NET Core 9 MVC ve JSON ile hazırlandı.",
                    "Site ayseguler.com.tr adresinde yayında."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core 9", "MVC", "Randevu Takvimi", "JSON" },
                Highlights = new()
                {
                    "Takvimli randevu sistemi",
                    "Hizmet kataloğu",
                    "Yönetim paneli"
                },
                LiveUrl = "https://ayseguler.com.tr/",
                SourceUrl = Src("ayse-guler-kuafor"),
                ImagePath = Img("ayse-guler-kuafor"),
                ImageAlt = "Ayşe Güler Kuaför web sitesinin ana sayfa ekran görüntüsü",
            },
            new()
            {
                Slug = "vetopya-veteriner-klinigi",
                Title = "Vetopya Veteriner Kliniği",
                Category = "web", Kind = "Klinik web sitesi",
                Client = "Vetopya Veteriner Kliniği", Year = "Eylül 2026", Status = "Yayında",
                Summary = "Vetopya Veteriner Kliniği'nin eski altyapılı sitesinin yerine kurulan, daha işlevli ve kullanışlı yeni web sitesi.",
                Body = new()
                {
                    "Vetopya Veteriner Kliniği'nin sahibi ve uzman hekimi, kliniğin web sitesi için Salurian Company ile çalışmayı tercih etti.",
                    "Eski bir altyapıyla çalışan sitenin yerine daha işlevli ve daha kullanışlı yeni bir site kuruldu. Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor.",
                    "Revizyonlarla birlikte siteye yeni özellikler eklenmesi planlanıyor. Site vetopya.com adresinde yayında."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
                Highlights = new()
                {
                    "Eski altyapının yerine yeni site",
                    "Revizyonlarla gelecek yeni özellikler",
                    "vetopya.com adresinde yayında"
                },
                LiveUrl = "https://vetopya.com",
                SourceUrl = Src("vetopya-veteriner-klinigi"),
                ImagePath = Img("vetopya-veteriner-klinigi"),
                ImageAlt = "Vetopya Veteriner Kliniği web sitesinin ana sayfası",
            },
            new()
            {
                Slug = "acay-kuyumculuk",
                Title = "Acay Kuyumculuk",
                Category = "web", Kind = "Kurumsal web sitesi",
                Client = "Acay Kuyumculuk · Döşemealtı, Antalya", Year = "Ağustos 2025", Status = "Yayında",
                Summary = "Antalya Döşemealtı'nın öne çıkan kuyumcularından Acay Kuyumculuk için SEO ve GEO altyapısıyla hazırlanmış kurumsal web sitesi.",
                Body = new()
                {
                    "Acay Kuyumculuk, Antalya'nın Döşemealtı bölgesinde kuyumculuk sektörünün öne çıkan işletmelerinden biri.",
                    "İşletme için hazırlanan site, arama motorlarında (SEO) ve yapay zekâ destekli aramada (GEO) görünür olacak şekilde kurgulanmış bir altyapıyla geliştirildi. Proje ASP.NET Core 9 ve JSON tabanlı veri yapısı üzerinde çalışıyor.",
                    "Site acaykuyumculuk.com adresinde yayında ve Salurian Company referansları arasında yer alıyor."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core 9", "JSON", "Web UI" },
                Highlights = new()
                {
                    "SEO ve GEO altyapısı",
                    "Döşemealtı / Antalya yerel işletmesi",
                    "acaykuyumculuk.com adresinde yayında"
                },
                LiveUrl = "https://acaykuyumculuk.com/",
                SourceUrl = Src("acay-kuyumculuk"),
                ImagePath = Img("acay-kuyumculuk"),
                ImageAlt = "Acay Kuyumculuk logosu: altın yüzük ve el yazısıyla Acay",
                Featured = true,
            },
            new()
            {
                Slug = "hegza-tasarim",
                Title = "Hegza Tasarım",
                Category = "web", Kind = "Kurumsal web sitesi",
                Client = "Hegza Tasarım", Year = "Kasım 2025", Status = "Yayında",
                Summary = "Hegza Tasarım firmasının aktif olarak kullandığı, C# backend altyapılı kurumsal web sitesi.",
                Body = new()
                {
                    "Hegza Tasarım için geliştirilen bu sistem, firmanın aktif olarak kullandığı web sitesi. Yapımı Kasım 2025'te tamamlandı.",
                    "Sunucu tarafı C# ve ASP.NET Core ile yazıldı; arayüzde Razor ve JavaScript kullanıldı. Salurian Company referans sayfasına göre site, yayına alındığı günden bu yana firmanın müşteri potansiyelini artırmaya devam ediyor.",
                    "Site hegzatasarim.com adresinde yayında."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core", "Razor", "JavaScript", "UI/UX" },
                Highlights = new()
                {
                    "C# backend altyapısı",
                    "Kasım 2025'te tamamlandı",
                    "Firmanın aktif olarak kullandığı site"
                },
                LiveUrl = "https://hegzatasarim.com/",
                SourceUrl = Src("hegza-tasarim"),
                ImagePath = Img("hegza-tasarim"),
                ImageAlt = "Hegza Tasarım web sitesinden baretli ve iş kıyafetli usta görseli",
                Featured = true,
            },
            new()
            {
                Slug = "beyond-gayrimenkul",
                Title = "Beyond Gayrimenkul",
                Category = "web", Kind = "Kurumsal gayrimenkul sitesi",
                Client = "Beyond Gayrimenkul · Ankara", Year = "2026", Status = "Yapım aşamasında",
                Summary = "Prestij konut, villa ve arsa portföyünü sergileyecek, portföy yönetimi için özel yönetim paneli olan kurumsal gayrimenkul sitesi.",
                Body = new()
                {
                    "Ankara'daki Beyond Gayrimenkul için Salurian Company ekibiyle kurumsal bir gayrimenkul sitesi geliştiriyoruz; yapım aşaması sürüyor.",
                    "Site prestij konut, villa ve arsa portföyünü sergileyecek. Portföyün yönetimi için özel bir yönetim paneli bulunuyor. Proje ASP.NET Core 9, C#, Razor, özel yazılmış CSS ve JSON ile hazırlanıyor."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core 9", "C#", "Razor", "Custom CSS", "JSON" },
                Highlights = new()
                {
                    "Konut, villa ve arsa portföyü",
                    "Portföy yönetimi için özel yönetim paneli",
                    "Yapım aşamasında"
                },
                SourceUrl = Src("beyond-gayrimenkul-yapim-asamasinda"),
            },
            new()
            {
                Slug = "checklist",
                Title = "CheckList",
                Category = "mobil", Kind = "Görev ve ajanda uygulaması",
                Client = "Salurian · İç ürün", Year = "Temmuz 2026", Status = ClosedNow,
                Summary = "Salurian Company'nin iç ürünü CheckList: üç katmanlı bildirim sistemine sahip, ücretsiz ortak görev ve ajanda uygulaması.",
                Body = new()
                {
                    "CheckList, Salurian Company'nin kendi iç ürünü olan ortak görev ve ajanda uygulaması. Üç katmanlı bir bildirim sistemi var.",
                    "Uygulamanın PWA, APK ve masaüstü sürümleri bulunuyor. ASP.NET Core 9, Electron ve JSON ile geliştirildi.",
                    "CheckList ücretsiz ve KVKK kurallarına uygun tasarlandı: kullanıcıların verileri kaydedilmiyor, izlenmiyor ve okunamıyor."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core 9", "PWA", "APK", "Electron", "JSON" },
                Highlights = new()
                {
                    "Üç katmanlı bildirim sistemi",
                    "PWA, APK ve masaüstü sürümü",
                    "Ücretsiz ve KVKK'ya uygun"
                },
                SourceUrl = Src("checklist"),
                ImagePath = Img("checklist"),
                ImageAlt = "CheckList uygulamasının tanıtım ekranı",
            },
            new()
            {
                Slug = "huawei-hedef-takip",
                Title = "Huawei Hedef Takip",
                Category = "mobil", Kind = "Hedef ve prim takip uygulaması",
                // Efecan'ın "Huawei Türkiye için geliştirdiğim mobil uygulama" beyanı = bu proje (teyit: 24 Eki 2026).
                // 8 Haziran 2026'da yayına girdi — Salurian'ın kuruluşundan önce: kendi projesi (OwnProject).
                Client = HuaweiClient, Year = HuaweiYear, Status = TemporarilyClosed,
                Summary = HuaweiSummary,
                Body = HuaweiBody(),
                Role = HuaweiRole,
                OwnProject = true,
                Stack = new() { "ASP.NET Core 9", "Saf JavaScript", "PWA", "APK" },
                Highlights = HuaweiHighlights(),
                SourceUrl = Src("huawei-hedef-takip"),
                ImagePath = Img("huawei-hedef-takip"),
                ImageAlt = "Hedef Takip uygulamasının Android/Huawei APK ve Windows indirme ekranı",
                Featured = true,
            },
            new()
            {
                Slug = "promotor-tecno-turkiye",
                Title = "Promotör Tecno Türkiye",
                Category = "mobil", Kind = "Saha satış ve prim yönetim sistemi",
                Client = "Tecno Türkiye", Year = "Ağustos 2026", Status = "Yayında",
                Summary = "Tecno Türkiye sahasında 302 promotörün aktif olarak kullandığı satış, mesai, check-in/check-out, stok ve prim takip sistemi.",
                Body = new()
                {
                    "Promotör Tecno Türkiye, sahada 302 promotörün aktif olarak kullandığı bir satış takip sistemi. Salurian Company ekibiyle geliştirildi.",
                    "Sistem satış takibi, mesai, check-in ve check-out, stok takibi ve prim hesaplama modüllerinden oluşuyor. Direktör, yönetici, müdür ve promotör olmak üzere farklı roller ve kategoriler bulunuyor.",
                    "Proje ASP.NET Core, C#, Razor, CSS ve JSON ile hazırlandı; promotortecno.com adresinde yayında."
                },
                Role = DefaultRole,
                Stack = new() { "ASP.NET Core", "C#", "JSON", "Razor", "CSS" },
                Highlights = new()
                {
                    "302 promotörün sahada aktif kullanımı",
                    "Satış, mesai, check-in/check-out, stok ve prim modülleri",
                    "Direktör, yönetici, müdür ve promotör rolleri"
                },
                LiveUrl = "https://promotortecno.com/",
                SourceUrl = Src("promotor-tecno-turkiye"),
                ImagePath = Img("promotor-tecno-turkiye"),
                ImageAlt = "Tecno Hedef Takip promotör ve saha yönetim paneli giriş ekranı",
            },
        };

        for (var i = 0; i < list.Count; i++) list[i].Order = i;
        return list;
    }

    /// <summary>
    /// Yeni kurulumun tam listesi: yukarıdaki 11 salurian.com referansı + 28 Ekim 2026'da eklenen
    /// tüm projeler (ReferenceSeedV3; çoğu gizli taslak). Build() migrasyonlar için olduğu gibi kalır.
    /// </summary>
    public static List<ReferenceItem> BuildAll()
    {
        var list = Build();
        list.AddRange(ReferenceSeedV3.Build());
        for (var i = 0; i < list.Count; i++) list[i].Order = i;
        return list;
    }
}
