using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// content.json yoksa ilk açılışta üretilen başlangıç içeriği (v2 — 24 Ekim 2026).
/// Buradaki her şey yönetim panelinden değiştirilebilir.
///
/// DOĞRULUK KURALI: buradaki her kişisel bilgi Efecan Sungur'un kendi verdiği
/// bilgi notundan (31 Tem 2026), Eylül 2025 tarihli özgeçmişinin izin verilen
/// kısımlarından veya salurian.com'dan gelir. Yeni olgu, sayı, tarih uydurma.
///
/// Canlı kurulumlara bu metinler content.json üzerine yazılarak DEĞİL,
/// ContentMigrations ile (yalnız alan hâlâ Temmuz 2026 tohum değerindeyse) ulaşır.
/// </summary>
public static class SeedData
{
    public const string GscToken = "jX7nH7wTmZMoS2bw7IZLrNkMEQFqNIoLcTqLNPQ_3tM";

    public static SiteContent Build()
    {
        var c = new SiteContent();

        // ----------------------------------------------------------- kimlik

        c.Settings.SiteName = "Efecan Sungur";
        c.Settings.OwnerName = "Efecan Sungur";
        c.Settings.Role = "Yazılım Geliştirici · Salurian Kurucu Ortağı";
        c.Settings.LogoWordmark = "EFECAN SUNGUR";
        c.Settings.MetaDescription = V2.MetaDescription;
        c.Settings.MetaKeywords = V2.MetaKeywords;
        c.Settings.FooterTagline = V2.FooterTagline;
        c.Settings.CopyrightText = "Efecan Sungur. Tüm hakları saklıdır.";
        c.Settings.ShortBio = V2.ShortBio;
        c.Settings.AlumniOf = "Piksel Akademi";
        c.Settings.GoogleVerification = GscToken;

        c.Settings.ShowIntro = false;
        // Gerçek müşteri yorumu gelene kadar yorum bölümü kapalı — yorum uydurulmaz.
        c.Settings.ShowQuotes = false;
        c.Settings.ShowGallery = true;
        c.Settings.ShowReferences = true;

        // ------------------------------------------------------------- hero
        // Tasarım H1 olarak kişinin adını basar; aşağıdaki üç parça slogandır
        // (TitleAccent tek kelime, serif italik görünür).

        c.Hero.Eyebrow = V2.HeroEyebrow;
        c.Hero.TitleTop = V2.HeroTitleTop;
        c.Hero.TitleAccent = V2.HeroTitleAccent;
        c.Hero.TitleBottom = V2.HeroTitleBottom;
        c.Hero.Subtitle = V2.HeroSubtitle;
        c.Hero.PrimaryCtaText = "Referansları inceleyin";
        c.Hero.PrimaryCtaUrl = "/referanslar";
        c.Hero.SecondaryCtaText = "İletişime geçin";
        c.Hero.SecondaryCtaUrl = "/iletisim";
        // Tohum görselleri /img/ altında: uploads klasörü yayına dâhil edilmez (SiteImages).
        c.Hero.ImagePath = SiteImages.Hero;
        c.Hero.ImageAlt = "Efecan Sungur portresi";
        c.Hero.ScrollHint = "Keşfet";
        c.Hero.MarqueeText = V2.MarqueeText;

        // --------------------------------------------------------- hakkında

        c.About.Eyebrow = "Hakkında";
        c.About.Title = "Efecan Sungur kimdir?";
        c.About.Lead = V2.AboutLead;
        c.About.Paragraphs = V2.AboutParagraphs();
        c.About.ImagePath = SiteImages.About;
        c.About.ImageAlt = "Efecan Sungur portre";
        c.About.QuoteText = V2.AboutQuote;
        c.About.SignatureText = "Efecan Sungur";
        c.About.CtaText = "Birlikte çalışalım";
        c.About.CtaUrl = "/iletisim";
        c.About.Facts = V2.Facts();
        c.About.Skills = V2.Skills();

        // --------------------------------------------------------- iletişim

        c.Contact.Eyebrow = "İletişim";
        c.Contact.Title = "Konuşalım";
        c.Contact.Lead = V2.ContactLead;
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

        // ------------------------------------------------------------ listeler

        c.Nav = V2.Nav();
        c.Stats = V2.Stats();
        c.Expertise = V2.Expertise();
        c.Timeline = V2.Timeline();
        c.Faq = V2.Faq();
        c.Socials = V2.Socials();
        c.References = ReferenceSeed.BuildAll();

        // Yorum listesi bilerek boş — panelden gerçek içerik eklenecek.
        c.Quotes = new List<QuoteItem>();
        c.Gallery = SiteImages.Gallery();

        // Yeni kurulum zaten v2 içerikle doğar: migrasyonların hiçbiri koşmamalı.
        c.AppliedMigrations = ContentMigrations.AllIds.ToList();
        c.UpdatedAt = DateTime.UtcNow;

        return c;
    }

    internal static List<T> Ordered<T>(List<T> list) where T : ContentItem
    {
        for (var i = 0; i < list.Count; i++) list[i].Order = i;
        return list;
    }

    /// <summary>
    /// v2 metinleri tek yerde: SeedData (yeni kurulum) ve ContentMigrations
    /// (canlı kurulum) aynı değerleri kullanır.
    /// </summary>
    public static class V2
    {
        public const string MetaDescription =
            "Efecan Sungur, Antalya'da web, mobil ve masaüstü yazılım geliştiren backend developer ve Salurian Company kurucu ortağı. Projelerini ve referanslarını inceleyin.";

        public const string MetaKeywords =
            "Efecan Sungur, Efecan Sungur kimdir, Efecan Sungur yazılım, Efecan Sungur Salurian, yazılım geliştirici, " +
            "backend developer, Antalya yazılım geliştirici, ASP.NET Core, C#, Salurian Company, Mahir Topuz, " +
            "Huawei Hedef Takip, web sitesi, mobil uygulama, masaüstü uygulama";

        public const string ShortBio =
            "Efecan Sungur, Antalya'da çalışan bir yazılım geliştirici (backend developer) ve 2026'da Mahir Topuz ile " +
            "kurduğu Salurian Company'nin kurucu ortağıdır. 2022'den bu yana mobil, masaüstü ve web projeleri " +
            "geliştirip pazarlıyor; Huawei Türkiye için geliştirdiği Huawei Hedef Takip uygulamasıyla Antalya " +
            "çevresinde tanındı. " +
            "Piksel Akademi'de Yazılım Uzmanlığı Eğitimi aldı.";

        public const string HeroEyebrow = "Yazılım Geliştirici · Antalya";
        public const string HeroTitleTop = "Sağlam altyapı,";
        public const string HeroTitleAccent = "sade";
        public const string HeroTitleBottom = "yazılım.";
        public const string HeroSubtitle =
            "Backend developer ve Salurian Company kurucu ortağıyım. 2022'den bu yana web, mobil ve masaüstü " +
            "projeler geliştiriyorum; ortağım Mahir Topuz ile markalara fikirden yayına kadar çalışan ürünler " +
            "teslim ediyoruz.";

        public const string MarqueeText =
            "Vianoria • Vetmassan • Atiye Tezcan • Ayşe Güler Kuaför • Vetopya Veteriner Kliniği • " +
            "Acay Kuyumculuk • Hegza Tasarım • Beyond Gayrimenkul • CheckList • Huawei Hedef Takip • Tecno Türkiye";

        public const string FooterTagline = "Fikirleri çalışan ürünlere dönüştürüyorum.";

        public const string AboutLead =
            "Efecan Sungur, Antalya'da çalışan bir backend developer ve 2026'da Mahir Topuz ile kurduğu Salurian " +
            "Company'nin kurucu ortağıdır. 2022'den bu yana web, mobil ve masaüstü projeleri geliştirip pazarlıyor.";

        public const string AboutQuote =
            "Önce veri, sonra ekran: neyin nerede durduğu netleşmeden tek bir ekran çizilmez.";

        public const string ContactLead =
            "Yeni bir marka mı kuruyorsunuz, yoksa mevcut işinizin dijital tarafını mı büyütmek istiyorsunuz? " +
            "Web, mobil ya da masaüstü — projenizi anlatın, kapsamı birlikte netleştirelim.";

        /// <summary>Hakkında 3. paragraf — Huawei Türkiye uygulaması = Huawei Hedef Takip (Efecan'ın teyidi, 24 Eki 2026).</summary>
        public const string HuaweiParagraph =
            "Huawei Türkiye için geliştirdiğim mobil uygulama, Huawei Hedef Takip, benim için bir dönüm noktası oldu. " +
            "Saha promotörlerinin hedef ve prim takibini yapan uygulama 8 Haziran 2026'da PWA ve Android APK olarak " +
            "yayına girdi; Antalya ve çevresinde adım bu uygulamayla duyulmaya başladı.";

        public const string HuaweiTimeline =
            "Huawei Türkiye için geliştirdiğim mobil uygulama, Huawei Hedef Takip, 8 Haziran 2026'da yayına girdi; " +
            "Antalya ve çevresinde adımın duyulmasını sağladı.";

        public static List<string> AboutParagraphs() => new()
        {
            "Yazılıma 2022'de başladım. O günden bu yana mobil uygulamalar, masaüstü programlar ve web projeleri geliştirdim; bunları yalnızca yazmakla kalmayıp pazarladım da. Bir ürünün fikirden kullanıcıya uzanan yolunu iki taraftan da tanımak, bugün bir işe baktığımda nerede tıkanacağını önceden görmemi sağlıyor.",
            "Şubat 2024'ten bu yana serbest olarak web projeleri ve içerik yönetim sistemleri geliştiriyorum. Ocak–Ekim 2025 arasında Piksel Akademi'de Yazılım Uzmanlığı Eğitimi aldım; C#, ASP.NET Core, HTML & CSS, SQL ve MSSQL üzerine çalıştım.",
            HuaweiParagraph,
            "2026'da ortağım Mahir Topuz ile Salurian Company'yi (salurian.com) kurduk. Ekipte altyapı ve mimari tarafındayım: ASP.NET Core ve C# ile veri modelini, yönetim paneli altyapısını, sunucu kurulumunu, yayını ve bakımı üstleniyorum. Mahir ise ürün, tasarım ve geliştirme tarafını yürütüyor.",
            "Markalar bana çoğunlukla yeni açıldıklarında ve müşteriye ihtiyaç duyduklarında ulaşıyor. Hizmetimi Salurian Company üzerinden veriyorum ve yaklaşımım her seferinde aynı: önce işin gerçekte ne olduğunu anlamak, sonra ona en sade ve sağlam çözümü kurmak."
        };

        public static List<AboutFact> Facts() => Ordered(new List<AboutFact>
        {
            new() { Label = "Uzmanlık",  Value = "Backend · web, mobil ve masaüstü uygulama geliştirme" },
            new() { Label = "Unvan",     Value = "Backend Developer" },
            new() { Label = "Şirket",    Value = "Salurian Company — kurucu ortak" },
            new() { Label = "Ortak",     Value = "Mahir Topuz" },
            new() { Label = "Eğitim",    Value = "Piksel Akademi — Yazılım Uzmanlığı (2025)" },
            new() { Label = "Öne çıkan", Value = "Huawei Hedef Takip — Huawei Türkiye mobil uygulaması" },
            new() { Label = "Konum",     Value = "Antalya / Türkiye" },
            new() { Label = "Diller",    Value = "Türkçe · İngilizce" },
            new() { Label = "Çalışma",   Value = "Uzaktan & yerinde" },
        });

        /// <summary>
        /// Hakkında sayfasındaki teknoloji listesi — elle seçilmiş, tekrarsız. Her madde referanslardan
        /// (salurian.com) veya özgeçmişten (Piksel Akademi: C#, ASP.NET Core, HTML & CSS, SQL & MSSQL) gelir.
        /// </summary>
        public static List<string> Skills() => new()
        {
            "C#", "ASP.NET Core MVC", "Razor", "JSON veri katmanı", "HTML", "CSS", "JavaScript",
            "PWA", "Android APK", "Electron", "SQL / MSSQL", "IIS / Plesk yayını"
        };

        public static List<NavItem> Nav() => Ordered(new List<NavItem>
        {
            new() { Label = "Ana Sayfa",   Url = "/" },
            new() { Label = "Hakkında",    Url = "/hakkinda" },
            new() { Label = "Referanslar", Url = "/referanslar" },
            new() { Label = "İletişim",    Url = "/iletisim" },
        });

        /// <summary>Ana sayfadaki referans sayısı istatistiğinin etiketi (migrasyon "2026-10-28-tum-projeler" bununla bulur).</summary>
        public const string ReferenceStatLabel = "Salurian referansı";

        public static List<StatItem> Stats() => Ordered(new List<StatItem>
        {
            new() { Value = "4",    Label = "Yıllık deneyim" },
            new() { Value = ReferenceSeed.BuildAll().Count(r => r.Visible).ToString(), Label = ReferenceStatLabel },
            new() { Value = "3",    Label = "Platform: web · mobil · masaüstü" },
            new() { Value = "2026", Label = "Salurian kuruluşu" },
        });

        public static List<ExpertiseItem> Expertise() => Ordered(new List<ExpertiseItem>
        {
            new() { Icon = "globe", Title = "Web Geliştirme",
                    Description = "ASP.NET Core ve C# ile kurumsal siteler, e-ticaret ve yönetim panelli platformlar. Vianoria, Vetmassan ve Acay Kuyumculuk bu işlerden birkaçı." },
            new() { Icon = "layers", Title = "Backend ve Mimari",
                    Description = "Veri modeli, API ve iş kuralları. Bir projeye önce veriyi netleştirerek başlıyorum; ekranlar sağlam bir iskeletin üzerine kurulur." },
            new() { Icon = "message", Title = "Mobil Uygulama",
                    Description = "Sahada çalışan PWA ve APK uygulamaları. Huawei Hedef Takip ve Promotör Tecno Türkiye, saha ekipleri için geliştirilen projelerden." },
            new() { Icon = "target", Title = "Masaüstü Uygulama",
                    Description = "İnternet bağlantısı gerektirmeden çalışan, işletmenin kendi süreçlerine göre kurgulanan masaüstü programlar." },
            new() { Icon = "shield", Title = "Yönetim Paneli ve Yayın",
                    Description = "Sitedeki her içeriği düzenleyen yönetim panelleri, sunucu kurulumu, yayına alma ve teslim sonrası bakım." },
            new() { Icon = "spark", Title = "Yeni Marka Kurulumu",
                    Description = "Yeni açılan markaların dijital varlığını Salurian Company üzerinden uçtan uca kurmak: site, uygulama, arama görünürlüğü ve yayın." },
        });

        public static List<TimelineItem> Timeline() => Ordered(new List<TimelineItem>
        {
            new() { Year = "2022", Title = "İlk satır",
                    Description = "Yazılıma adım attım; mobil, masaüstü ve web projeleri geliştirip pazarlamaya başladım." },
            new() { Year = "Şubat 2024", Title = "Serbest web geliştirme",
                    Description = "Serbest (freelance) olarak birden fazla web projesi ve içerik yönetim sistemi geliştirdim." },
            new() { Year = "2025", Title = "Piksel Akademi",
                    Description = "Ocak–Ekim 2025 arasında Piksel Akademi'de Yazılım Uzmanlığı Eğitimi aldım: C#, ASP.NET Core, HTML & CSS, SQL ve MSSQL." },
            // Huawei Hedef Takip 8 Haziran 2026'da yayına girdi — Salurian'ın kuruluşundan önce (sıra korunur).
            new() { Year = "Haziran 2026", Title = "Huawei Türkiye",
                    Description = HuaweiTimeline },
            new() { Year = "2026", Title = "Salurian Company",
                    Description = "Ortağım Mahir Topuz ile Salurian Company'yi kurduk; referanslarımızın çoğu bu çatı altında hayata geçti." },
            new() { Year = "Bugün", Title = "Seçili markalarla",
                    Description = "Yeni açılan ve müşteriye ihtiyaç duyan markalarla Salurian Company üzerinden uçtan uca çalışıyorum." },
        });

        public static List<FaqItem> Faq() => Ordered(new List<FaqItem>
        {
            new() { Question = "Efecan Sungur kimdir?",
                    Answer = "Efecan Sungur, Antalya'da çalışan bir yazılım geliştirici (backend developer) ve Salurian Company'nin kurucu ortağıdır. 2022'den bu yana mobil, masaüstü ve web projeleri geliştiriyor. Huawei Türkiye için geliştirdiği Huawei Hedef Takip uygulamasıyla Antalya çevresinde tanındı." },
            new() { Question = "Efecan Sungur ne iş yapar?",
                    Answer = "Web siteleri, mobil uygulamalar ve masaüstü yazılımlar geliştirir. Salurian Company'de altyapı ve mimari tarafındadır: ASP.NET Core ve C# ile veri modeli, yönetim paneli altyapısı, sunucu kurulumu, yayın ve bakım işlerini üstlenir." },
            new() { Question = "Salurian Company nedir?",
                    Answer = "Salurian Company, Efecan Sungur ve Mahir Topuz'un 2026'da kurduğu, Antalya merkezli bir yazılım şirketidir. Kurumsal web sitesi, mobil uygulama ve masaüstü yazılım geliştirir. Efecan Sungur hizmetlerini bu şirket üzerinden verir; ayrıntılar salurian.com adresinde." },
            new() { Question = "Efecan Sungur hangi projeleri geliştirdi?",
                    Answer = "Huawei Türkiye için geliştirdiği mobil uygulama, Huawei Hedef Takip, adını Antalya çevresinde duyuran proje oldu. Salurian Company referansları arasında ayrıca Vianoria e-ticaret sitesi; Vetmassan, Acay Kuyumculuk, Hegza Tasarım, Atiye Tezcan ve Ayşe Güler Kuaför web siteleri ile Promotör Tecno Türkiye saha uygulaması bulunuyor. Tam liste Referanslar sayfasında." },
            new() { Question = "Yeni açılan bir marka için ne yapıyorsunuz?",
                    Answer = "Markalar bize çoğunlukla yeni açıldıklarında ve müşteriye ihtiyaç duyduklarında ulaşıyor. Kısa bir tanışma görüşmesinde işi ve hedef kitleyi konuşuyor, ardından ihtiyaca göre site, uygulama ve yayın kapsamını Salurian Company üzerinden teklif olarak çıkarıyoruz." },
            new() { Question = "Efecan Sungur'a nasıl ulaşılır?",
                    Answer = "En hızlı yol WhatsApp ya da telefon: 0531 832 86 78. E-posta adresi efecansungur@icloud.com; iletişim sayfasındaki form da kullanılabilir. Genellikle aynı gün dönüş yapılır." },
            new() { Question = "Antalya dışında çalışıyor musunuz?",
                    Answer = "Evet. Merkezimiz Antalya, ancak işlerin büyük bölümü uzaktan yürüyor; Ankara'daki Beyond Gayrimenkul projesi de böyle. Gerektiğinde toplantı ve teslim aşamalarında yerinde oluyoruz." },
        });

        public static List<SocialLink> Socials() => Ordered(new List<SocialLink>
        {
            new() { Label = "Salurian Company", Icon = "link", Url = "https://salurian.com" },
            new() { Label = "Salurian — Efecan Sungur", Icon = "link", Url = "https://salurian.com/hakkimizda" },
        });
    }
}
