using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// Canlı content.json'u yeni sürüme taşıyan idempotent içerik migrasyonları.
///
/// CANLI VERİ KURALI: yayın content.json'un üzerine asla yazmaz. Yeni içerik canlıya
/// yalnızca buradan ulaşır ve bir alan YALNIZCA hâlâ Temmuz 2026 tohum değerine
/// birebir eşitse değişir (LegacySeed202607). Panelden düzenlenmiş bir değere dokunulmaz.
///
/// Her migrasyonun bir kimliği vardır; uygulandığında AppliedMigrations'a yazılır
/// ve bir daha koşmaz (panelden silinen bir şey geri gelmez).
/// Hiçbir migrasyon siteyi düşüremez: her biri ayrı try/catch içinde.
/// </summary>
public static class ContentMigrations
{
    public const string Referanslar = "2026-10-24-referanslar";
    public const string NavReferanslar = "2026-10-24-nav-referanslar";
    public const string ThemeV2 = "2026-10-24-theme-v2";
    public const string IntroOff = "2026-10-24-intro-off";
    public const string CopyV2 = "2026-10-24-copy-v2";
    public const string Gsc = "2026-10-24-gsc";
    public const string PortreBw = "2026-10-24-portre-bw";
    public const string V2Fix = "2026-10-24-v2-duzeltme";
    public const string Skills = "2026-10-24-skills";
    public const string HuaweiLink = "2026-10-24-huawei-bag";
    public const string Gorseller = "2026-10-28-gorseller";
    public const string TumProjeler = "2026-10-28-tum-projeler";

    /// <summary>v2 tohumundaki referans sayısı istatistiği (salurian.com'dan gelen 11 referans).</summary>
    public const string OldReferenceStat = "11";

    /// <summary>Temmuz 2026'daki Hakkında portresi ve onun S/B (srcset'li) karşılığı.</summary>
    public const string OldPortrait = "/img/efecan-portre.jpg";
    public const string NewPortrait = "/img/efecan-portre-bw.jpg";

    private static readonly (string Id, Func<SiteContent, bool> Run)[] Steps =
    {
        (Referanslar, AddReferences),
        (NavReferanslar, AddNavItem),
        (ThemeV2, UpgradeTheme),
        // intro-off, hero metninin hâlâ Temmuz değerinde olup olmadığına bakar —
        // bu yüzden copy-v2'den ÖNCE koşmalı.
        (IntroOff, TurnIntroOff),
        (CopyV2, UpgradeCopy),
        (Gsc, SetVerification),
        (PortreBw, UpgradePortrait),
        // İlk v2 tohumunun düzeltmeleri (gözden geçirme bulguları). Temmuz kurulumlarında
        // copy-v2 zaten düzeltilmiş metni yazdığı için burada hiçbir şey değişmez.
        (V2Fix, FixV2),
        (Skills, AddSkills),
        // "Huawei Türkiye uygulaması" = Huawei Hedef Takip (Efecan'ın teyidi, 24 Eki 2026): v2.1 → bağlı metinler.
        // Temmuz ve ilk v2 kurulumlarında copy-v2 / v2-duzeltme güncel metni zaten yazar; burada kalanlar düzelir.
        (HuaweiLink, LinkHuawei),
        // Efecan'ın 28 Eki 2026 görsel seti (SiteImages): eski tohum görselleri yenisine geçer,
        // boş/yer tutucu galeri dolar. Portre adımı (portre-bw) bundan önce koştuğu için sıra önemli değil.
        (Gorseller, UpgradeImages),
        // Bilgisayardaki tüm projeler (ReferenceSeedV3): slug'ı olmayanlar sona eklenir, çoğu gizli taslak.
        (TumProjeler, AddAllProjects),
    };

    public static IReadOnlyList<string> AllIds { get; } = Steps.Select(s => s.Id).ToArray();

    /// <summary>
    /// Uygulanmamış migrasyonları koşturur. İçerik değiştiyse true döner
    /// (çağıran taraf diske yazar). Asla istisna fırlatmaz.
    /// </summary>
    public static bool Run(SiteContent content, ILogger? log = null)
    {
        var changed = false;
        content.AppliedMigrations ??= new List<string>();

        foreach (var (id, run) in Steps)
        {
            if (content.AppliedMigrations.Contains(id)) continue;
            try
            {
                run(content);
                content.AppliedMigrations.Add(id);
                changed = true;
            }
            catch (Exception ex)
            {
                // Kimlik eklenmez: bir sonraki açılışta yeniden denenir.
                log?.LogError(ex, "İçerik migrasyonu başarısız: {Id}", id);
            }
        }

        if (changed) content.UpdatedAt = DateTime.UtcNow;
        return changed;
    }

    // ------------------------------------------------------------ adımlar

    private static bool AddReferences(SiteContent c)
    {
        c.References ??= new List<ReferenceItem>();
        if (c.References.Count > 0) return false;
        c.References.AddRange(ReferenceSeed.Build());
        return true;
    }

    private static bool AddNavItem(SiteContent c)
    {
        c.Nav ??= new List<NavItem>();
        if (c.Nav.Any(n => SameUrl(n.Url, "/referanslar"))) return false;

        var ordered = c.Nav.OrderBy(n => n.Order).ToList();
        var contactIndex = ordered.FindIndex(n => SameUrl(n.Url, "/iletisim"));
        var item = new NavItem { Label = "Referanslar", Url = "/referanslar", Visible = true };
        if (contactIndex >= 0) ordered.Insert(contactIndex, item); else ordered.Add(item);

        for (var i = 0; i < ordered.Count; i++) ordered[i].Order = i;
        c.Nav.Clear();
        c.Nav.AddRange(ordered);
        return true;
    }

    private static bool UpgradeTheme(SiteContent c)
    {
        if (c.Theme is null) return false;
        if (c.Theme.DisplayFont != "Playfair Display") return false;
        c.Theme.DisplayFont = "Inter Tight";
        return true;
    }

    private static bool TurnIntroOff(SiteContent c)
    {
        if (!c.Settings.ShowIntro) return false;
        var old = LegacySeed202607.Build().Hero;
        if (!HeroTitleIs(c.Hero, old) || c.Hero.Subtitle != old.Subtitle) return false;
        c.Settings.ShowIntro = false;
        return true;
    }

    /// <summary>
    /// Hakkında portresi hâlâ Temmuz dosyasıysa aynı fotoğrafın S/B, webp kardeşli
    /// sürümüne geçer. Eski dosya diskte kalır (başka bir alan onu kullanıyor olabilir).
    /// </summary>
    private static bool UpgradePortrait(SiteContent c)
    {
        if (c.About is null) return false;
        if (!string.Equals((c.About.ImagePath ?? "").Trim(), OldPortrait, StringComparison.OrdinalIgnoreCase)) return false;
        c.About.ImagePath = NewPortrait;
        return true;
    }

    private static bool SetVerification(SiteContent c)
    {
        if (!string.IsNullOrWhiteSpace(c.Settings.GoogleVerification)) return false;
        c.Settings.GoogleVerification = SeedData.GscToken;
        return true;
    }

    /// <summary>
    /// Metin yükseltmesi: her alan yalnız Temmuz 2026 tohum değerine eşitse değişir.
    /// Listeler bütün olarak karşılaştırılır — tek bir öğesi düzenlenmişse liste olduğu gibi kalır.
    /// </summary>
    private static bool UpgradeCopy(SiteContent c)
    {
        var old = LegacySeed202607.Build();
        var fresh = SeedData.Build();
        var changed = false;

        void Swap(Func<string?> get, Action<string> set, string oldValue, string newValue)
        {
            if (get() == oldValue && oldValue != newValue) { set(newValue); changed = true; }
        }

        // --- Genel ayarlar
        var s = c.Settings;
        Swap(() => s.Role, v => s.Role = v, old.Settings.Role, fresh.Settings.Role);
        Swap(() => s.MetaDescription, v => s.MetaDescription = v, old.Settings.MetaDescription, fresh.Settings.MetaDescription);
        Swap(() => s.MetaKeywords, v => s.MetaKeywords = v, old.Settings.MetaKeywords, fresh.Settings.MetaKeywords);
        Swap(() => s.FooterTagline, v => s.FooterTagline = v, old.Settings.FooterTagline, fresh.Settings.FooterTagline);
        Swap(() => s.ShortBio, v => s.ShortBio = v, old.Settings.ShortBio, fresh.Settings.ShortBio);
        Swap(() => s.AlumniOf ?? "", v => s.AlumniOf = v, "", fresh.Settings.AlumniOf);

        // --- Hero: slogan üç parça birlikte değişir (yarım kalmış karışık slogan olmasın)
        var h = c.Hero;
        if (HeroTitleIs(h, old.Hero))
        {
            h.TitleTop = fresh.Hero.TitleTop;
            h.TitleAccent = fresh.Hero.TitleAccent;
            h.TitleBottom = fresh.Hero.TitleBottom;
            changed = true;
        }
        Swap(() => h.Eyebrow, v => h.Eyebrow = v, old.Hero.Eyebrow, fresh.Hero.Eyebrow);
        Swap(() => h.Subtitle, v => h.Subtitle = v, old.Hero.Subtitle, fresh.Hero.Subtitle);
        Swap(() => h.ImageAlt, v => h.ImageAlt = v, old.Hero.ImageAlt, fresh.Hero.ImageAlt);
        Swap(() => h.MarqueeText, v => h.MarqueeText = v, old.Hero.MarqueeText, fresh.Hero.MarqueeText);
        // Birincil buton: metin ve bağlantı birlikte (Hakkında → Referanslar)
        if (h.PrimaryCtaText == old.Hero.PrimaryCtaText && h.PrimaryCtaUrl == old.Hero.PrimaryCtaUrl)
        {
            h.PrimaryCtaText = fresh.Hero.PrimaryCtaText;
            h.PrimaryCtaUrl = fresh.Hero.PrimaryCtaUrl;
            changed = true;
        }

        // --- Hakkında
        var a = c.About;
        Swap(() => a.Lead, v => a.Lead = v, old.About.Lead, fresh.About.Lead);
        Swap(() => a.QuoteText, v => a.QuoteText = v, old.About.QuoteText, fresh.About.QuoteText);
        if (a.Paragraphs is not null && a.Paragraphs.SequenceEqual(old.About.Paragraphs))
        {
            a.Paragraphs = fresh.About.Paragraphs;
            changed = true;
        }
        if (SameList(a.Facts, old.About.Facts, f => f.Label + "\u001f" + f.Value))
        {
            a.Facts = fresh.About.Facts;
            changed = true;
        }

        // --- İletişim
        Swap(() => c.Contact.Lead, v => c.Contact.Lead = v, old.Contact.Lead, fresh.Contact.Lead);

        // --- Listeler (bütün olarak)
        if (SameList(c.Stats, old.Stats, x => x.Value + "\u001f" + x.Suffix + "\u001f" + x.Label))
        { c.Stats = fresh.Stats; changed = true; }
        if (SameList(c.Expertise, old.Expertise, x => x.Title + "\u001f" + x.Description))
        { c.Expertise = fresh.Expertise; changed = true; }
        if (SameList(c.Timeline, old.Timeline, x => x.Year + "\u001f" + x.Title + "\u001f" + x.Description))
        { c.Timeline = fresh.Timeline; changed = true; }
        if (SameList(c.Faq, old.Faq, x => x.Question + "\u001f" + x.Answer))
        { c.Faq = fresh.Faq; changed = true; }
        if (SameList(c.Socials, old.Socials, x => x.Label + "\u001f" + x.Url))
        { c.Socials = fresh.Socials; changed = true; }

        return changed;
    }

    /// <summary>
    /// İlk v2 tohumundaki (LegacySeedV2a) metin, durum ve adres düzeltmeleri. Her alan YALNIZCA
    /// hâlâ ilk v2 değerine eşitse değişir; liste öğeleri tek tek (başlık/soru eşleşmesiyle) ele alınır,
    /// böylece panelde düzenlenmiş bir öğe ya da alan korunur.
    /// </summary>
    private static bool FixV2(SiteContent c)
    {
        var fresh = SeedData.Build();
        var changed = false;

        void Swap(Func<string?> get, Action<string> set, string oldValue, string newValue)
        {
            if (get() == oldValue && oldValue != newValue) { set(newValue); changed = true; }
        }

        if (c.Settings is not null)
        {
            var s = c.Settings;
            Swap(() => s.MetaDescription, v => s.MetaDescription = v, LegacySeedV2a.MetaDescription, fresh.Settings.MetaDescription);
            Swap(() => s.FooterTagline, v => s.FooterTagline = v, LegacySeedV2a.FooterTagline, fresh.Settings.FooterTagline);
        }

        if (c.About is not null)
        {
            var a = c.About;
            Swap(() => a.Lead, v => a.Lead = v, LegacySeedV2a.AboutLead, fresh.About.Lead);
            if (a.Paragraphs is not null && a.Paragraphs.SequenceEqual(LegacySeedV2a.AboutParagraphs()))
            {
                a.Paragraphs = fresh.About.Paragraphs;
                changed = true;
            }
        }

        foreach (var (title, desc) in LegacySeedV2a.Expertise())
        {
            var item = c.Expertise?.FirstOrDefault(x => x.Title == title && x.Description == desc);
            var neu = fresh.Expertise.FirstOrDefault(x => x.Title == title);
            if (item is not null && neu is not null && neu.Description != desc) { item.Description = neu.Description; changed = true; }
        }
        foreach (var (year, title, desc) in LegacySeedV2a.Timeline())
        {
            var item = c.Timeline?.FirstOrDefault(x => x.Year == year && x.Title == title && x.Description == desc);
            var neu = fresh.Timeline.FirstOrDefault(x => x.Year == year && x.Title == title);
            if (item is not null && neu is not null && neu.Description != desc) { item.Description = neu.Description; changed = true; }
        }
        foreach (var (question, answer) in LegacySeedV2a.Faq())
        {
            var item = c.Faq?.FirstOrDefault(x => x.Question == question && x.Answer == answer);
            var neu = fresh.Faq.FirstOrDefault(x => x.Question == question);
            if (item is not null && neu is not null && neu.Answer != answer) { item.Answer = neu.Answer; changed = true; }
        }

        // --- Referanslar
        var refs = c.References ?? new List<ReferenceItem>();
        var freshRefs = ReferenceSeed.Build();
        foreach (var o in LegacySeedV2a.References())
        {
            var r = refs.FirstOrDefault(x => string.Equals(x.Slug, o.Slug, StringComparison.OrdinalIgnoreCase));
            var n = freshRefs.FirstOrDefault(x => string.Equals(x.Slug, o.Slug, StringComparison.OrdinalIgnoreCase));
            if (r is null || n is null) continue;
            if (o.Summary is not null) Swap(() => r.Summary, v => r.Summary = v, o.Summary, n.Summary);
            if (o.Role is not null) Swap(() => r.Role, v => r.Role = v, o.Role, n.Role);
            if (o.Status is not null) Swap(() => r.Status, v => r.Status = v, o.Status, n.Status);
            if (o.LiveUrl is not null) Swap(() => r.LiveUrl, v => r.LiveUrl = v, o.LiveUrl, n.LiveUrl);
            if (o.Body is not null && r.Body is not null && r.Body.SequenceEqual(o.Body) && !n.Body.SequenceEqual(o.Body))
            {
                r.Body = n.Body.ToList();
                changed = true;
            }
        }

        // Kalıcı adrese gömülmüş geçici durum: eski adres LegacyRedirects ile 301 olur.
        foreach (var (oldSlug, newSlug) in LegacySeedV2a.SlugRenames)
        {
            var r = refs.FirstOrDefault(x => string.Equals(x.Slug, oldSlug, StringComparison.OrdinalIgnoreCase));
            if (r is null || refs.Any(x => string.Equals(x.Slug, newSlug, StringComparison.OrdinalIgnoreCase))) continue;
            r.Slug = newSlug;
            changed = true;
        }

        return changed;
    }

    /// <summary>Hakkında "Teknolojiler" listesi boşsa elle seçilmiş v2 listesiyle doldurulur.</summary>
    private static bool AddSkills(SiteContent c)
    {
        if (c.About is null) return false;
        c.About.Skills ??= new List<string>();
        if (c.About.Skills.Any(x => !string.IsNullOrWhiteSpace(x))) return false;
        c.About.Skills.Clear();
        c.About.Skills.AddRange(SeedData.V2.Skills());
        return true;
    }

    /// <summary>
    /// Huawei Türkiye uygulamasını Huawei Hedef Takip referansına bağlar. Her alan YALNIZCA hâlâ v2.1 tohum
    /// değerine (LegacySeedV21) eşitse değişir; liste öğeleri tek tek ele alınır (düzenlenmiş öğe korunur).
    /// </summary>
    private static bool LinkHuawei(SiteContent c)
    {
        var changed = false;

        void Swap(Func<string?> get, Action<string> set, string oldValue, string newValue)
        {
            if (get() == oldValue && oldValue != newValue) { set(newValue); changed = true; }
        }

        if (c.Settings is not null)
        {
            var s = c.Settings;
            Swap(() => s.ShortBio, v => s.ShortBio = v, LegacySeedV21.ShortBio, SeedData.V2.ShortBio);
        }

        if (c.About is not null)
        {
            var a = c.About;
            if (a.Paragraphs is not null)
            {
                for (var i = 0; i < a.Paragraphs.Count; i++)
                {
                    if (a.Paragraphs[i] != LegacySeedV21.HuaweiParagraph) continue;
                    a.Paragraphs[i] = SeedData.V2.HuaweiParagraph;
                    changed = true;
                }
            }
            var newFact = SeedData.V2.Facts().First(f => f.Label == LegacySeedV21.FactLabel).Value;
            foreach (var f in a.Facts ?? new List<AboutFact>())
            {
                if (f.Label == LegacySeedV21.FactLabel) Swap(() => f.Value, v => f.Value = v, LegacySeedV21.FactValue, newFact);
            }
        }

        var tlNew = SeedData.V2.Timeline().First(t => t.Title == LegacySeedV21.TimelineTitle);
        foreach (var t in c.Timeline ?? new List<TimelineItem>())
        {
            if (t.Title != LegacySeedV21.TimelineTitle) continue;
            Swap(() => t.Year, v => t.Year = v, LegacySeedV21.TimelineYear, tlNew.Year);
            Swap(() => t.Description, v => t.Description = v, LegacySeedV21.TimelineDescription, tlNew.Description);
        }

        var faqNew = SeedData.V2.Faq();
        foreach (var (question, answer) in LegacySeedV21.Faq())
        {
            var neu = faqNew.FirstOrDefault(x => x.Question == question);
            if (neu is null) continue;
            foreach (var q in (c.Faq ?? new List<FaqItem>()).Where(x => x.Question == question))
                Swap(() => q.Answer, v => q.Answer = v, answer, neu.Answer);
        }

        var r = c.References?.FirstOrDefault(x => string.Equals(x.Slug, LegacySeedV21.HuaweiSlug, StringComparison.OrdinalIgnoreCase));
        if (r is not null)
        {
            Swap(() => r.Client, v => r.Client = v, LegacySeedV21.HuaweiClient, ReferenceSeed.HuaweiClient);
            Swap(() => r.Year, v => r.Year = v, LegacySeedV21.HuaweiYear, ReferenceSeed.HuaweiYear);
            Swap(() => r.Summary, v => r.Summary = v, LegacySeedV21.HuaweiSummary, ReferenceSeed.HuaweiSummary);
            Swap(() => r.Role, v => r.Role = v, LegacySeedV21.HuaweiRole, ReferenceSeed.HuaweiRole);
            if (r.Body is not null && r.Body.SequenceEqual(LegacySeedV21.HuaweiBody()))
            {
                r.Body = ReferenceSeed.HuaweiBody();
                changed = true;
            }
            if (r.Highlights is not null && r.Highlights.SequenceEqual(LegacySeedV21.HuaweiHighlights()))
            {
                r.Highlights = ReferenceSeed.HuaweiHighlights();
                changed = true;
            }
            // v2.1 değeri false (alan yoktu); teyitle birlikte kendi projesi olarak işaretlenir.
            if (!r.OwnProject) { r.OwnProject = true; changed = true; }
        }

        return changed;
    }

    /// <summary>
    /// Ana görsel ve Hakkında portresi YALNIZCA hâlâ bir önceki tohum dosyasındaysa yenisine geçer
    /// (panelden yüklenmiş bir görsele dokunulmaz). Galeri boşsa ya da yalnız stok yer tutuculardan
    /// oluşuyorsa yeni setle değişir ve bölüm açılır; panelden eklenmiş gerçek görseller korunur.
    /// </summary>
    private static bool UpgradeImages(SiteContent c)
    {
        var changed = false;
        static bool IsOneOf(string? path, string[] olds) =>
            olds.Any(o => string.Equals((path ?? "").Trim(), o, StringComparison.OrdinalIgnoreCase));

        if (c.Hero is not null && IsOneOf(c.Hero.ImagePath, SiteImages.OldHero))
        {
            c.Hero.ImagePath = SiteImages.Hero;
            changed = true;
        }
        if (c.About is not null && IsOneOf(c.About.ImagePath, SiteImages.OldAbout))
        {
            c.About.ImagePath = SiteImages.About;
            changed = true;
        }

        c.Gallery ??= new List<GalleryItem>();
        if (c.Gallery.All(SiteImages.IsPlaceholder))
        {
            c.Gallery.Clear();
            c.Gallery.AddRange(SiteImages.Gallery());
            c.Settings.ShowGallery = true;
            changed = true;
        }

        return changed;
    }

    /// <summary>
    /// 28 Eki 2026 referansları listenin SONUNA eklenir (mevcut sıra ve panel düzenlemeleri korunur); aynı slug'lı
    /// bir kayıt varsa o proje atlanır. Ana sayfadaki referans sayısı YALNIZCA hâlâ v2 tohum değerindeyse ("11")
    /// görünür referans sayısına güncellenir.
    /// </summary>
    private static bool AddAllProjects(SiteContent c)
    {
        c.References ??= new List<ReferenceItem>();
        var changed = false;
        var order = c.References.Count == 0 ? -1 : c.References.Max(r => r.Order);
        foreach (var r in ReferenceSeedV3.Build())
        {
            if (c.References.Any(x => string.Equals((x.Slug ?? "").Trim(), r.Slug, StringComparison.OrdinalIgnoreCase))) continue;
            r.Order = ++order;
            c.References.Add(r);
            changed = true;
        }

        var stat = c.Stats?.FirstOrDefault(s => s.Label == SeedData.V2.ReferenceStatLabel && s.Value == OldReferenceStat);
        if (stat is not null)
        {
            var visible = c.References.Count(r => r.Visible && !string.IsNullOrWhiteSpace(r.Slug)).ToString();
            if (visible != stat.Value) { stat.Value = visible; changed = true; }
        }
        return changed;
    }

    // ------------------------------------------------------------ yardımcılar

    private static bool HeroTitleIs(HeroSection h, HeroSection old) =>
        h.TitleTop == old.TitleTop && h.TitleAccent == old.TitleAccent && h.TitleBottom == old.TitleBottom;

    private static bool SameUrl(string? a, string b) =>
        string.Equals((a ?? "").Trim().TrimEnd('/'), b.TrimEnd('/'), StringComparison.OrdinalIgnoreCase);

    private static bool SameList<T>(List<T>? current, List<T> old, Func<T, string> key) where T : ContentItem
    {
        if (current is null || current.Count != old.Count) return false;
        return current.OrderBy(x => x.Order).Select(key)
                      .SequenceEqual(old.OrderBy(x => x.Order).Select(key));
    }
}
