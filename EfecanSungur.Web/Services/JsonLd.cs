using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using EfecanSungur.Web.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EfecanSungur.Web.Services;

/// <summary>
/// Sayfa başına TEK bir schema.org @graph üretir (Views/Shared/_StructuredData.cshtml basar).
///
/// Merkez varlık Person (Efecan Sungur) — Google ve yapay zekâların "Efecan Sungur kimdir?"
/// sorusunu cevaplayabilmesi için. Her sayfada Person + Organization (Salurian) + WebSite,
/// üstüne sayfaya özel düğümler (WebPage / ProfilePage / CollectionPage+ItemList /
/// proje / ContactPage+FAQPage) ve ana sayfa dışında BreadcrumbList.
/// Boş alanlar atılır; grafikteki her @id başvurusu yine grafikte tanımlıdır.
/// </summary>
public static class JsonLd
{
    public const string OrgId = "https://salurian.com/#organization";
    /// <summary>salurian.com'un kendi grafiğinde Efecan Sungur için kullandığı @id (iki düğüm aynı kişi).</summary>
    public const string SalurianPersonId = "https://salurian.com/hakkimizda#efecan-sungur";

    private static readonly JsonSerializerOptions Opts = new()
    {
        WriteIndented = false,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    /// <summary>
    /// &lt;script type="application/ld+json"&gt; etiketinin tamamı. Html.Raw ile basılmalı
    /// (Razor "ld+json" içindeki + işaretini kodlar). JSON içindeki her "&lt;" → <.
    /// </summary>
    public static string Script(SiteContent site, ViewDataDictionary vd, HttpRequest req)
    {
        var json = Json(site, vd, req);
        return "<script type=\"application/ld+json\">" + json + "</script>";
    }

    public static string Json(SiteContent site, ViewDataDictionary vd, HttpRequest req)
    {
        var graph = Build(site, vd, req);
        var payload = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@graph"] = graph.Select(n => Clean(n)).Where(n => n is not null).ToList(),
        };
        return JsonSerializer.Serialize(payload, Opts).Replace("<", "\\u003c");
    }

    private static List<Dictionary<string, object?>> Build(SiteContent site, ViewDataDictionary vd, HttpRequest req)
    {
        var s = site.Settings;
        var c = site.Contact;
        var host = (s.CanonicalHost ?? "").Trim().TrimEnd('/');
        if (string.IsNullOrWhiteSpace(host)) host = $"{req.Scheme}://{req.Host}";

        string Abs(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "";
            if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || value.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) return value;
            return host + (value.StartsWith('/') ? value : "/" + value);
        }
        string PageUrl(string path) => path is "" or "/" ? host + "/" : host + "/" + path.Trim('/').ToLowerInvariant();

        var pageType = vd["PageType"] as string ?? "home";
        var canonical = vd["Canonical"] as string ?? req.Path.Value ?? "/";
        var url = PageUrl(canonical);
        var title = vd["Title"] as string ?? $"{s.OwnerName} — {s.Role}";
        var description = vd["Description"] as string ?? s.MetaDescription;
        var updated = Iso(site.UpdatedAt);

        var personId = host + "/#efecan-sungur";
        var siteId = host + "/#website";
        var pageId = url + "#webpage";
        var crumbId = url + "#breadcrumb";
        var name = string.IsNullOrWhiteSpace(s.OwnerName) ? "Efecan Sungur" : s.OwnerName.Trim();
        var nameParts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, object?> Ref(string id) => new() { ["@id"] = id };
        Dictionary<string, object?> Antalya() => new()
        {
            ["@type"] = "PostalAddress",
            ["addressLocality"] = "Antalya",
            ["addressRegion"] = "Antalya",
            ["addressCountry"] = "TR",
        };

        var visibleRefs = SeoText.VisibleReferences(site);
        // Teknolojiler: Hakkında sayfasındaki elle seçilmiş liste (About.Skills) — sayfada görünenle aynı.
        var skills = (site.About.Skills ?? new List<string>()).Select(x => (x ?? "").Trim())
            .Where(x => x.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var knowsAbout = site.Expertise.OrderBy(x => x.Order).Select(x => (x.Title ?? "").Trim())
            .Concat(skills)
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        // Şirketin ana sayfası kişinin kendisi değildir (worksFor ile bağlı) — sameAs'e girmez.
        var sameAs = site.Socials.OrderBy(x => x.Order).Select(x => (x.Url ?? "").Trim())
                         .Where(u => u.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                         .Where(u => !string.Equals(u.TrimEnd('/'), "https://salurian.com", StringComparison.OrdinalIgnoreCase))
                         .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        // salurian.com'daki Efecan düğümüyle aynı kişi olduğunu bağla (iki grafik birleşince tek varlık).
        if (!sameAs.Contains(SalurianPersonId, StringComparer.OrdinalIgnoreCase)) sameAs.Add(SalurianPersonId);

        // ------------------------------------------------------------ Person
        var portrait = Abs(site.About.ImagePath);
        var person = new Dictionary<string, object?>
        {
            ["@type"] = "Person",
            ["@id"] = personId,
            ["name"] = name,
            ["givenName"] = nameParts.Length > 1 ? string.Join(' ', nameParts[..^1]) : name,
            ["familyName"] = nameParts.Length > 1 ? nameParts[^1] : null,
            ["jobTitle"] = s.Role,
            ["description"] = s.ShortBio,
            ["image"] = string.IsNullOrEmpty(portrait) ? null : new Dictionary<string, object?>
            {
                ["@type"] = "ImageObject",
                ["@id"] = host + "/#portre",
                ["url"] = portrait,
                ["contentUrl"] = portrait,
                ["caption"] = name,
            },
            ["url"] = host + "/",
            ["mainEntityOfPage"] = host + "/hakkinda",
            ["email"] = string.IsNullOrWhiteSpace(c.Email) ? null : c.Email.Trim(),
            ["telephone"] = string.IsNullOrWhiteSpace(c.Phone) ? null : c.Phone.Trim(),
            ["address"] = Antalya(),
            ["homeLocation"] = new Dictionary<string, object?> { ["@type"] = "City", ["name"] = "Antalya" },
            ["nationality"] = string.IsNullOrWhiteSpace(s.Nationality) ? null
                : new Dictionary<string, object?> { ["@type"] = "Country", ["name"] = s.Nationality.Trim() },
            ["knowsLanguage"] = new object[]
            {
                new Dictionary<string, object?> { ["@type"] = "Language", ["name"] = "Türkçe", ["alternateName"] = "tr" },
                new Dictionary<string, object?> { ["@type"] = "Language", ["name"] = "İngilizce", ["alternateName"] = "en" },
            },
            ["knowsAbout"] = knowsAbout,
            ["alumniOf"] = string.IsNullOrWhiteSpace(s.AlumniOf) ? null
                : new Dictionary<string, object?> { ["@type"] = "EducationalOrganization", ["name"] = s.AlumniOf.Trim() },
            ["hasOccupation"] = new Dictionary<string, object?>
            {
                ["@type"] = "Occupation",
                ["name"] = "Yazılım Geliştirici",
                ["alternateName"] = "Backend Developer",
                ["occupationLocation"] = new Dictionary<string, object?> { ["@type"] = "City", ["name"] = "Antalya" },
                ["skills"] = skills.Count > 0 ? string.Join(", ", skills) : null,
            },
            ["worksFor"] = Ref(OrgId),
            ["makesOffer"] = site.Expertise.OrderBy(x => x.Order).Where(e => !string.IsNullOrWhiteSpace(e.Title))
                .Select(e => (object)new Dictionary<string, object?>
                {
                    ["@type"] = "Offer",
                    ["itemOffered"] = new Dictionary<string, object?>
                    {
                        ["@type"] = "Service",
                        ["name"] = e.Title,
                        ["description"] = e.Description,
                        ["areaServed"] = new Dictionary<string, object?> { ["@type"] = "Country", ["name"] = "Türkiye" },
                    },
                }).ToList(),
            ["sameAs"] = sameAs,
        };

        // ------------------------------------------------------------ Organization
        // Salurian düğümü salurian.com'un kendi grafiğinde tanımlıdır (aynı @id). Burada yalnız
        // asgari kimlik basılır; ad/tür/açıklama/kurucular YENİDEN beyan edilmez ki iki grafik
        // birleşince çelişen özellikler ve aynı kişi için iki ayrı kurucu düğümü oluşmasın.
        var organization = new Dictionary<string, object?>
        {
            ["@type"] = "Organization",
            ["@id"] = OrgId,
            ["name"] = "Salurian",
            ["alternateName"] = "Salurian Company",
            ["url"] = "https://salurian.com/",
        };

        // ------------------------------------------------------------ WebSite
        var website = new Dictionary<string, object?>
        {
            ["@type"] = "WebSite",
            ["@id"] = siteId,
            ["url"] = host + "/",
            ["name"] = s.SiteName,
            ["alternateName"] = HostName(host),
            ["description"] = s.MetaDescription,
            ["inLanguage"] = "tr-TR",
            ["publisher"] = Ref(personId),
            ["about"] = Ref(personId),
        };

        var graph = new List<Dictionary<string, object?>> { person, organization, website };

        Dictionary<string, object?> Page(string type, params (string Key, object? Value)[] extra)
        {
            var p = new Dictionary<string, object?>
            {
                ["@type"] = type,
                ["@id"] = pageId,
                ["url"] = url,
                ["name"] = title,
                ["description"] = description,
                ["inLanguage"] = "tr-TR",
                ["isPartOf"] = Ref(siteId),
                ["dateModified"] = updated,
                ["breadcrumb"] = pageType == "home" ? null : Ref(crumbId),
            };
            foreach (var (k, v) in extra) p[k] = v;
            return p;
        }

        Dictionary<string, object?> Image(string? path, string caption)
        {
            var abs = Abs(path);
            return string.IsNullOrEmpty(abs) ? null! : new Dictionary<string, object?>
            {
                ["@type"] = "ImageObject",
                ["url"] = abs,
                ["contentUrl"] = abs,
                ["caption"] = caption,
            };
        }

        switch (pageType)
        {
            case "about":
                graph.Add(Page("ProfilePage",
                    ("mainEntity", Ref(personId)),
                    ("about", Ref(personId)),
                    ("dateCreated", "2026-07-30T00:00:00+03:00"),
                    ("primaryImageOfPage", Image(site.About.ImagePath, name))));
                break;

            case "references":
            {
                var listId = url + "#liste";
                graph.Add(Page("CollectionPage",
                    ("about", Ref(personId)),
                    ("mainEntity", Ref(listId))));
                graph.Add(new Dictionary<string, object?>
                {
                    ["@type"] = "ItemList",
                    ["@id"] = listId,
                    ["name"] = $"{name} referansları",
                    ["numberOfItems"] = visibleRefs.Count,
                    ["itemListElement"] = visibleRefs.Select((r, i) => (object)new Dictionary<string, object?>
                    {
                        ["@type"] = "ListItem",
                        ["position"] = i + 1,
                        ["url"] = PageUrl(SeoText.ReferencePath(r)),
                        ["name"] = r.Title,
                    }).ToList(),
                });
                break;
            }

            case "reference" when vd["Reference"] is ReferenceItem r:
            {
                var projectId = url + "#proje";
                graph.Add(Page("ItemPage",
                    ("mainEntity", Ref(projectId)),
                    ("about", Ref(projectId)),
                    ("primaryImageOfPage", Image(r.ImagePath, string.IsNullOrWhiteSpace(r.ImageAlt) ? r.Title : r.ImageAlt))));
                graph.Add(Project(r, projectId, url, personId, Abs, SeoText.PredatesSalurian(r)));
                break;
            }

            case "contact":
            {
                graph.Add(Page("ContactPage",
                    ("about", Ref(personId)),
                    ("mainEntity", Ref(personId))));
                var faq = site.Faq.OrderBy(x => x.Order)
                    .Where(f => !string.IsNullOrWhiteSpace(f.Question) && !string.IsNullOrWhiteSpace(f.Answer)).ToList();
                if (s.ShowFaq && faq.Count > 0)
                {
                    graph.Add(new Dictionary<string, object?>
                    {
                        ["@type"] = "FAQPage",
                        ["@id"] = url + "#sss",
                        ["url"] = url,
                        ["name"] = $"{name} — sık sorulan sorular",
                        ["inLanguage"] = "tr-TR",
                        ["isPartOf"] = Ref(siteId),
                        ["mainEntity"] = faq.Select(f => (object)new Dictionary<string, object?>
                        {
                            ["@type"] = "Question",
                            ["name"] = f.Question.Trim(),
                            ["acceptedAnswer"] = new Dictionary<string, object?>
                            {
                                ["@type"] = "Answer",
                                ["text"] = f.Answer.Trim(),
                            },
                        }).ToList(),
                    });
                }
                break;
            }

            default:
                graph.Add(Page("WebPage",
                    ("about", Ref(personId)),
                    ("primaryImageOfPage", Image(site.Hero.ImagePath, name))));
                break;
        }

        // ------------------------------------------------------------ BreadcrumbList
        if (pageType != "home" && vd["Breadcrumbs"] is IEnumerable<BreadcrumbItem> crumbs)
        {
            var list = crumbs.ToList();
            if (list.Count > 0)
            {
                graph.Add(new Dictionary<string, object?>
                {
                    ["@type"] = "BreadcrumbList",
                    ["@id"] = crumbId,
                    ["itemListElement"] = list.Select((b, i) => (object)new Dictionary<string, object?>
                    {
                        ["@type"] = "ListItem",
                        ["position"] = i + 1,
                        ["name"] = b.Name,
                        ["item"] = PageUrl(b.Url),
                    }).ToList(),
                });
            }
        }

        return graph;
    }

    /// <summary>
    /// Referansın kendisi: kategoriye göre WebSite / MobileApplication / SoftwareApplication / CreativeWork.
    /// - Canlı adresi olmayan web işi WebSite DEĞİL CreativeWork olur (url = efecansungur.com'daki vaka
    ///   sayfası, sitenin kendisi değildir).
    /// - downloadUrl yalnız gerçek bir indirme sayfasına (/indir, /download, .apk, .exe) verilir;
    ///   değilse canlı adres url olur.
    /// - Salurian'dan (2026) eski işlerde Organization yaratıcı olarak yazılmaz (kuruluş tarihiyle çelişir).
    ///   Ekip işlerinde yaratıcı Salurian, Efecan katkıda bulunan (contributor) olarak geçer.
    /// - Kendi projesi (OwnProject, ör. Huawei Hedef Takip) olan işte yaratıcı Efecan'dır (Person).
    /// </summary>
    private static Dictionary<string, object?> Project(ReferenceItem r, string id, string detailUrl, string personId,
                                                       Func<string?, string> abs, bool predatesSalurian)
    {
        var category = (r.Category ?? "").Trim().ToLowerInvariant();
        var live = (r.LiveUrl ?? "").Trim();
        if (!live.StartsWith("http", StringComparison.OrdinalIgnoreCase)) live = "";
        var source = (r.SourceUrl ?? "").Trim();
        var type = category switch
        {
            "web" => live.Length > 0 ? "WebSite" : "CreativeWork",
            "mobil" => "MobileApplication",
            "masaustu" => "SoftwareApplication",
            _ => "CreativeWork",
        };
        var isDownload = live.Length > 0 && IsDownloadUrl(live);
        var url = live.Length > 0 && !isDownload ? live : null;
        var sameAs = new[] { isDownload ? "" : live, source }
            .Where(u => u.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !string.Equals(u, url, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        var client = (r.Client ?? "").Trim();
        var node = new Dictionary<string, object?>
        {
            ["@type"] = type,
            ["@id"] = id,
            ["name"] = r.Title,
            ["alternateName"] = string.IsNullOrWhiteSpace(r.Kind) ? null : $"{r.Title} — {r.Kind}",
            ["description"] = string.IsNullOrWhiteSpace(r.Summary) ? null : r.Summary.Trim(),
            ["image"] = abs(r.ImagePath),
            // url yalnız işin kendi adresi; vaka sayfası mainEntityOfPage'dir (iş "o sayfa" değildir).
            ["url"] = url,
            ["mainEntityOfPage"] = detailUrl,
            ["dateCreated"] = SeoText.IsoDate(r.Year),
            ["creativeWorkStatus"] = string.IsNullOrWhiteSpace(r.Status) ? null : r.Status.Trim(),
            ["keywords"] = r.Stack.Count > 0 ? string.Join(", ", r.Stack) : null,
            ["inLanguage"] = "tr-TR",
            // Kendi projesi (OwnProject): yaratıcı Efecan. Ekip işi: yaratıcı Salurian, Efecan katkıda bulunan.
            // Salurian'dan eski ve kendi projesi olarak işaretlenmemiş iş: yaratıcı yazılmaz.
            ["creator"] = r.OwnProject ? new Dictionary<string, object?> { ["@id"] = personId }
                        : predatesSalurian ? null : new Dictionary<string, object?> { ["@id"] = OrgId },
            ["contributor"] = r.OwnProject ? null : new Dictionary<string, object?> { ["@id"] = personId },
            ["about"] = client.Length == 0 || client.Contains("Salurian", StringComparison.OrdinalIgnoreCase)
                        || client.Contains("ekib", StringComparison.OrdinalIgnoreCase) ? null
                : new Dictionary<string, object?> { ["@type"] = "Organization", ["name"] = client },
            ["sameAs"] = sameAs,
        };

        if (type is "MobileApplication" or "SoftwareApplication")
        {
            var os = new List<string>();
            foreach (var t in r.Stack.Select(x => x.Trim().ToUpperInvariant()))
            {
                if (t == "APK" && !os.Contains("Android")) os.Add("Android");
                if (t == "PWA" && !os.Contains("Web")) os.Add("Web");
                if (t == "ELECTRON" && !os.Contains("Windows")) os.Add("Windows");
            }
            node["applicationCategory"] = "BusinessApplication";
            // İşletim sistemi çıkarılamıyorsa (ör. tarayıcıdan kullanılan saha sistemi) "Web".
            node["operatingSystem"] = os.Count > 0 ? string.Join(", ", os) : "Web";
            node["downloadUrl"] = isDownload ? live : null;
        }
        return node;
    }

    private static bool IsDownloadUrl(string u)
    {
        var path = u.Split('?', '#')[0].TrimEnd('/').ToLowerInvariant();
        return path.EndsWith("/indir") || path.EndsWith("/download") || path.EndsWith(".apk") || path.EndsWith(".exe")
               || path.EndsWith(".msi") || path.EndsWith(".zip");
    }

    // ------------------------------------------------------------ yardımcılar

    private static string? Iso(DateTime value)
    {
        if (value == default) return null;
        var utc = value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : DateTime.SpecifyKind(value, DateTimeKind.Utc);
        return utc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
    }

    private static string HostName(string host)
    {
        try { return new Uri(host).Host; } catch { return host; }
    }

    /// <summary>null, boş metin ve boş listeleri özyinelemeli olarak atar.</summary>
    private static object? Clean(object? value)
    {
        switch (value)
        {
            case null: return null;
            case string str: return string.IsNullOrWhiteSpace(str) ? null : str;
            case IDictionary<string, object?> dict:
            {
                var outDict = new Dictionary<string, object?>();
                foreach (var (k, v) in dict)
                {
                    var cv = Clean(v);
                    if (cv is not null) outDict[k] = cv;
                }
                return outDict.Count == 0 ? null : outDict;
            }
            case IEnumerable list:
            {
                var items = list.Cast<object?>().Select(Clean).Where(x => x is not null).ToList();
                return items.Count == 0 ? null : items;
            }
            default: return value;
        }
    }
}
