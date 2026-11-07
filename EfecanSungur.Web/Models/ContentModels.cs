using System.Text.Json.Serialization;

namespace EfecanSungur.Web.Models;

/// <summary>
/// Sitenin tamamı tek bir JSON dosyasında tutulur. Yönetim paneli bu ağacın
/// her dalını okuyup yazabilir — SQL yok.
/// </summary>
public class SiteContent
{
    public SiteSettings Settings { get; set; } = new();
    public ThemeSettings Theme { get; set; } = new();
    public HeroSection Hero { get; set; } = new();
    public AboutSection About { get; set; } = new();
    public ContactSection Contact { get; set; } = new();

    public List<NavItem> Nav { get; set; } = new();
    public List<StatItem> Stats { get; set; } = new();
    public List<ExpertiseItem> Expertise { get; set; } = new();
    public List<TimelineItem> Timeline { get; set; } = new();
    public List<QuoteItem> Quotes { get; set; } = new();
    public List<GalleryItem> Gallery { get; set; } = new();
    public List<SocialLink> Socials { get; set; } = new();
    public List<FaqItem> Faq { get; set; } = new();
    public List<ContactMessage> Messages { get; set; } = new();

    /// <summary>Referanslar / projeler (salurian.com işleri dahil).</summary>
    public List<ReferenceItem> References { get; set; } = new();

    /// <summary>Uygulanmış idempotent içerik migrasyonlarının adları.</summary>
    public List<string> AppliedMigrations { get; set; } = new();

    public DateTime UpdatedAt { get; set; }
}

// ---------------------------------------------------------------- singletons

public class SiteSettings
{
    public string SiteName { get; set; } = "Efecan Sungur";
    public string OwnerName { get; set; } = "Efecan Sungur";
    public string Role { get; set; } = "Marka Stratejisti & Dijital Danışman";
    public string LogoLetterOne { get; set; } = "E";
    public string LogoLetterTwo { get; set; } = "S";
    public string LogoWordmark { get; set; } = "EFECAN SUNGUR";
    public string MetaDescription { get; set; } =
        "Efecan Sungur — marka stratejisi, dijital dönüşüm ve iletişim danışmanlığı. Resmî kişisel web sitesi.";
    public string MetaKeywords { get; set; } = "Efecan Sungur, marka stratejisi, dijital danışmanlık, iletişim";
    public string FooterTagline { get; set; } = "Fikirden markaya giden yolu birlikte kuruyoruz.";
    public string CopyrightText { get; set; } = "Efecan Sungur. Tüm hakları saklıdır.";

    /// <summary>Sitemap ve JSON-LD'de kullanılan kesin adres. Sonunda / OLMAMALI.</summary>
    public string CanonicalHost { get; set; } = "https://efecansungur.com";

    /// <summary>
    /// Yapay zekâların ve Google'ın "Efecan Sungur kimdir?" sorusuna
    /// verebileceği tek paragraflık cevap. JSON-LD Person.description olarak basılır.
    /// </summary>
    public string ShortBio { get; set; } =
        "Efecan Sungur, mobil uygulama, masaüstü uygulama ve web geliştirme alanlarında " +
        "çalışan bir yazılım geliştiricisidir. 2026 yılında Mahir Topuz ile birlikte kurduğu " +
        "Salurian Company'nin kurucu ortağıdır. Huawei Türkiye için geliştirdiği mobil " +
        "uygulamayla Antalya ve çevresinde tanınmıştır.";

    /// <summary>Google Search Console doğrulama kodu (yalnızca içerik kısmı).</summary>
    public string GoogleVerification { get; set; } = "jX7nH7wTmZMoS2bw7IZLrNkMEQFqNIoLcTqLNPQ_3tM";

    /// <summary>Doğum yeri, mezuniyet gibi ek bilgiler — boşsa şemaya yazılmaz.</summary>
    public string AlumniOf { get; set; } = "";
    public string Nationality { get; set; } = "Türkiye";
    public bool ShowIntro { get; set; } = true;
    public bool ShowWhatsAppFloat { get; set; } = true;
    public bool ShowStats { get; set; } = true;
    public bool ShowExpertise { get; set; } = true;
    public bool ShowTimeline { get; set; } = true;
    public bool ShowQuotes { get; set; } = true;
    public bool ShowGallery { get; set; } = true;
    public bool ShowFaq { get; set; } = true;
    public bool ShowReferences { get; set; } = true;
}

public class ThemeSettings
{
    public string Bordo { get; set; } = "#6E1423";
    public string BordoDeep { get; set; } = "#400C15";
    public string BordoSoft { get; set; } = "#8C1C2E";
    public string Cream { get; set; } = "#F6EDE1";
    public string CreamDeep { get; set; } = "#EADCC9";
    public string CreamSoft { get; set; } = "#FCF7F0";
    public string White { get; set; } = "#FFFFFF";
    public string Ink { get; set; } = "#241014";
    public string DisplayFont { get; set; } = "Inter Tight";
    public string BodyFont { get; set; } = "Inter";

    // ---- v2 tasarım belirteçleri (eski alanlar JSON uyumluluğu için duruyor)
    public string Night { get; set; } = "#0B0D11";
    public string NightSoft { get; set; } = "#151820";
    public string NightLine { get; set; } = "#262B35";
    public string Paper { get; set; } = "#F4F1EA";
    public string PaperDeep { get; set; } = "#E9E4D9";
    public string Surface { get; set; } = "#FFFFFF";
    public string TextMain { get; set; } = "#111318";
    public string TextMuted { get; set; } = "#5A606A";
    public string Signal { get; set; } = "#2F54EB";
    public string SignalSoft { get; set; } = "#8EA6FF";
    public string AccentFont { get; set; } = "Instrument Serif";
    public string MonoFont { get; set; } = "JetBrains Mono";
}

public class HeroSection
{
    public string Eyebrow { get; set; } = "Efecan Sungur";
    public string TitleTop { get; set; } = "Markaların";
    public string TitleAccent { get; set; } = "hikâyesini";
    public string TitleBottom { get; set; } = "yazıyorum";
    public string Subtitle { get; set; } =
        "Strateji, kimlik ve dijital iletişim. On yılı aşkın süredir markaların doğru sesi bulmasına eşlik ediyorum.";
    public string PrimaryCtaText { get; set; } = "Benimle Tanışın";
    public string PrimaryCtaUrl { get; set; } = "/hakkinda";
    public string SecondaryCtaText { get; set; } = "İletişime Geçin";
    public string SecondaryCtaUrl { get; set; } = "/iletisim";
    public string ImagePath { get; set; } = "/img/uploads/hero.jpg";
    public string ImageAlt { get; set; } = "Efecan Sungur portre";
    public string ScrollHint { get; set; } = "Keşfet";
    public string MarqueeText { get; set; } = "Marka Stratejisi • Dijital Dönüşüm • İletişim Danışmanlığı • İçerik Yönetimi";
}

public class AboutSection
{
    public string Eyebrow { get; set; } = "Hakkında";
    public string Title { get; set; } = "Efecan Sungur kimdir?";
    public string Lead { get; set; } =
        "Markaların yalnızca ne söylediğiyle değil, nasıl hissettirdiğiyle de ilgilenen bir stratejist.";
    public List<string> Paragraphs { get; set; } = new()
    {
        "Efecan Sungur, kariyerine dijital ajans dünyasında içerik üreticisi olarak başladı. Kısa sürede stratejinin yaratıcı işin merkezinde olduğunu fark etti ve odağını marka konumlandırma ile iletişim mimarisine kaydırdı.",
        "Bugün; yeni kurulan girişimlerden köklü kurumlara kadar geniş bir yelpazede markaların kimlik, ses tonu ve dijital varlık stratejilerini kuruyor. Çalışma biçiminin merkezinde tek bir soru var: “Bu marka kimin hayatında, tam olarak neyi değiştiriyor?”",
        "Sahada öğrenmeye, veriyle doğrulamaya ve sade anlatmaya inanır. Karmaşık olanı anlaşılır kılmayı; anlaşılır olanı da unutulmaz hâle getirmeyi hedefler."
    };
    public string ImagePath { get; set; } = "/img/uploads/about.jpg";
    public string ImageAlt { get; set; } = "Efecan Sungur çalışırken";
    public string SignatureText { get; set; } = "Efecan Sungur";
    public string QuoteText { get; set; } = "İyi marka, doğru cümleyi doğru insana söyleyen markadır.";
    public List<AboutFact> Facts { get; set; } = new();
    /// <summary>Hakkında sayfasındaki "Teknolojiler" listesi ve JSON-LD knowsAbout (elle seçilir, panelden düzenlenir).</summary>
    public List<string> Skills { get; set; } = new();
    public string CtaText { get; set; } = "Birlikte çalışalım";
    public string CtaUrl { get; set; } = "/iletisim";
}

public class ContactSection
{
    public string Eyebrow { get; set; } = "İletişim";
    public string Title { get; set; } = "Konuşalım";
    public string Lead { get; set; } =
        "Bir fikriniz, bir markanız ya da sadece bir sorunuz olabilir. Her ikisi de aynı adrese gider.";
    public string Phone { get; set; } = "+905550000000";
    public string PhoneDisplay { get; set; } = "+90 555 000 00 00";
    public string WhatsAppNumber { get; set; } = "905550000000";
    public string WhatsAppMessage { get; set; } = "Merhaba Efecan Bey, web siteniz üzerinden ulaşıyorum.";
    public string Email { get; set; } = "info@efecansungur.com";
    public string Address { get; set; } = "Antalya, Türkiye";
    public string WorkingHours { get; set; } = "Hafta içi 09:00 – 18:00";
    public string MapEmbedUrl { get; set; } =
        "https://www.openstreetmap.org/export/embed.html?bbox=30.65%2C36.85%2C30.78%2C36.92&layer=mapnik";
    public bool FormEnabled { get; set; } = true;
    public string FormTitle { get; set; } = "Mesaj bırakın";
    public string FormNote { get; set; } = "Genellikle aynı gün içinde dönüş yapıyorum.";
    public string FormSuccessMessage { get; set; } = "Mesajınız ulaştı. En kısa sürede size dönüş yapacağım.";
}

// ------------------------------------------------------------ list öğeleri

public abstract class ContentItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];
    public int Order { get; set; }
}

public class NavItem : ContentItem
{
    public string Label { get; set; } = "";
    public string Url { get; set; } = "/";
    public bool Visible { get; set; } = true;
}

public class StatItem : ContentItem
{
    public string Value { get; set; } = "";
    public string Suffix { get; set; } = "";
    public string Label { get; set; } = "";
}

public class ExpertiseItem : ContentItem
{
    public string Icon { get; set; } = "spark";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}

public class TimelineItem : ContentItem
{
    public string Year { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}

public class QuoteItem : ContentItem
{
    public string Text { get; set; } = "";
    public string Author { get; set; } = "";
    public string Role { get; set; } = "";
}

public class GalleryItem : ContentItem
{
    public string ImagePath { get; set; } = "";
    public string Caption { get; set; } = "";
    public string Alt { get; set; } = "";
}

public class SocialLink : ContentItem
{
    public string Label { get; set; } = "";
    public string Url { get; set; } = "";
    public string Icon { get; set; } = "link";
}

public class FaqItem : ContentItem
{
    public string Question { get; set; } = "";
    public string Answer { get; set; } = "";
}

public class ReferenceItem : ContentItem
{
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    /// <summary>"web", "mobil", "masaustu" veya "diger".</summary>
    public string Category { get; set; } = "web";
    /// <summary>Görünen tür etiketi (ör. "Kurumsal web sitesi").</summary>
    public string Kind { get; set; } = "";
    public string Client { get; set; } = "";
    public string Year { get; set; } = "";
    public string Status { get; set; } = "Yayında";
    public string Summary { get; set; } = "";
    public List<string> Body { get; set; } = new();
    public string Role { get; set; } = "";
    public List<string> Stack { get; set; } = new();
    public List<string> Highlights { get; set; } = new();
    public string LiveUrl { get; set; } = "";
    public string SourceUrl { get; set; } = "";
    public string ImagePath { get; set; } = "";
    public string ImageAlt { get; set; } = "";
    public bool Featured { get; set; }
    public bool Visible { get; set; } = true;
    /// <summary>
    /// Efecan'ın kendi projesi (Salurian ekibinin işi değil; ör. Salurian'dan önce yayına giren Huawei Hedef Takip).
    /// Açıksa JSON-LD'de yaratıcı (creator) Efecan olur, künyede "Rolüm" yazar.
    /// </summary>
    public bool OwnProject { get; set; }
}

public class AboutFact : ContentItem
{
    public string Label { get; set; } = "";
    public string Value { get; set; } = "";
}

public class ContactMessage : ContentItem
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsRead { get; set; }

    [JsonIgnore]
    public string CreatedAtDisplay => CreatedAt.ToString("dd.MM.yyyy HH:mm");
}
