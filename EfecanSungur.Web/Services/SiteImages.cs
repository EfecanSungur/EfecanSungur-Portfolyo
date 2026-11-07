using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// 28 Ekim 2026 görsel seti — Efecan'ın Gemini ile ürettiği ve eski fotoğrafların yerine siteye
/// konmasını istediği görseller (kaynak: Desktop\01-Musteriler\Efecan-Sungur\Gorseller\Site-2026-10-28,
/// işleyen araç: tools/images.py). Alt metin ve altyazılarda "fotoğraf" denmez, yer/olay iddiası kurulmaz.
///
/// Tek kaynak: SeedData (yeni kurulum), ContentMigrations "2026-10-28-gorseller" (canlı kurulum)
/// ve LegacyRedirects (eski görsel adresleri → yenileri, 301) aynı değerleri kullanır.
/// </summary>
public static class SiteImages
{
    public const string Hero = "/img/efecan-imza.jpg";
    public const string About = "/img/efecan-antalya.jpg";

    /// <summary>Önceki tohumların ana görseli (Temmuz ve v2) — migrasyon yalnız bunları değiştirir.</summary>
    public static readonly string[] OldHero = { "/img/efecan-hero.jpg" };

    /// <summary>Önceki tohumların Hakkında portresi (Temmuz renkli selfie, v2 S/B sürümü).</summary>
    public static readonly string[] OldAbout = { "/img/efecan-portre.jpg", "/img/efecan-portre-bw.jpg" };

    /// <summary>Galerideki yer tutucu (stok) görseller: yalnız bunlardan oluşan bir galeri yenisiyle değişir.</summary>
    public static bool IsPlaceholder(GalleryItem g) =>
        (g.ImagePath ?? "").Trim().StartsWith("https://images.unsplash.com/", StringComparison.OrdinalIgnoreCase);

    public static List<GalleryItem> Gallery() => SeedData.Ordered(new List<GalleryItem>
    {
        new() { ImagePath = "/img/galeri/efecan-sunucu-koridoru.jpg", Caption = "Altyapı",
                Alt = "Efecan Sungur, iki sıra sunucu kabini arasındaki koridorda siyah takım elbiseyle" },
        new() { ImagePath = "/img/galeri/efecan-sistem-kabini.jpg", Caption = "Ağ kabini",
                Alt = "Efecan Sungur, açık bir ağ kabininde kablo bağlantılarıyla ilgilenirken" },
        new() { ImagePath = "/img/galeri/efecan-gece-mesaisi.jpg", Caption = "Gece mesaisi",
                Alt = "Efecan Sungur, akşam saatinde dağ manzaralı bir ofiste dizüstü bilgisayarla çalışırken" },
        new() { ImagePath = "/img/galeri/efecan-antalya-yuruyus.jpg", Caption = "Gün batımı",
                Alt = "Efecan Sungur, arkasında deniz kıyısı ve dağlar olan bir terasta yürürken" },
    });

    /// <summary>Silinen eski dosyalar → yeni karşılıkları (aynı genişlikteki webp kardeşleri dahil).</summary>
    public static IEnumerable<(string From, string To)> Redirects()
    {
        yield return ("/img/efecan-hero.jpg", Hero);
        foreach (var w in new[] { 640, 1080, 1600 })
            yield return ($"/img/efecan-hero-{w}.webp", $"/img/efecan-imza-{w}.webp");
        yield return ("/img/efecan-portre.jpg", About);
        yield return ("/img/efecan-portre-bw.jpg", About);
        foreach (var w in new[] { 640, 1080 })
            yield return ($"/img/efecan-portre-bw-{w}.webp", $"/img/efecan-antalya-{w}.webp");
    }
}
