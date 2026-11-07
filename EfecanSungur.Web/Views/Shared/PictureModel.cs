namespace EfecanSungur.Web.Views.Shared;

/// <summary>
/// _Picture.cshtml için parametreler. Width/Height her zaman verilir (yerleşim
/// kayması olmasın); gerçek görsel farklı orandaysa CSS object-fit ile kırpılır.
/// </summary>
/// <param name="Src">Görsel yolu (/img/... ya da tam adres).</param>
/// <param name="Alt">Alternatif metin — içerikten gelir.</param>
/// <param name="Width">Çizim oranı için genişlik.</param>
/// <param name="Height">Çizim oranı için yükseklik.</param>
/// <param name="Sizes">srcset için sizes değeri.</param>
/// <param name="Eager">LCP görseli: eager + fetchpriority="high".</param>
/// <param name="CssClass">img etiketine eklenecek sınıf.</param>
/// <param name="HighPriority">
/// Eager ile birlikte fetchpriority="high". false: ilk ekrandaki ama LCP olmayan görseller
/// (ör. /referanslar ilk kart satırı) eager yüklenir ama LCP ile yarışmaz.
/// </param>
public sealed record PictureModel(
    string Src,
    string Alt,
    int Width,
    int Height,
    string Sizes = "100vw",
    bool Eager = false,
    string? CssClass = null,
    bool HighPriority = true);
