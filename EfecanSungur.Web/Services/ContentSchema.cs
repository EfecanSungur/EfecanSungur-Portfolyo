using System.Collections;
using System.Reflection;
using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

public enum FieldType { Text, TextArea, Number, Url, Email, Tel, Image, Color, Toggle, Lines, Select }

public class FieldDef
{
    public string Name { get; init; } = "";
    public string Label { get; init; } = "";
    public FieldType Type { get; init; } = FieldType.Text;
    public string? Help { get; init; }
    public string? Placeholder { get; init; }
    public string[]? Options { get; init; }
    /// <summary>full | half | third</summary>
    public string Width { get; init; } = "full";
}

public class SectionDef
{
    public string Key { get; init; } = "";
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string Icon { get; init; } = "settings";
    public Func<SiteContent, object> Target { get; init; } = _ => new object();
    public List<FieldDef> Fields { get; init; } = new();
}

public class CollectionDef
{
    public string Key { get; init; } = "";
    public string Title { get; init; } = "";
    public string Singular { get; init; } = "";
    public string Description { get; init; } = "";
    public string Icon { get; init; } = "list";
    public Type ItemType { get; init; } = typeof(ContentItem);
    public Func<SiteContent, IList> Target { get; init; } = _ => new List<ContentItem>();
    public List<FieldDef> Fields { get; init; } = new();
    /// <summary>Tabloda gösterilecek alanlar.</summary>
    public string[] ListColumns { get; init; } = Array.Empty<string>();
}

/// <summary>
/// Panelin tamamı bu şemadan üretilir. Yeni bir alan eklemek istersen
/// buraya bir FieldDef eklemen yeterli — form, tablo ve kayıt otomatik uyum sağlar.
/// </summary>
public static class ContentSchema
{
    public static readonly List<SectionDef> Sections = new()
    {
        new SectionDef
        {
            Key = "genel",
            Title = "Genel Ayarlar",
            Description = "Site adı, kimlik bilgileri, SEO ve bölüm görünürlükleri.",
            Icon = "settings",
            Target = c => c.Settings,
            Fields = new()
            {
                new() { Name = nameof(SiteSettings.SiteName), Label = "Site Adı", Width = "half" },
                new() { Name = nameof(SiteSettings.OwnerName), Label = "Ad Soyad", Width = "half" },
                new() { Name = nameof(SiteSettings.Role), Label = "Ünvan / Meslek", Help = "Logonun altında ve footer'da görünür." },
                new() { Name = nameof(SiteSettings.LogoLetterOne), Label = "Logo 1. Harf", Width = "third", Help = "Monogramın sol harfi." },
                new() { Name = nameof(SiteSettings.LogoLetterTwo), Label = "Logo 2. Harf", Width = "third", Help = "Monogramın sağ harfi." },
                new() { Name = nameof(SiteSettings.LogoWordmark), Label = "Logo Yazısı", Width = "third" },
                new() { Name = nameof(SiteSettings.MetaDescription), Label = "SEO Açıklaması", Type = FieldType.TextArea },
                new() { Name = nameof(SiteSettings.MetaKeywords), Label = "SEO Anahtar Kelimeleri", Help = "Virgülle ayırın." },
                new() { Name = nameof(SiteSettings.ShortBio), Label = "Yapay Zekâ / Google Özeti", Type = FieldType.TextArea,
                        Help = "\"Efecan Sungur kimdir?\" sorusunun tek paragraflık cevabı. Google ve yapay zekâlar öncelikle bunu okur — üçüncü şahıs ağzıyla, adınızla başlayarak yazın." },
                new() { Name = nameof(SiteSettings.CanonicalHost), Label = "Site Adresi", Width = "half",
                        Placeholder = "https://efecansungur.com", Help = "Sonunda / olmadan. Sitemap ve şema bunu kullanır." },
                new() { Name = nameof(SiteSettings.GoogleVerification), Label = "Google Search Console Kodu", Width = "half",
                        Help = "Search Console'un verdiği doğrulama kodunun yalnızca content kısmı." },
                new() { Name = nameof(SiteSettings.AlumniOf), Label = "Okul / Mezuniyet", Width = "half", Help = "Boş bırakılırsa şemaya yazılmaz (Person.alumniOf)." },
                new() { Name = nameof(SiteSettings.Nationality), Label = "Uyruk", Width = "half" },
                new() { Name = nameof(SiteSettings.FooterTagline), Label = "Footer Sloganı" },
                new() { Name = nameof(SiteSettings.CopyrightText), Label = "Telif Metni" },
                new() { Name = nameof(SiteSettings.ShowIntro), Label = "Açılış animasyonu", Type = FieldType.Toggle, Width = "third" },
                new() { Name = nameof(SiteSettings.ShowWhatsAppFloat), Label = "WhatsApp butonu", Type = FieldType.Toggle, Width = "third" },
                new() { Name = nameof(SiteSettings.ShowStats), Label = "İstatistik bölümü", Type = FieldType.Toggle, Width = "third" },
                new() { Name = nameof(SiteSettings.ShowExpertise), Label = "Uzmanlık bölümü", Type = FieldType.Toggle, Width = "third" },
                new() { Name = nameof(SiteSettings.ShowTimeline), Label = "Yolculuk bölümü", Type = FieldType.Toggle, Width = "third" },
                new() { Name = nameof(SiteSettings.ShowQuotes), Label = "Müşteri yorumları bölümü", Type = FieldType.Toggle, Width = "third" },
                new() { Name = nameof(SiteSettings.ShowReferences), Label = "Referanslar bölümü", Type = FieldType.Toggle, Width = "third",
                        Help = "Kapalıyken /referanslar sayfaları 404 döner; menü, site haritası, llms.txt ve yapısal veri de referansları göstermez. Hero butonu /referanslar'a gidiyorsa onu da değiştirin." },
                new() { Name = nameof(SiteSettings.ShowGallery), Label = "Galeri bölümü", Type = FieldType.Toggle, Width = "third" },
                new() { Name = nameof(SiteSettings.ShowFaq), Label = "S.S.S. bölümü", Type = FieldType.Toggle, Width = "third" },
            }
        },
        new SectionDef
        {
            Key = "tema",
            Title = "Renk & Tipografi",
            Description = "Sitenin tüm renkleri buradan yönetilir; değişiklik anında her sayfaya yansır.",
            Icon = "palette",
            Target = c => c.Theme,
            Fields = new()
            {
                // v2 belirteçleri (24 Eki 2026). Eski bordo/krem alanları modelde duruyor
                // ama tasarım artık kullanmadığı için panelde gösterilmiyor.
                new() { Name = nameof(ThemeSettings.Night), Label = "Gece (koyu zemin)", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.NightSoft), Label = "Gece — yumuşak", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.NightLine), Label = "Gece — çizgi", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.Paper), Label = "Kâğıt (açık zemin)", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.PaperDeep), Label = "Kâğıt — koyu", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.Surface), Label = "Yüzey (kart)", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.TextMain), Label = "Metin", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.TextMuted), Label = "Soluk metin", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.Signal), Label = "Sinyal (vurgu rengi)", Type = FieldType.Color, Width = "third" },
                new() { Name = nameof(ThemeSettings.SignalSoft), Label = "Sinyal — açık", Type = FieldType.Color, Width = "third",
                        Help = "Koyu zemin üzerindeki vurgu ve bağlantılar." },
                new() { Name = nameof(ThemeSettings.DisplayFont), Label = "Başlık Fontu", Type = FieldType.Select, Width = "half",
                        Options = ThemeFonts.DisplayOptions },
                new() { Name = nameof(ThemeSettings.BodyFont), Label = "Metin Fontu", Type = FieldType.Select, Width = "half",
                        Options = ThemeFonts.BodyOptions },
                new() { Name = nameof(ThemeSettings.AccentFont), Label = "Vurgu Fontu (italik)", Type = FieldType.Select, Width = "half",
                        Options = ThemeFonts.AccentOptions, Help = "Slogandaki vurgu kelimesi ve alıntılar." },
                new() { Name = nameof(ThemeSettings.MonoFont), Label = "Kod / Etiket Fontu", Type = FieldType.Select, Width = "half",
                        Options = ThemeFonts.MonoOptions },
            }
        },
        new SectionDef
        {
            Key = "hero",
            Title = "Ana Sayfa — Hero",
            Description = "Ziyaretçinin ilk gördüğü alan.",
            Icon = "star",
            Target = c => c.Hero,
            Fields = new()
            {
                new() { Name = nameof(HeroSection.Eyebrow), Label = "Üst Etiket", Width = "half" },
                new() { Name = nameof(HeroSection.ScrollHint), Label = "Kaydırma İpucu", Width = "half" },
                new() { Name = nameof(HeroSection.TitleTop), Label = "Slogan — Baş", Width = "third", Help = "Büyük başlık adınızdır; bu üç parça altındaki slogandır." },
                new() { Name = nameof(HeroSection.TitleAccent), Label = "Slogan — Vurgu", Width = "third", Help = "Tek kelime; italik vurgu fontuyla görünür." },
                new() { Name = nameof(HeroSection.TitleBottom), Label = "Slogan — Son", Width = "third" },
                new() { Name = nameof(HeroSection.Subtitle), Label = "Alt Metin", Type = FieldType.TextArea },
                new() { Name = nameof(HeroSection.PrimaryCtaText), Label = "1. Buton Yazısı", Width = "half" },
                new() { Name = nameof(HeroSection.PrimaryCtaUrl), Label = "1. Buton Linki", Type = FieldType.Url, Width = "half" },
                new() { Name = nameof(HeroSection.SecondaryCtaText), Label = "2. Buton Yazısı", Width = "half" },
                new() { Name = nameof(HeroSection.SecondaryCtaUrl), Label = "2. Buton Linki", Type = FieldType.Url, Width = "half" },
                new() { Name = nameof(HeroSection.ImagePath), Label = "Hero Görseli", Type = FieldType.Image },
                new() { Name = nameof(HeroSection.ImageAlt), Label = "Görsel Açıklaması (alt)" },
                new() { Name = nameof(HeroSection.MarqueeText), Label = "Kayan Yazı Şeridi", Help = "Öğeleri • ile ayırın." },
            }
        },
        new SectionDef
        {
            Key = "hakkinda",
            Title = "Hakkında Sayfası",
            Description = "“Efecan Sungur kimdir?” içeriği — görsel ve metin.",
            Icon = "user",
            Target = c => c.About,
            Fields = new()
            {
                new() { Name = nameof(AboutSection.Eyebrow), Label = "Üst Etiket", Width = "half" },
                new() { Name = nameof(AboutSection.Title), Label = "Başlık", Width = "half" },
                new() { Name = nameof(AboutSection.Lead), Label = "Giriş Cümlesi", Type = FieldType.TextArea },
                new() { Name = nameof(AboutSection.Paragraphs), Label = "Paragraflar", Type = FieldType.Lines,
                        Help = "Her satır ayrı bir paragraf olur. Boş satır bırakmayın." },
                new() { Name = nameof(AboutSection.Skills), Label = "Teknolojiler", Type = FieldType.Lines,
                        Help = "Her satıra bir teknoloji. Hakkında sayfasında ve yapısal veride (knowsAbout) kullanılır; tekrar eden satırlar bir kez gösterilir." },
                new() { Name = nameof(AboutSection.ImagePath), Label = "Portre Görseli", Type = FieldType.Image },
                new() { Name = nameof(AboutSection.ImageAlt), Label = "Görsel Açıklaması (alt)" },
                new() { Name = nameof(AboutSection.QuoteText), Label = "Öne Çıkan Alıntı", Type = FieldType.TextArea },
                new() { Name = nameof(AboutSection.SignatureText), Label = "İmza Metni", Width = "half" },
                new() { Name = nameof(AboutSection.CtaText), Label = "Buton Yazısı", Width = "half" },
                new() { Name = nameof(AboutSection.CtaUrl), Label = "Buton Linki", Type = FieldType.Url },
            }
        },
        new SectionDef
        {
            Key = "iletisim",
            Title = "İletişim Bilgileri",
            Description = "Telefon, WhatsApp, e-posta, adres ve form ayarları.",
            Icon = "phone",
            Target = c => c.Contact,
            Fields = new()
            {
                new() { Name = nameof(ContactSection.Eyebrow), Label = "Üst Etiket", Width = "half" },
                new() { Name = nameof(ContactSection.Title), Label = "Başlık", Width = "half" },
                new() { Name = nameof(ContactSection.Lead), Label = "Giriş Metni", Type = FieldType.TextArea },
                new() { Name = nameof(ContactSection.PhoneDisplay), Label = "Telefon (görünen)", Width = "half", Placeholder = "+90 555 000 00 00" },
                new() { Name = nameof(ContactSection.Phone), Label = "Telefon (tel: linki)", Type = FieldType.Tel, Width = "half", Placeholder = "+905550000000" },
                new() { Name = nameof(ContactSection.WhatsAppNumber), Label = "WhatsApp Numarası", Width = "half",
                        Placeholder = "905550000000", Help = "Başında + ve boşluk olmadan, ülke koduyla." },
                new() { Name = nameof(ContactSection.Email), Label = "E-posta", Type = FieldType.Email, Width = "half" },
                new() { Name = nameof(ContactSection.WhatsAppMessage), Label = "WhatsApp Hazır Mesajı", Type = FieldType.TextArea },
                new() { Name = nameof(ContactSection.Address), Label = "Adres", Width = "half" },
                new() { Name = nameof(ContactSection.WorkingHours), Label = "Çalışma Saatleri", Width = "half" },
                new() { Name = nameof(ContactSection.MapEmbedUrl), Label = "Harita Embed URL", Type = FieldType.Url },
                new() { Name = nameof(ContactSection.FormEnabled), Label = "İletişim formu açık", Type = FieldType.Toggle, Width = "half" },
                new() { Name = nameof(ContactSection.FormTitle), Label = "Form Başlığı", Width = "half" },
                new() { Name = nameof(ContactSection.FormNote), Label = "Form Notu" },
                new() { Name = nameof(ContactSection.FormSuccessMessage), Label = "Başarı Mesajı", Type = FieldType.TextArea },
            }
        },
    };

    public static readonly List<CollectionDef> Collections = new()
    {
        new CollectionDef
        {
            Key = "menu", Title = "Menü", Singular = "Menü Öğesi", Icon = "menu",
            Description = "Üst menüde ve mobil alt barda görünen bağlantılar.",
            ItemType = typeof(NavItem), Target = c => c.Nav,
            ListColumns = new[] { nameof(NavItem.Label), nameof(NavItem.Url), nameof(NavItem.Visible) },
            Fields = new()
            {
                new() { Name = nameof(NavItem.Label), Label = "Menü Adı", Width = "half" },
                new() { Name = nameof(NavItem.Url), Label = "Link", Type = FieldType.Url, Width = "half" },
                new() { Name = nameof(NavItem.Visible), Label = "Görünür", Type = FieldType.Toggle },
            }
        },
        new CollectionDef
        {
            Key = "istatistik", Title = "İstatistikler", Singular = "İstatistik", Icon = "chart",
            Description = "Ana sayfadaki sayı şeridi.",
            ItemType = typeof(StatItem), Target = c => c.Stats,
            ListColumns = new[] { nameof(StatItem.Value), nameof(StatItem.Suffix), nameof(StatItem.Label) },
            Fields = new()
            {
                new() { Name = nameof(StatItem.Value), Label = "Değer", Width = "third", Placeholder = "12" },
                new() { Name = nameof(StatItem.Suffix), Label = "Son Ek", Width = "third", Placeholder = "+" },
                new() { Name = nameof(StatItem.Label), Label = "Etiket", Width = "third", Placeholder = "Yıllık deneyim" },
            }
        },
        new CollectionDef
        {
            Key = "uzmanlik", Title = "Uzmanlık Alanları", Singular = "Uzmanlık", Icon = "grid",
            Description = "Ana sayfadaki kartlar.",
            ItemType = typeof(ExpertiseItem), Target = c => c.Expertise,
            ListColumns = new[] { nameof(ExpertiseItem.Title), nameof(ExpertiseItem.Icon) },
            Fields = new()
            {
                new() { Name = nameof(ExpertiseItem.Title), Label = "Başlık", Width = "half" },
                new() { Name = nameof(ExpertiseItem.Icon), Label = "İkon", Type = FieldType.Select, Width = "half",
                        Options = new[] { "spark", "compass", "layers", "target", "pen", "chart", "message", "shield", "globe", "users" } },
                new() { Name = nameof(ExpertiseItem.Description), Label = "Açıklama", Type = FieldType.TextArea },
            }
        },
        new CollectionDef
        {
            Key = "yolculuk", Title = "Yolculuk", Singular = "Dönem", Icon = "timeline",
            Description = "Yıl yıl kariyer zaman çizelgesi.",
            ItemType = typeof(TimelineItem), Target = c => c.Timeline,
            ListColumns = new[] { nameof(TimelineItem.Year), nameof(TimelineItem.Title) },
            Fields = new()
            {
                new() { Name = nameof(TimelineItem.Year), Label = "Yıl", Width = "third", Placeholder = "2019" },
                new() { Name = nameof(TimelineItem.Title), Label = "Başlık", Width = "third" },
                new() { Name = nameof(TimelineItem.Description), Label = "Açıklama", Type = FieldType.TextArea },
            }
        },
        new CollectionDef
        {
            Key = "referans", Title = "Müşteri Yorumları", Singular = "Yorum", Icon = "quote",
            Description = "Müşteri / iş ortağı yorumları. Yalnız gerçek yorum ekleyin.",
            ItemType = typeof(QuoteItem), Target = c => c.Quotes,
            ListColumns = new[] { nameof(QuoteItem.Author), nameof(QuoteItem.Role) },
            Fields = new()
            {
                new() { Name = nameof(QuoteItem.Text), Label = "Yorum", Type = FieldType.TextArea },
                new() { Name = nameof(QuoteItem.Author), Label = "Kişi", Width = "half" },
                new() { Name = nameof(QuoteItem.Role), Label = "Ünvan / Kurum", Width = "half" },
            }
        },
        new CollectionDef
        {
            Key = "referanslar", Title = "Referanslar", Singular = "Referans", Icon = "layers",
            Description = "Referanslar sayfası, proje detay sayfaları, site haritası ve yapısal veri bu listeden üretilir.",
            ItemType = typeof(ReferenceItem), Target = c => c.References,
            ListColumns = new[] { nameof(ReferenceItem.Title), nameof(ReferenceItem.Kind), nameof(ReferenceItem.Year), nameof(ReferenceItem.Featured), nameof(ReferenceItem.Visible) },
            Fields = new()
            {
                new() { Name = nameof(ReferenceItem.Title), Label = "Proje Adı", Width = "half" },
                new() { Name = nameof(ReferenceItem.Slug), Label = "Adres (slug)", Width = "half", Placeholder = "ornek-proje",
                        Help = "Boş bırakılırsa başlıktan üretilir: /referanslar/ornek-proje" },
                new() { Name = nameof(ReferenceItem.Category), Label = "Kategori", Type = FieldType.Select, Width = "third",
                        Options = new[] { "web", "mobil", "masaustu", "diger" } },
                new() { Name = nameof(ReferenceItem.Kind), Label = "Tür", Width = "third", Placeholder = "Kurumsal web sitesi" },
                new() { Name = nameof(ReferenceItem.Status), Label = "Durum", Width = "third", Placeholder = "Yayında" },
                new() { Name = nameof(ReferenceItem.Client), Label = "Müşteri", Width = "half" },
                new() { Name = nameof(ReferenceItem.Year), Label = "Yıl / Tarih", Width = "half", Placeholder = "Eylül 2026" },
                new() { Name = nameof(ReferenceItem.Summary), Label = "Kısa Özet", Type = FieldType.TextArea,
                        Help = "1–2 cümle. Kartlarda ve arama sonucu açıklamasında kullanılır." },
                new() { Name = nameof(ReferenceItem.Body), Label = "Proje Metni", Type = FieldType.Lines, Help = "Her satır ayrı bir paragraf." },
                new() { Name = nameof(ReferenceItem.Role), Label = "Benim Rolüm" },
                new() { Name = nameof(ReferenceItem.Stack), Label = "Teknolojiler", Type = FieldType.Lines, Width = "half", Help = "Her satıra bir teknoloji." },
                new() { Name = nameof(ReferenceItem.Highlights), Label = "Öne Çıkanlar", Type = FieldType.Lines, Width = "half", Help = "Her satıra bir madde." },
                new() { Name = nameof(ReferenceItem.LiveUrl), Label = "Canlı Adres", Type = FieldType.Url, Width = "half", Placeholder = "https://" },
                new() { Name = nameof(ReferenceItem.SourceUrl), Label = "Kaynak (salurian.com sayfası)", Type = FieldType.Url, Width = "half" },
                new() { Name = nameof(ReferenceItem.ImagePath), Label = "Kapak Görseli", Type = FieldType.Image,
                        Help = "Boşsa tasarım tipografik bir kapak gösterir." },
                new() { Name = nameof(ReferenceItem.ImageAlt), Label = "Görsel Açıklaması (alt)" },
                new() { Name = nameof(ReferenceItem.Featured), Label = "Öne çıkan (ana sayfada)", Type = FieldType.Toggle, Width = "half" },
                new() { Name = nameof(ReferenceItem.Visible), Label = "Sitede görünür", Type = FieldType.Toggle, Width = "half" },
                new() { Name = nameof(ReferenceItem.OwnProject), Label = "Kendi projem (Salurian ekibinin işi değil)", Type = FieldType.Toggle,
                        Help = "Açıksa yapı verisinde yaratıcı Efecan Sungur olur ve künye başlığı \"Rolüm\" olur." },
            }
        },
        new CollectionDef
        {
            Key = "galeri", Title = "Galeri", Singular = "Görsel", Icon = "image",
            Description = "Ana sayfadaki görsel şerit.",
            ItemType = typeof(GalleryItem), Target = c => c.Gallery,
            ListColumns = new[] { nameof(GalleryItem.ImagePath), nameof(GalleryItem.Caption) },
            Fields = new()
            {
                new() { Name = nameof(GalleryItem.ImagePath), Label = "Görsel", Type = FieldType.Image },
                new() { Name = nameof(GalleryItem.Caption), Label = "Başlık", Width = "half" },
                new() { Name = nameof(GalleryItem.Alt), Label = "Alt Metin", Width = "half" },
            }
        },
        new CollectionDef
        {
            Key = "bilgi", Title = "Künye Bilgileri", Singular = "Bilgi", Icon = "info",
            Description = "Hakkında sayfasındaki etiket–değer listesi.",
            ItemType = typeof(AboutFact), Target = c => c.About.Facts,
            ListColumns = new[] { nameof(AboutFact.Label), nameof(AboutFact.Value) },
            Fields = new()
            {
                new() { Name = nameof(AboutFact.Label), Label = "Etiket", Width = "half", Placeholder = "Uzmanlık" },
                new() { Name = nameof(AboutFact.Value), Label = "Değer", Width = "half", Placeholder = "Marka stratejisi" },
            }
        },
        new CollectionDef
        {
            Key = "sss", Title = "Sık Sorulanlar", Singular = "Soru", Icon = "help",
            Description = "İletişim sayfasındaki akordeon.",
            ItemType = typeof(FaqItem), Target = c => c.Faq,
            ListColumns = new[] { nameof(FaqItem.Question) },
            Fields = new()
            {
                new() { Name = nameof(FaqItem.Question), Label = "Soru" },
                new() { Name = nameof(FaqItem.Answer), Label = "Cevap", Type = FieldType.TextArea },
            }
        },
        new CollectionDef
        {
            Key = "sosyal", Title = "Sosyal Medya", Singular = "Hesap", Icon = "share",
            Description = "Footer ve iletişim sayfasındaki bağlantılar.",
            ItemType = typeof(SocialLink), Target = c => c.Socials,
            ListColumns = new[] { nameof(SocialLink.Label), nameof(SocialLink.Url) },
            Fields = new()
            {
                new() { Name = nameof(SocialLink.Label), Label = "Ad", Width = "half" },
                new() { Name = nameof(SocialLink.Icon), Label = "İkon", Type = FieldType.Select, Width = "half",
                        Options = new[] { "instagram", "linkedin", "x", "youtube", "facebook", "link" } },
                new() { Name = nameof(SocialLink.Url), Label = "Bağlantı", Type = FieldType.Url },
            }
        },
    };

    public static SectionDef? Section(string key) =>
        Sections.FirstOrDefault(s => s.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

    public static CollectionDef? Collection(string key) =>
        Collections.FirstOrDefault(s => s.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

    // ------------------------------------------------------------ reflection

    public static object? Read(object target, string propertyName) =>
        target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)?.GetValue(target);

    public static string ReadString(object target, string propertyName)
    {
        var value = Read(target, propertyName);
        return value switch
        {
            null => "",
            bool b => b ? "Evet" : "Hayır",
            IEnumerable<string> list => string.Join(Environment.NewLine, list),
            _ => value.ToString() ?? ""
        };
    }

    /// <summary>Form değerlerini reflection ile hedef nesneye yazar.</summary>
    public static void Apply(object target, IEnumerable<FieldDef> fields, IDictionary<string, string?> values)
    {
        var type = target.GetType();
        foreach (var field in fields)
        {
            var prop = type.GetProperty(field.Name, BindingFlags.Public | BindingFlags.Instance);
            if (prop is null || !prop.CanWrite) continue;

            values.TryGetValue(field.Name, out var raw);

            if (prop.PropertyType == typeof(bool))
            {
                prop.SetValue(target, raw is not null && (raw == "true" || raw == "on" || raw == "True"));
            }
            else if (prop.PropertyType == typeof(List<string>))
            {
                var lines = (raw ?? "")
                    .Replace("\r\n", "\n").Split('\n')
                    .Select(l => l.Trim()).Where(l => l.Length > 0).ToList();
                prop.SetValue(target, lines);
            }
            else if (prop.PropertyType == typeof(int))
            {
                prop.SetValue(target, int.TryParse(raw, out var n) ? n : 0);
            }
            else
            {
                prop.SetValue(target, (raw ?? "").Trim());
            }
        }
    }
}
