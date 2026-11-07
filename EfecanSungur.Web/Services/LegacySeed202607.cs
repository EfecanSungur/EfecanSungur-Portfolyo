using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// 31 Temmuz 2026 tohum içeriğinin BİREBİR kopyası (taban commit 7c44fd4). DEĞİŞTİRMEYİN.
/// ContentMigrations, canlıdaki bir değerin hâlâ bu eski tohum değeri olup olmadığını buna bakarak anlar.
/// </summary>
public static class LegacySeed202607
{
    public static SiteContent Build()
    {
        var c = new SiteContent();

        // ----------------------------------------------------------- kimlik

        c.Settings.SiteName = "Efecan Sungur";
        c.Settings.OwnerName = "Efecan Sungur";
        c.Settings.Role = "Yazılım Geliştirici · Salurian Kurucu Ortağı";
        c.Settings.LogoWordmark = "EFECAN SUNGUR";
        c.Settings.MetaDescription =
            "Efecan Sungur — mobil uygulama, masaüstü uygulama ve web geliştiricisi. " +
            "Salurian Company kurucu ortağı. Huawei Türkiye mobil uygulaması. Antalya.";
        c.Settings.MetaKeywords =
            "Efecan Sungur, yazılım geliştirici, mobil uygulama, masaüstü uygulama, web tasarımı, " +
            "Salurian Company, salurian.com, Mahir Topuz, Huawei Türkiye, Antalya yazılım";
        c.Settings.FooterTagline = "Fikri çalışan bir ürüne çeviriyorum.";
        c.Settings.CopyrightText = "Efecan Sungur. Tüm hakları saklıdır.";

        // Gerçek müşteri yorumu gelene kadar referans bölümü kapalı.
        c.Settings.ShowQuotes = false;
        // Elde iki fotoğraf var, ikisi de hero ve hakkında bölümünde kullanılıyor.
        c.Settings.ShowGallery = false;

        // ------------------------------------------------------------- hero

        c.Hero.Eyebrow = "Efecan Sungur";
        c.Hero.TitleTop = "Fikri";
        c.Hero.TitleAccent = "çalışan";
        c.Hero.TitleBottom = "ürüne çeviriyorum";
        c.Hero.Subtitle =
            "4 yıldır mobil uygulama, masaüstü program ve web tarafında ürün geliştiriyorum. " +
            "2026'da ortağım Mahir Topuz ile Salurian Company'yi kurduk; markalara ihtiyaç duydukları " +
            "dijital ürünü fikirden yayına kadar tek elden teslim ediyoruz.";
        c.Hero.PrimaryCtaText = "Beni tanıyın";
        c.Hero.PrimaryCtaUrl = "/hakkinda";
        c.Hero.SecondaryCtaText = "İletişime geçin";
        c.Hero.SecondaryCtaUrl = "/iletisim";
        // Tohum görselleri /img/ altında: uploads klasörü yayına dâhil edilmez
        // (yeniden yayınlarken kullanıcının yüklediği görseller silinmesin diye).
        c.Hero.ImagePath = "/img/efecan-hero.jpg";
        c.Hero.ImageAlt = "Efecan Sungur";
        c.Hero.ScrollHint = "Keşfet";
        c.Hero.MarqueeText =
            "Mobil Uygulama • Masaüstü Uygulama • Web Geliştirme • Huawei Türkiye Projesi • " +
            "Salurian Company • Antalya";

        // --------------------------------------------------------- hakkında

        c.About.Eyebrow = "Hakkında";
        c.About.Title = "Efecan Sungur kimdir?";
        c.About.Lead =
            "4 yıldır yazılımla iç içe; mobil uygulamadan masaüstüne, web tasarımından ürünü " +
            "pazarlamaya kadar işin her aşamasında bizzat bulunmuş bir geliştirici.";
        c.About.Paragraphs = new List<string>
        {
            "Yazılıma 4 yıl önce başladım. Bu süre boyunca yalnızca kod yazmakla kalmadım; mobil uygulamalar, masaüstü programlar ve web siteleri geliştirip bunları pazarladım. Bir ürünün fikir aşamasından kullanıcıya ulaşana kadar geçirdiği her adımı kendi ellerimle yaşadım — ve bu, bugün bir işe baktığımda nerede tıkanacağını önceden görmemi sağlıyor.",
            "Huawei Türkiye için geliştirdiğim mobil uygulama benim için bir dönüm noktası oldu. O projeden sonra Antalya ve çevresinde adım duyulmaya başladı; bugün pek çok markayla o vesileyle tanıştım.",
            "2026 yılında ortağım Mahir Topuz ile birlikte Salurian Company'yi (salurian.com) kurduk. Kuruluşumuzdan bu yana birbirinden farklı işlere imza attık. Artık verdiğim hizmetin tamamı bu çatı altında, kurumsal bir yapı ve sözleşmeyle yürüyor.",
            "Markalar bana genellikle iki noktada ulaşıyor: yeni açıldıklarında ve müşteriye ihtiyaç duyduklarında. Her iki durumda da yaklaşımım aynı — önce işin gerçekte ne olduğunu anlamak, sonra buna en sade çözümü kurmak. Gösterişli ama işe yaramayan bir ürün yerine, sessizce işini yapan bir ürün her zaman daha değerlidir."
        };
        c.About.ImagePath = "/img/efecan-portre.jpg";
        c.About.ImageAlt = "Efecan Sungur portre";
        c.About.QuoteText = "İyi yazılım, kullanıcının varlığını fark etmediği yazılımdır.";
        c.About.SignatureText = "Efecan Sungur";
        c.About.CtaText = "Birlikte çalışalım";
        c.About.CtaUrl = "/iletisim";

        c.About.Facts = Ordered(new List<AboutFact>
        {
            new() { Label = "Uzmanlık",   Value = "Mobil · masaüstü · web uygulama geliştirme" },
            new() { Label = "Deneyim",    Value = "4 yıl" },
            new() { Label = "Şirket",     Value = "Salurian Company — kurucu ortak" },
            new() { Label = "Ortak",      Value = "Mahir Topuz" },
            new() { Label = "Öne çıkan",  Value = "Huawei Türkiye mobil uygulaması" },
            new() { Label = "Konum",      Value = "Antalya / Türkiye" },
            new() { Label = "Çalışma",    Value = "Uzaktan & yerinde" },
        });

        // --------------------------------------------------------- iletişim

        c.Contact.Eyebrow = "İletişim";
        c.Contact.Title = "Konuşalım";
        c.Contact.Lead =
            "Yeni bir marka mı kuruyorsunuz, yoksa mevcut işinizin dijital tarafını mı büyütmek " +
            "istiyorsunuz? İkisi de aynı adrese gider.";
        c.Contact.Phone = "+905318328678";
        c.Contact.PhoneDisplay = "0531 832 86 78";
        c.Contact.WhatsAppNumber = "905318328678";
        c.Contact.WhatsAppMessage = "Merhaba Efecan Bey, web siteniz üzerinden ulaşıyorum.";
        c.Contact.Email = "efecansungur@icloud.com";
        c.Contact.Address = "Antalya, Türkiye";
        c.Contact.WorkingHours = "Hafta içi 09:00 – 18:00";
        c.Contact.FormTitle = "Mesaj bırakın";
        c.Contact.FormNote = "Genellikle aynı gün içinde dönüş yapıyorum.";
        c.Contact.FormSuccessMessage = "Mesajınız ulaştı. En kısa sürede size dönüş yapacağım.";

        // ------------------------------------------------------------ menü

        c.Nav = Ordered(new List<NavItem>
        {
            new() { Label = "Ana Sayfa", Url = "/" },
            new() { Label = "Hakkında",  Url = "/hakkinda" },
            new() { Label = "İletişim",  Url = "/iletisim" },
        });

        // ------------------------------------------------------ istatistik

        c.Stats = Ordered(new List<StatItem>
        {
            new() { Value = "4",    Suffix = "",  Label = "Yıllık yazılım deneyimi" },
            new() { Value = "3",    Suffix = "",  Label = "Mobil · masaüstü · web" },
            new() { Value = "2026", Suffix = "",  Label = "Salurian Company kuruluşu" },
            new() { Value = "2",    Suffix = "",  Label = "Kurucu ortak" },
        });

        // -------------------------------------------------------- uzmanlık

        c.Expertise = Ordered(new List<ExpertiseItem>
        {
            new() { Icon = "message", Title = "Mobil Uygulama",
                    Description = "Kullanıcının cebine giren uygulamalar. Huawei Türkiye için geliştirdiğim proje dâhil, fikirden mağazada yayına kadar tüm süreci yürütüyorum." },
            new() { Icon = "layers", Title = "Masaüstü Uygulama",
                    Description = "İşletmenin kendi süreçlerine göre kurgulanan, internet bağlantısına muhtaç olmayan masaüstü programlar." },
            new() { Icon = "globe", Title = "Web Geliştirme",
                    Description = "Markanın ilk izlenimini belirleyen kurumsal siteler. Mobil ve masaüstü görünüm ayrı ayrı düşünülerek, tasarımdan yayına." },
            new() { Icon = "spark", Title = "Yeni Marka Kurulumu",
                    Description = "Yeni açılan markaların dijital varlığını sıfırdan kurmak: kimlik, site, uygulama ve yayına alma — hepsi tek elden." },
            new() { Icon = "users", Title = "Müşteri Kazanımı",
                    Description = "Ürünü geliştirmek işin yarısı. 4 yıldır geliştirdiğim ürünleri bizzat pazarladım; markaları müşteriyle buluşturan tarafı da biliyorum." },
            new() { Icon = "shield", Title = "Salurian Company",
                    Description = "Tüm bu hizmetleri, ortağım Mahir Topuz ile kurduğumuz Salurian Company çatısı altında kurumsal güvenceyle veriyorum." },
        });

        // -------------------------------------------------------- yolculuk

        c.Timeline = Ordered(new List<TimelineItem>
        {
            new() { Year = "2022", Title = "İlk satır",
                    Description = "Yazılıma adım attım. İlk dönemi kendi projelerimi geliştirerek, işin temelini kurarak geçirdim." },
            new() { Year = "Sonrası", Title = "Geliştir ve pazarla",
                    Description = "Mobil uygulamalar, masaüstü programlar ve web siteleri geliştirdim — ve bunları yalnızca yazmakla kalmayıp pazarladım. Ürünün kullanıcıyla buluşma tarafını burada öğrendim." },
            new() { Year = "Dönüm", Title = "Huawei Türkiye",
                    Description = "Huawei Türkiye için geliştirdiğim mobil uygulama, Antalya ve çevresinde adımın duyulmasını sağladı. Sonrasında gelen işlerin çoğu bu projeye dayanıyor." },
            new() { Year = "2026", Title = "Salurian Company",
                    Description = "Ortağım Mahir Topuz ile Salurian Company'yi kurduk. salurian.com üzerinden kurumsal olarak hizmet vermeye başladık ve kuruluşumuzdan bu yana pek çok projeye imza attık." },
            new() { Year = "Bugün", Title = "Seçili markalarla",
                    Description = "Yeni açılan ve müşteriye ihtiyaç duyan markalarla, şirketimiz üzerinden uçtan uca çalışıyorum." },
        });

        // ------------------------------------------------------------- sss

        c.Faq = Ordered(new List<FaqItem>
        {
            new() { Question = "Ne tür işler yapıyorsunuz?",
                    Answer = "Mobil uygulama, masaüstü uygulama ve web sitesi geliştiriyorum. Üçünü de fikir aşamasından yayına alma aşamasına kadar uçtan uca yürütüyorum; gerektiğinde ürünün pazarlama tarafında da yer alıyorum." },
            new() { Question = "Salurian Company nedir?",
                    Answer = "2026 yılında ortağım Mahir Topuz ile kurduğumuz yazılım şirketi. Verdiğim hizmetlerin tamamı bu çatı altında, kurumsal yapı ve sözleşmeyle yürüyor. Detaylar için: salurian.com" },
            new() { Question = "Yeni bir marka kuruyorum, nereden başlamalıyım?",
                    Answer = "Doğru yerdesiniz — markalar bana en çok yeni açıldıklarında ulaşıyor. İlk adım kısa bir tanışma görüşmesi: ne yaptığınızı ve kime ulaşmak istediğinizi konuşuyoruz, ardından size özel kapsamı ve teklifi çıkarıyorum." },
            new() { Question = "Antalya dışından çalışıyor musunuz?",
                    Answer = "Evet. Merkezim Antalya ama süreçlerin büyük bölümü uzaktan yürüyor. Toplantı ve teslim aşamalarında gerektiğinde yerinde de oluyorum." },
            new() { Question = "En hızlı nasıl ulaşırım?",
                    Answer = "En hızlısı WhatsApp: 0531 832 86 78. Dilerseniz efecansungur@icloud.com adresine yazabilir ya da iletişim sayfasındaki formu doldurabilirsiniz — genellikle aynı gün dönüş yapıyorum." },
        });

        // -------------------------------------------------------- bağlantı

        c.Socials = Ordered(new List<SocialLink>
        {
            new() { Label = "Salurian Company", Icon = "link", Url = "https://salurian.com" },
        });

        // Referans ve galeri listeleri bilerek boş — panelden gerçek içerik eklenecek.
        c.Quotes = new List<QuoteItem>();
        c.Gallery = new List<GalleryItem>();

        return c;
    }

    private static List<T> Ordered<T>(List<T> list) where T : ContentItem
    {
        for (var i = 0; i < list.Count; i++) list[i].Order = i;
        return list;
    }
}
