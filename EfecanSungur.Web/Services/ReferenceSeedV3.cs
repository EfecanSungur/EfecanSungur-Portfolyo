using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

/// <summary>
/// 28 Ekim 2026: bilgisayardaki ve Claude hafızasındaki TÜM projelerden referans kartları (Efecan'ın isteği:
/// "bütün projelerin listesini al onları da referanslara ekleyeceğiz").
/// Kaynak ve denetim kaydı: tools/referanslar-2026-10-28.json (her olgunun kaynağı + hassasiyet notları);
/// metinler 5 yazar + 5 karşı-doğrulayıcı ajanla kaynak notlardan yazıldı, yeni olgu eklenmedi.
///
/// GÖRÜNÜR yalnız kamuya açık ve doğrulanabilir olanlar (4). Diğerleri GİZLİ taslak (Visible=false):
/// satılmamış/teslim edilmemiş işler, müşteri onayı ya da sahiplik teyidi bekleyenler. Panelde
/// "Sitede görünür" açılınca yayına girer — açmadan önce JSON'daki sensitivities notlarına bakın.
/// ContentMigrations "2026-10-28-tum-projeler" bunları canlı listeye (slug yoksa) ekler.
/// </summary>
public static class ReferenceSeedV3
{
    public static List<ReferenceItem> Build() => new()
    {
        new()
        {
            Slug = "veteriner-klinik-altyapisi",
            Title = "Veteriner Klinik Altyapısı",
            Category = "web", Kind = "Klinik web altyapısı",
            Client = "Salurian · Ürün altyapısı", Year = "Eylül 2026", Status = "Yayında",
            Summary = "Veteriner klinikleri için geliştirdiğimiz, online randevu, hasta takibi, sahip portalı ve rol tabanlı yönetim paneli sunan yeniden kullanılabilir web altyapısı.",
            Body = new()
            {
                "Salurian Company ekibiyle veteriner kliniklerine özel, yeniden kullanılabilir bir web altyapısı geliştirdik. Ziyaretçi tarafında üç adımlı online randevu, yatan ya da ameliyat olan hastanın gününü zaman çizelgesiyle gösteren Pati Takip, dijital aşı karnesi ve ev bakım planı sunan sahip portalı, belirti sihirbazı, aşı takvimi aracı ve acil rehberi yer alıyor.",
                "Yönetim paneli yönetici, hekim ve resepsiyon rolleriyle çalışıyor; gizli hekim notlarını yalnız hekim ve yönetici görebiliyor. Randevu takvimi, hasta ve sahip kayıtları, aşısı yaklaşanlara hazır WhatsApp mesajı hazırlayan hatırlatma merkezi, içerik, SEO ve JSON yedekleme panelden yönetiliyor. Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı.",
                "Altyapı bugüne kadar beş veteriner kliniği için, her biri kliniğin kendi logosu ve renkleriyle ayrı ayrı uyarlandı. Üçüncü uyarlaması olan Vetopya Veteriner Kliniği sitesi Eylül 2026'dan beri vetopya.com'da yayında; bu uyarlamada modüller panelden açılıp kapatılabiliyor ve içerik güncellemeleri canlı veriyi silmeden uygulanıyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS", "JavaScript" },
            Highlights = new()
            {
                "Yönetici, hekim ve resepsiyon rolleriyle yönetim paneli",
                "Online randevu, Pati Takip ve sahip portalı",
                "Belirti sihirbazı, aşı takvimi aracı ve hatırlatma merkezi",
                "Beş veteriner kliniği için ayrı kimlikle uyarlandı",
                "Vetopya uyarlaması vetopya.com'da yayında",
            },
            ImagePath = "/img/referanslar/veteriner-klinik-altyapisi.webp",
            ImageAlt = "Veteriner klinik altyapısının Vetopya uyarlamasındaki üç adımlı online randevu ekranı",
            Visible = true,
        },
        new()
        {
            Slug = "gokyuzu-sezinleri",
            Title = "Gökyüzü Sezinleri",
            Category = "web", Kind = "Astroloji içerik ve eğitim sitesi",
            Client = "Gökyüzü Sezinleri · Astrolog", Year = "Eylül 2026", Status = "Demo yayında",
            Summary = "Bir astroloğun içerik, eğitim ve danışmanlık sitesi; anlık gökyüzü şeridi, doğum haritası hesaplama ve dönen Dünya ile açılan bir kahraman alanı içeriyor.",
            Body = new()
            {
                "Gökyüzü Sezinleri markasıyla çalışan bir astrolog için içerik, eğitim ve danışmanlık sitesi. Eylül 2026'da Salurian Company ekibiyle geliştirdik; site, müşterinin incelemesi için demo adresinde yayında ve geri bildirimlerle revize süreci sürüyor.",
                "Gezegen, burç, element, ev ve açı sayfalarının yanında doğum haritası hesaplama, Ay ve retro takvimleri ile sabit yıldızlar ve asteroitler sayfası bulunuyor. Gökyüzü hesapları Astronomy Engine ile sunucuda yapılıyor; kahraman alanındaki dönen Dünya ve galaksi three.js ile WebGL'de çiziliyor. Site açık ve koyu temayla kullanılabiliyor.",
                "Her içerik şema tabanlı bir yönetim panelinden eklenip düzenlenebiliyor; panelde çöp kutusu, sürüm geçmişi ve medya kitaplığı da var. Logolar ve marka kiti Mahir Topuz tarafından hazırlandı. Proje ASP.NET Core 9, C# ve Razor ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "three.js / WebGL", "Astronomy Engine", "JSON" },
            Highlights = new()
            {
                "Anlık gökyüzü şeridi",
                "Doğum haritası hesaplama",
                "Dönen Dünya ve galaksiyle açılan kahraman alanı",
                "Şema tabanlı yönetim paneli",
                "Açık ve koyu tema",
            },
            LiveUrl = "https://gokyuzu.salurian.com/",
            ImagePath = "/img/referanslar/gokyuzu-sezinleri.webp",
            ImageAlt = "Gökyüzü Sezinleri sitesinin dönen Dünya ve galaksiyle açılan beyaz-lila ana sayfası",
            Visible = true,
        },
        new()
        {
            Slug = "salurian-kurumsal-web-sitesi",
            Title = "Salurian Kurumsal Web Sitesi",
            Category = "web", Kind = "Kurumsal web sitesi",
            Client = "Salurian Company · Kendi sitemiz", Year = "Temmuz 2026", Status = "Yayında",
            Summary = "Salurian Company'yi, referanslarını, duyurularını ve müşteri demolarını tanıtan, mobil ve masaüstü için ayrı kurgulanmış kurumsal web sitesi.",
            Body = new()
            {
                "Salurian Company'nin kendi kurumsal web sitesi. Temmuz 2026'da Salurian Company ekibiyle sıfırdan geliştirdik ve 31 Temmuz 2026'da yayına aldık. Site; ana sayfa, referanslar, hakkımızda, iletişim, duyurular, yıl arşivi ve müşteri demoları sayfalarından oluşuyor.",
                "Proje ASP.NET Core 9 MVC, C#, Razor ve elle yazılmış CSS ile geliştirildi; veriler tek bir JSON dosyasında tutuluyor. Mobil ve masaüstü sürümleri yalnızca medya sorgusuyla değil, ayrı stil dosyaları ve ayrı başlık, menü ve alt çubuk bileşenleriyle kuruldu. Referanslar, duyurular ve demolar yönetim panelinden düzenleniyor.",
                "Ana sayfadaki kahraman alanı küçük bir oyuna dönüşüyor: logodaki meşale, halkalardan oluşan hedef tahtasına dart gibi fırlatılıyor ve üç tam isabetten sonra ziyaretçi iletişim sayfasına yönlendiriliyor. Yapılandırılmış veri, dinamik site haritası ve llms.txt de sitenin parçası. Site salurian.com adresinde yayında.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "Mobil ve masaüstü için ayrı katmanlar",
                "Ana sayfada meşale dart oyunu",
                "Referans, duyuru ve demo yönetim paneli",
                "Yapılandırılmış veri, site haritası ve llms.txt",
                "salurian.com adresinde yayında",
            },
            LiveUrl = "https://salurian.com/",
            ImagePath = "/img/referanslar/salurian-kurumsal-web-sitesi.webp",
            ImageAlt = "Salurian kurumsal web sitesinin meşale logolu, koyu lacivert ana sayfası",
            Visible = true,
        },
        new()
        {
            Slug = "auvita-plaza",
            Title = "Auvita Plaza",
            Category = "web", Kind = "Kuaför ve bakım kompleksi web sitesi",
            Client = "Auvita Plaza", Year = "Kasım 2025", Status = "Şu an erişime kapalı",
            Summary = "Ankara Etimesgut Bağlıca'daki erkek kuaförü ve kadın-erkek bakım kompleksi için geliştirdiğim, yönetim panelli ve JSON tabanlı web sitesi.",
            Body = new()
            {
                "Auvita Plaza, Ankara Etimesgut Bağlıca'da hizmet veren bir erkek kuaförü ve kadın-erkek bakım kompleksi. Kasım–Aralık 2025'te işletmenin web sitesini ve içerikleri yönetmek için bir yönetim paneli geliştirdim.",
                "Sitede ana sayfa, ekip, randevu, iletişim, fiyat listesi, ürünler ve galeri sayfaları bulunuyor. Randevu butonu işletmenin online randevu sayfasına, ürün siparişleri WhatsApp'a yönlendiriliyor; iletişim sayfasında çalışma saatleri ve harita yer alıyor.",
                "Yönetim panelinden galeri, ekip, ürünler, fiyat listesi, hakkımızda metni, çalışma saatleri ve telefon güncellenebiliyor; görseller doğrudan panelden yükleniyor. Proje ASP.NET Core 9, C# ve Razor ile geliştirildi, veriler JSON dosyalarında tutuluyor. Site yayına alınmıştı; alan adı kaydı düştüğü için şu an erişime kapalı.",
            },
            Role = "Geliştirme, yönetim paneli ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "Bootstrap", "jQuery" },
            Highlights = new()
            {
                "Yedi sayfalık işletme sitesi",
                "Altı bölümlü yönetim paneli",
                "Panelden görsel yükleme",
                "Online randevu ve WhatsApp sipariş yönlendirmesi",
                "Veritabanısız, JSON tabanlı yapı",
            },
            ImagePath = "/img/referanslar/auvita-plaza.webp",
            ImageAlt = "Auvita Plaza web sitesinin ana sayfası",
            OwnProject = true,
            Visible = true,
        },
        new()
        {
            Slug = "terzi-bala",
            Title = "Terzi Bala",
            Category = "masaustu", Kind = "Masaüstü terzihane programı",
            Client = "Terzi Bala · Terzihane", Year = "Haziran 2026", Status = "Yerelde tamamlandı",
            Summary = "Bir terzihane için muhasebe, cari takibi, PDF teklif ve ölçü defterini tek programda toplayan, internetsiz çalışan Windows masaüstü uygulaması.",
            Body = new()
            {
                "Terzi Bala, bir terzihanenin günlük işlerini tek yerde toplamak için geliştirdiğim Windows masaüstü uygulaması. İlk sürümü Haziran 2026'da tamamladım. Program tamamen çevrimdışı çalışıyor; veriler bilgisayardaki tek bir JSON dosyasında, atomik yazım ve yedek dosyasıyla tutuluyor.",
                "Muhasebe bölümünde gelir, gider, veresiye ve stok kayıtları dönem filtreli özet kartlarıyla izleniyor; müşteri bakiyeleri bu kayıtlardan hesaplanıyor. Teklif ekranında kalemler düzenlenip isteğe bağlı KDV ile otomatik numaralı PDF teklif üretiliyor. Kalıplar bölümünde kadın ve erkek ölçü setleriyle ölçü defteri hazırlanıp A4 PDF alınabiliyor.",
                "Ölçü defterleri şablon olarak saklanıp yeniden açılabiliyor. Uygulama WPF ve .NET 9 ile geliştirildi; PDF çıktıları QuestPDF ile üretiliyor. .NET kurulu olmayan bilgisayarlarda da çalışan tek dosyalık bir sürüm ve yönetici izni gerektirmeyen Türkçe bir kurulum sihirbazı hazırladım.",
            },
            Role = "Altyapı ve mimari, geliştirme ve kurulum paketi",
            Stack = new() { "WPF", ".NET 9", "C#", "QuestPDF", "JSON", "Inno Setup" },
            Highlights = new()
            {
                "Gelir, gider, veresiye ve stok takibi",
                "Otomatik numaralı PDF teklif",
                "Kadın ve erkek ölçü defteri, A4 PDF",
                "Tamamen çevrimdışı çalışma",
                "Türkçe kurulum sihirbazı",
            },
            ImagePath = "/img/referanslar/terzi-bala.webp",
            ImageAlt = "Terzi Bala masaüstü programının ana ekranı: gelir, gider, alacak ve müşteri özet kartları",
            OwnProject = true,
            Visible = false,
        },
        new()
        {
            Slug = "ilerioglu-hukuk",
            Title = "İlerioğlu Hukuk Bürosu",
            Category = "web", Kind = "Hukuk bürosu web sitesi",
            Client = "İlerioğlu Hukuk Bürosu", Year = "Ekim 2026", Status = "Yerelde tamamlandı",
            Summary = "Antalya Kepez'deki İlerioğlu Hukuk Bürosu için avukatlık reklam yasağına uygun, yalnız bilgilendirme amaçlı kurumsal web sitesi ve yönetim paneli.",
            Body = new()
            {
                "Antalya Kepez'deki İlerioğlu Hukuk Bürosu için kurumsal web sitesini Salurian Company ekibiyle geliştirdik. Site yalnız bilgilendirme amaçlı kurgulandı; Avukatlık Kanunu ve Türkiye Barolar Birliği Reklam Yasağı Yönetmeliği'ne uyum, tasarımda ve metinlerde esas alındı.",
                "Logo mühür biçimi yerine tipografik olarak hazırlandı. Sitede başarı ya da üstünlük iddiası, müvekkil yorumu, ücret rakamı ve pop-up gibi ısrarcı çağrılar yer almıyor; her sayfanın altında panelden düzenlenen yasal uyarı satırı bulunuyor. Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor.",
                "Sitede 12 çalışma alanı sayfası, hukuki bilgi yazıları ve sıkça sorulan sorular yer alıyor; hukuki bilgiler bölümü gerektiğinde panelden tek ayarla kapatılabiliyor. Avukat profilleri, çalışma alanları, sayfalar, iletişim mesajları ve büro bilgileri yönetim panelinden düzenleniyor. İletişim formu hız sınırı ve bot korumasıyla çalışıyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "Avukatlık reklam yasağına uygun içerik ve tasarım",
                "Mühür yerine tipografik logo",
                "Her sayfada yasal uyarı satırı",
                "12 çalışma alanı sayfası ve hukuki bilgiler bölümü",
                "İçeriklerin yönetildiği yönetim paneli",
            },
            ImageAlt = "İlerioğlu Hukuk Bürosu web sitesinin tipografik logolu, sade kurumsal ana sayfası",
            Visible = false,
        },
        new()
        {
            Slug = "este-vip-beauty",
            Title = "Este VIP Beauty Güzellik Salonu",
            Category = "web", Kind = "Güzellik salonu web sitesi ve online randevu",
            Client = "Este VIP Beauty Özdilek Şubesi", Year = "Ekim 2026", Status = "Yerelde tamamlandı",
            Summary = "Este VIP Beauty Güzellik Salonu'nun Antalya Kepez'deki Özdilek şubesi için online randevu sistemli, yönetim panelinden düzenlenen tanıtım sitesi.",
            Body = new()
            {
                "Este VIP Beauty Güzellik Salonu'nun Antalya Kepez'deki Özdilek şubesi için tanıtım sitesini ve online randevu sistemini Salurian Company ekibiyle geliştirdik. Cilt bakımı, lazer epilasyon, kalıcı makyaj, kirpik ve kaş, bölgesel incelme olmak üzere beş hizmet kategorisi ayrı sayfalarda anlatılıyor.",
                "Randevu sistemi seçilen hizmet ve güne göre uygun saatleri gösteriyor; çift kaydı, süre çakışmasını ve kapalı günlere gelen talepleri reddediyor. Yönetim panelinde randevular gün, hafta ve liste görünümüyle izleniyor, durum ve not eklenebiliyor, kayıtlar CSV olarak dışa aktarılabiliyor.",
                "Hizmetler, kampanyalar, galeri, eğitimler, sayfa metinleri, çalışma saatleri ve iletişim bilgileri panelden yönetiliyor. İçerikler reklam mevzuatı gözetilerek hazırlandı; sağlık iddiası ya da garanti vaadi yer almıyor. Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS", "JavaScript" },
            Highlights = new()
            {
                "Uygun saat gösteren, çakışma denetimli online randevu",
                "Gün, hafta ve liste görünümlü randevu yönetimi",
                "Beş hizmet kategorisi için ayrı sayfalar",
                "Reklam mevzuatı gözetilerek hazırlanan içerik",
                "Hizmet, kampanya ve galeri panelden yönetiliyor",
            },
            ImageAlt = "Este VIP Beauty Güzellik Salonu web sitesinin açık renkli ana sayfası ve online randevu bölümü",
            Visible = false,
        },
        new()
        {
            Slug = "q-plus-sigorta",
            Title = "Q Plus Sigorta",
            Category = "web", Kind = "Sigorta acentesi web sitesi",
            Client = "Q Plus Sigorta", Year = "Ağustos 2026", Status = "Yerelde tamamlandı",
            Summary = "Türkiye geneli hizmet veren sigorta acentesi Q Plus Sigorta için 24 ürünlü, 81 il sayfalı ve tam yetkili yönetim panelli kurumsal web sitesi.",
            Body = new()
            {
                "Türkiye geneli hizmet veren Q Plus Sigorta için yeni kurumsal web sitesini Salurian Company ekibiyle geliştirdik. Acentenin mevcut sitesindeki metinler, 24 sigorta ürünü, 33 anlaşmalı şirket ve sıkça sorulan sorular temel alınarak yeniden yazıldı ve genişletildi.",
                "Sitede her il için ayrı sayfa, il ve ürün bazlı sayfalar ve 26 ilde ilçe bazlı 436 sayfa bulunuyor. Metinler ilin bölgesi, deprem riski ve ekonomik yapısına göre üretiliyor; talep görmeyen il ve ürün eşleşmeleri eleniyor. Türkçe ek uyumu için yazılan bir motor, il adlarının cümle içinde doğru çekimlenmesini sağlıyor.",
                "Mobil ve masaüstü için ayrı görünümler hazırlandı. Ürünler, menü, SSS, rehber yazıları, sigorta sözlüğü ve adres yönlendirmeleri yönetim panelinden düzenleniyor; teklif talepleri, iletişim mesajları ve şube başvuruları da panelden izleniyor. Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "24 sigorta ürünü ve 33 anlaşmalı şirket",
                "81 il sayfası ve 26 ilde ilçe bazlı 436 sayfa",
                "İlin niteliklerine göre üretilen il ve ürün metinleri",
                "Mobil ve masaüstü için ayrı görünümler",
                "Sitenin bölümlerini düzenleyen yönetim paneli",
            },
            ImageAlt = "Q Plus Sigorta web sitesinin beyaz ağırlıklı, lacivert vurgulu ana sayfası ve teklif formu",
            Visible = false,
        },
        new()
        {
            Slug = "marasli-sutcu-imam",
            Title = "Maraşlı Sütçü İmam",
            Category = "web", Kind = "Kurumsal web sitesi yenileme demosu",
            Client = "Maraşlı Sütçü İmam", Year = "Eylül 2026", Status = "Yerelde tamamlandı",
            Summary = "Alanya'da geleneksel dondurma ve baklava üreten Maraşlı Sütçü İmam için hazırlanan, dört dilli ve üç ana sayfa tasarımlı yeni kurumsal site demosu.",
            Body = new()
            {
                "Alanya'da geleneksel dövme ve kesme dondurma ile baklava üreten, kafeleri de bulunan Maraşlı Sütçü İmam için Salurian Company ekibiyle yeni bir kurumsal site hazırladık. 2020 tarihli eski sitedeki 38 ürün, 8 yazı ve şube bilgileri yeni yapıya taşındı; eski sitenin adresleri yeni sitede de çalışıyor.",
                "Site Türkçe, İngilizce, Almanca ve Rusça olarak kuruldu. Sitedeki 1·2·3 düğmeleriyle üç ana sayfa tasarımı (Mühür, Tente, Külah) arasında geçiş yapılabiliyor; yalnız üst bilgi ve giriş bölümü değişiyor. Kafe kartlarında yol tarifi ve saati bilinen kafeler için açık/kapalı durumu, ayrıca toptan satış, franchise ve memnuniyet anketi formları bulunuyor.",
                "Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor. İçerik yönetim panelinden düzenlenebiliyor; arama motorları için site haritaları, yapılandırılmış veri ve llms.txt hazırlandı. Site, firmaya sunulan bir yenileme demosu ve henüz yayında değil.",
            },
            Role = "Web kiti (renk, yazı tipi ve logo dosyaları), altyapı ve mimari, yönetim paneli altyapısı",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "Türkçe, İngilizce, Almanca ve Rusça",
                "Üç ana sayfa tasarımı: Mühür, Tente, Külah",
                "Kafe kartlarında yol tarifi ve açık/kapalı durumu",
                "Eski site adresleri korunuyor",
                "Yönetim paneli",
            },
            ImageAlt = "Maraşlı Sütçü İmam yeni sitesinin mühür logolu mavi-beyaz ana sayfası",
            Visible = false,
        },
        new()
        {
            Slug = "barim-doganay-gallery",
            Title = "BARIM Doğanay Gallery",
            Category = "web", Kind = "Oto galeri web sitesi ve yönetim paneli",
            Client = "BARIM Doğanay Gallery", Year = "Eylül 2026", Status = "Yerelde tamamlandı",
            Summary = "Isparta'daki ikinci el oto galerisi BARIM Doğanay Gallery için siyah ve altın tonlarında kurumsal web sitesi ve ilan odaklı yönetim paneli.",
            Body = new()
            {
                "Isparta'daki ikinci el oto galerisi BARIM Doğanay Gallery için kurumsal web sitesini ve yönetim panelini Salurian Company ekibiyle geliştirdik. Tasarım, logodaki siyah ve altın tonlarından ve showroom'un görünümünden yola çıkıyor; açılıp kapanan vitrin kepengi motifi ve araçların boyalı ya da değişen parçalarını gösteren etkileşimli harita siteye özgü öğeler.",
                "İlanlar, Motorlu Kara Taşıtlarının Ticareti Hakkında Yönetmelik'in istediği bilgiler gözetilerek hazırlanıyor. Araç sayfalarındaki taşıt kredisi hesaplayıcısı aylık taksiti ve yıllık maliyet oranını gösteriyor. Ana sayfa için iki ayrı tasarım hazırlandı. Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor.",
                "Yönetim panelinde araç ve içerik yönetiminin yanında Yayın Merkezi ve Fırsat Radarı bulunuyor. Yayın Merkezi her ilan kanalı için hazır ilan metni ve fotoğraf paketi üretip fiyat ve durum farklarını izliyor; Fırsat Radarı elle girilen ilanların tahmini piyasa değerini hesaplıyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS", "JavaScript" },
            Highlights = new()
            {
                "Açılıp kapanan vitrin kepengi motifi",
                "Etkileşimli boyalı ve değişen parça haritası",
                "Taşıt kredisi hesaplayıcısı",
                "Panelde Yayın Merkezi ve Fırsat Radarı",
                "Ana sayfa için iki farklı tasarım",
            },
            ImageAlt = "BARIM Doğanay Gallery web sitesinin siyah ve altın tonlu, vitrin kepengi motifli ana sayfası",
            Visible = false,
        },
        new()
        {
            Slug = "mybrand-tente",
            Title = "MyBrand Tente",
            Category = "web", Kind = "Katalog web sitesi yenileme demosu",
            Client = "MyBrand", Year = "Eylül 2026", Status = "Şu an erişime kapalı",
            Summary = "Döşemealtı'ndaki tente kumaşı firması MyBrand için OpenCart tabanlı eski sitenin yerine hazırlanan, sepetsiz ve WhatsApp odaklı yeni katalog sitesi demosu.",
            Body = new()
            {
                "Antalya Döşemealtı'nda tente kumaşı, screen kumaş, dış mekân döşemelik kumaş ve gölgelendirme sistemleri satan MyBrand, mevcut sitesi için 9 maddelik bir düzenleme listesi gönderdi. Salurian Company ekibiyle, OpenCart üzerinde çalışan eski sitenin yerine bu istekleri karşılayan yeni bir site önerdik ve satış öncesi demosunu hazırladık.",
                "Eski sitedeki 306 ürün, 15 kategori ve 9 marka yeni yapıya taşındı; eski adresler yeni sayfalara yönlendiriliyor. Ürün fiyatları görünüyor ama sepet yok; ürün sayfalarında WhatsApp düğmesi ve teklif formu var. Kumaşlar seri ve renk kartelasıyla gezilebiliyor; Yelken Gölge Sistemi ve Hareketli Katlanır Tente için ayrı sayfalar hazırlandı.",
                "Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor. Ürün, kategori, marka, sayfa, sistem sayfası ve slider gibi içeriklerin tamamı yönetim panelinden eklenip düzenlenebiliyor. Demo yayını şu an kapalı.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "OpenCart'tan yeni altyapıya geçiş önerisi",
                "306 ürün, 15 kategori ve 9 marka taşındı",
                "Seri ve renk kartelası",
                "Sepetsiz, WhatsApp ve teklif formu odaklı",
                "Her içeriği düzenleyen yönetim paneli",
            },
            ImageAlt = "MyBrand yeni sitesinde tente kumaşlarının renk kartelası sayfası",
            Visible = false,
        },
        new()
        {
            Slug = "antalya-guncel",
            Title = "Antalya Güncel Haber Portalı",
            Category = "web", Kind = "Haber portalı (demo)",
            Client = "Antalya Güncel", Year = "Ekim 2026", Status = "Yerelde tamamlandı",
            Summary = "Antalya Güncel için eski haber adreslerini koruyan, sosyal medya paylaşımı ve anlık bildirim altyapısı bulunan yeni haber portalının yerel demosu.",
            Body = new()
            {
                "Antalya merkezli haber sitesi Antalya Güncel için yeni bir haber portalının demosunu Salurian Company ekibiyle geliştirdik. Mevcut sitedeki haber adresleri aynen korunuyor; eski kategori ve AMP adresleri tek adımda yeni adreslere yönlendiriliyor. Demo, sitenin kendi yayınlarından alınan 27 örnek haberle hazırlandı.",
                "Çok kullanıcılı yönetim panelinde yönetici, editör, muhabir ve yazar rolleri bulunuyor. Haberler zengin editörle yazılıyor ve sürüm geçmişi tutuluyor; manşet düzeni, galeri, video, canlı haber akışı, yorumlar, okur ihbarları ve bülten panelden yönetiliyor. Facebook ve Instagram paylaşımı ile tarayıcı bildirimi için altyapı hazır.",
                "Haber şeması, Google News site haritası, llms.txt ve IndexNow ile arama motorları ve yapay zekâ servisleri için görünürlük gözetildi. Haber sayfalarında sesli dinleme, sitenin genelinde koyu tema seçeneği var. Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı, sunucu kurulumu ve yayın",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS", "JavaScript" },
            Highlights = new()
            {
                "Eski haber adresleri korunuyor, eski kategori ve AMP adresleri yönlendiriliyor",
                "Rol tabanlı, çok kullanıcılı haber yönetim paneli",
                "Facebook ve Instagram paylaşım altyapısı",
                "Tarayıcı bildirimi, bülten ve haberi sesli dinleme",
                "Haber şeması, Google News site haritası ve llms.txt",
            },
            ImageAlt = "Antalya Güncel haber portalı demosunun kırmızı vurgulu ana sayfası ve manşet alanı",
            Visible = false,
        },
        new()
        {
            Slug = "motorumdanevar",
            Title = "motorumdanevar.com",
            Category = "web", Kind = "Arıza ansiklopedisi ve forum sitesi",
            Client = "Salurian · Kendi girişimimiz", Year = "Ekim 2026", Status = "Yapım aşamasında",
            Summary = "Türkiye'de 1990–2026 arasında satılan araçların motorlarını, kronik arızalarını ve OBD kodlarını anlatan, her motor için ayrı forumu olan arıza ansiklopedisi.",
            Body = new()
            {
                "motorumdanevar.com, Salurian Company ekibiyle geliştirdiğimiz kendi girişimimiz. Türkiye'de 1990–2026 arasında satılmış araçları marka, model, kasa ve motor seçeneğine kadar listeliyor; her motor için açıklama, kronik arızalar ve çözümler ile o motora özel bir forum sunuyor.",
                "Katalogda şu an 68 marka, 801 model ve 315 motor rehberi bulunuyor. Rehberlerin hepsinde motora özel tıklanabilir şema ve kontrol listesi; çoğunda sık görülen arıza kodları ile Wikimedia Commons'tan özgür lisanslı motor bölmesi fotoğrafı yer alıyor. Ayrıca 1.920 Türkçe OBD kodu, 58 arıza lambası ve 171 parçalık bir parça ansiklopedisi var.",
                "Forumda konu açılabiliyor, cevap yazılabiliyor, \"bende de oldu\" denebiliyor ve çözüm işaretlenebiliyor. Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştiriliyor; içerik JSON tabanlı. Motor rehberleri, Türkiye'de en çok satan motorlardan başlanarak eklenmeye devam ediyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS", "Python" },
            Highlights = new()
            {
                "68 marka, 801 model, 315 motor rehberi",
                "Her motor için ayrı forum",
                "1.920 Türkçe OBD arıza kodu",
                "Motora özel tıklanabilir şemalar",
                "Özgür lisanslı görseller ve atıf",
            },
            ImageAlt = "motorumdanevar.com'da bir motor rehberi sayfası ve tıklanabilir motor şeması",
            Visible = false,
        },
        new()
        {
            Slug = "astroloji-masaustu-programi",
            Title = "Astroloji Masaüstü Programı",
            Category = "masaustu", Kind = "Masaüstü astroloji programı",
            Client = "Salurian · Kendi ürünümüz", Year = "Eylül 2026", Status = "Yapım aşamasında",
            Summary = "Astrologlar için geliştirdiğimiz, internetsiz çalışan ve hesaplarını Swiss Ephemeris motoruyla yapan Türkçe masaüstü astroloji programı.",
            Body = new()
            {
                "Salurian Company ekibiyle, başlangıçtan profesyonel düzeye her astrolog için Türkçe bir masaüstü astroloji programı geliştiriyoruz. Program internetsiz çalışıyor; arayüzünde Gökyüzü Sezinleri'nin renkleri ve yazı tipleri, açık ve koyu tema var. Program bir astrolog tarafından kullanılıyor ve geliştirme onun geri bildirimleriyle sürüyor.",
                "Hesaplar Swiss Ephemeris motoruyla yapılıyor. Doğum haritası çarkının yanında transit, ilerletim, sinastri, kompozit ve Güneş dönüşü gibi türev haritalar, astrokartografi, Vedik raporlar, danışan ve randevu kaydı, rapor ve sayfa tasarımı araçları bulunuyor. Yorum metinleri programa özgü olarak Türkçe yazıldı; her ekranda bağlamsal bir yardım rehberi var.",
                "Program C# ve WPF ile .NET 9 üzerinde geliştiriliyor; kurulum paketi ve otomatik güncelleme altyapısı hazır. Ekim 2026'da çıkan 0.5.0 sürümünde 1.135 otomatik testin tamamı geçti. Sıradaki sürümlerde İngilizce arayüz ve yeni Vedik araçlar planlanıyor.",
            },
            Role = "Altyapı ve mimari, kurulum ve güncelleme altyapısı",
            Stack = new() { "C#", ".NET 9", "WPF", "Swiss Ephemeris", "JSON" },
            Highlights = new()
            {
                "İnternetsiz çalışan masaüstü program",
                "Swiss Ephemeris hesap motoru",
                "Türev haritalar, astrokartografi ve Vedik raporlar",
                "Danışan, randevu ve yazdırma araçları",
                "Özgün Türkçe yorum metinleri",
            },
            ImageAlt = "Astroloji programında lila ve lacivert tonlarında doğum haritası çarkı",
            Visible = false,
        },
        new()
        {
            Slug = "huawei-saha-dokumantasyon-otomasyonu",
            Title = "Huawei Saha Dokümantasyon Otomasyonu",
            Category = "diger", Kind = "Belge üretim araçları",
            Client = "Huawei saha ekibi", Year = "Haziran 2026", Status = "Teslim edildi",
            Summary = "Huawei saha ekibinde çalışırken günlük eğitim raporları, operatör mağazası ürün katalogları ve yeni promotör rehberi için geliştirdiğim belge üretim araçları.",
            Body = new()
            {
                "Huawei saha ekibinde bölge eğitmeni olarak çalışırken üç belge üretim aracı geliştirdim. İlki günlük saha eğitim raporu üreticisi: günün notları tek bir JSON dosyasına giriliyor, sistem buradan üst yönetime sunulacak PDF raporu, PowerPoint sunumunu ve görselleriyle birlikte bir klasör çıkarıyor.",
                "İkincisi, Vodafone ve Turkcell mağazalarında müşterilerin okuyacağı A5 ürün kataloğu: her operatörün renklerinde, kulaklık, akıllı saat ve tablet kategorilerinde 14 ürün sayfası içeren 20 sayfalık iki ayrı PDF. Görsellerde resmî Huawei ürün görselleri, açıklamalarda özgün Türkçe metinler kullanıldı.",
                "Üçüncüsü, işe yeni başlayan promotörler için telefonda kaydırılarak okunan, 26 cihazı anlatan 59 sayfalık ürün ve satış rehberi. Üç araç da Python ile hazırlanan HTML şablonlarından Chromium üzerinden PDF üretiyor.",
            },
            Role = "Tasarım, geliştirme ve içerik hazırlığı",
            Stack = new() { "Python", "HTML & CSS", "Playwright (Chromium)", "python-pptx", "PyMuPDF" },
            Highlights = new()
            {
                "Tek JSON'dan PDF, PowerPoint ve klasör rapor",
                "Vodafone ve Turkcell için iki ayrı A5 katalog",
                "59 sayfalık, 26 cihazlı mobil satış rehberi",
                "Özgün Türkçe ürün metinleri",
                "Python ile otomatik PDF üretimi",
            },
            ImageAlt = "Huawei ürün kataloğu ve telefon rehberinden örnek sayfalar",
            OwnProject = true,
            Visible = false,
        },
        new()
        {
            Slug = "planet-veteriner",
            Title = "Planet Veteriner Kliniği",
            Category = "web", Kind = "Klinik web sitesi",
            Client = "Planet Veteriner Kliniği", Year = "Eylül 2026", Status = "Tasarım taslağı",
            Summary = "Döşemealtı'ndaki Planet Veteriner Kliniği için hazırlanan, online randevu, hasta takibi ve sahip portalı içeren, gece gökyüzü temalı web sitesi.",
            Body = new()
            {
                "Planet Veteriner Kliniği için hazırlanan web sitesini Salurian Company ekibiyle geliştirdik. Tasarım, kliniğin gezegen ve kedi-köpek silüetli logosundan ve ekibin forma renginden yola çıkıyor: lacivert gece gökyüzü, güneş sarısı vurgular ve pati motifleri. Düşük çözünürlüklü logo vektörel olarak yeniden çizildi.",
                "Sitede üç adımlı online randevu, ameliyat ya da yatış gününü zaman çizelgesiyle gösteren Pati Takip, dijital aşı karnesi ve kilo grafiği sunan Pati Portalı, belirti sihirbazı, aşı takvimi, acil rehberi, galeri, kayıp & bulundu ilanları ve 8 rehber yazısı yer alıyor. Yönetim paneli yönetici, hekim ve resepsiyon rolleriyle çalışıyor.",
                "Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor. Mobil ve masaüstü için ayrı arayüz katmanları var; klinik metinleri, fotoğrafları, saatleri ve ekibi panelden kendisi güncelleyebiliyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "Logodan ve ekip formasından türetilen gece gökyüzü teması",
                "Vektörel olarak yeniden çizilen logo",
                "Online randevu, Pati Takip ve Pati Portalı",
                "Belirti sihirbazı ve aşı takvimi aracı",
                "Yönetici, hekim ve resepsiyon rolleriyle yönetim paneli",
            },
            ImageAlt = "Planet Veteriner Kliniği sitesinin gece gökyüzü temalı ana sayfası",
            Visible = false,
        },
        new()
        {
            Slug = "badem-veteriner",
            Title = "Badem Veteriner Kliniği",
            Category = "web", Kind = "Klinik web sitesi",
            Client = "Badem Veteriner Kliniği", Year = "Eylül 2026", Status = "Tasarım taslağı",
            Summary = "Döşemealtı'ndaki Badem Veteriner Kliniği için hazırlanan, logosundaki kırmızı çizgilerden yola çıkan, açık temalı ve sade yapılı web sitesi.",
            Body = new()
            {
                "Badem Veteriner Kliniği için hazırlanan web sitesini Salurian Company ekibiyle geliştirdik. Tasarım kliniğin logosunu temel alıyor: beyaz ağırlıklı açık tema, logo kırmızısı ve antrasit, badem biçimli fotoğraf çerçeveleri ve açık zeminde koyu tonlu yıldız motifleri. İnce çizgili logo vektörel olarak yeniden çizildi.",
                "Site, iletişimin telefon ve WhatsApp üzerinden yürüdüğü sade bir yapıyla kurgulandı; online randevu ve hasta takip modülleri bu sürümde yer almıyor. Ziyaretçi tarafında hizmet sayfaları, acil rehberi, belirti sihirbazı, aşı takvimi aracı ve rehber yazıları bulunuyor; panelde hasta kayıtları, bakım planları ve hatırlatmalar tutuluyor.",
                "Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor. Mobil görünümdeki alt çubuk WhatsApp, hizmetler, telefonla arama, acil ve yol tarifi bağlantılarını bir arada sunuyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "Logodan türetilen beyaz-kırmızı açık tema",
                "Vektörel olarak yeniden çizilen logo",
                "Telefon ve WhatsApp odaklı sade yapı",
                "Belirti sihirbazı ve aşı takvimi aracı",
                "Mobilde WhatsApp, arama ve yol tarifi çubuğu",
            },
            ImageAlt = "Badem Veteriner Kliniği sitesinin beyaz-kırmızı açık temalı ana sayfası",
            Visible = false,
        },
        new()
        {
            Slug = "palmiye-veteriner",
            Title = "Palmiye Veteriner Kliniği",
            Category = "web", Kind = "Klinik web sitesi",
            Client = "Palmiye Veteriner Kliniği", Year = "Eylül 2026", Status = "Tasarım taslağı",
            Summary = "Döşemealtı Yeniköy'deki Palmiye Veteriner Kliniği için hazırlanan, online randevu, hasta takibi ve sahip portalı içeren, beyaz-yeşil temalı web sitesi.",
            Body = new()
            {
                "Palmiye Veteriner Kliniği için hazırlanan web sitesini Salurian Company ekibiyle geliştirdik. Beyaz ağırlıklı tema, kliniğin palmiye ve hayvan silüetli logosundaki açık ve koyu yeşilden türetildi; logo yeniden çizilmeden siteye yerleştirildi.",
                "Sitede kliniğin röntgen, ultrason, kan ve hormon analizleri, yoğun bakım, gaz anestezi, aşı, cerrahi ve ağız-diş sağlığı hizmetlerinin her biri için ayrı sayfa var. Online randevu, Pati Takip, Pati Portalı, belirti sihirbazı, aşı takvimi ve 11 rehber yazısı da yer alıyor.",
                "Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor. Yönetim paneli yönetici, hekim ve resepsiyon rolleriyle çalışıyor; tablolar telefonda karta dönüşüyor, çalışma saatleri, hizmetler ve içerik panelden güncelleniyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "Logodan türetilen beyaz-yeşil tema",
                "Her hizmete ayrı sayfa",
                "Online randevu, Pati Takip ve Pati Portalı",
                "11 rehber yazısı ve belirti sihirbazı",
                "Mobil için ayrıca tasarlanan yönetim paneli",
            },
            ImageAlt = "Palmiye Veteriner Kliniği sitesinin beyaz-yeşil temalı ana sayfası",
            Visible = false,
        },
        new()
        {
            Slug = "termessos-veteriner",
            Title = "Termessos Veteriner Kliniği",
            Category = "web", Kind = "Klinik web sitesi",
            Client = "Termessos Veteriner Kliniği", Year = "Eylül 2026", Status = "Tasarım taslağı",
            Summary = "Muratpaşa'daki Termessos Veteriner Kliniği için hazırlanan, lacivert-altın temalı, online randevu, hasta takibi ve sahip portalı içeren web sitesi.",
            Body = new()
            {
                "Termessos Veteriner Kliniği için hazırlanan web sitesini Salurian Company ekibiyle geliştirdik. Açık krem zeminli tema, kliniğin rozet logosundaki lacivert ve altından türetildi; logodaki menderes deseni sitede motif olarak kullanıldı. Kliniğin bu siteden önce bir web sitesi yoktu.",
                "Sitede online randevu, yatan hastanın gününü fotoğraflı adımlarla gösteren Pati Takip, dijital aşı karnesi sunan Pati Portalı, belirti sihirbazı, aşı ve parazit takvimi ile klinik imzalı 15 rehber yazısı yer alıyor. Kliniğin gerçek Google yorumlarına yer veriliyor; yorum daveti puandan bağımsız çalışıyor.",
                "Proje ASP.NET Core 9, C#, Razor, HTML ve CSS ile geliştirildi; veriler JSON tabanlı bir yapıda tutuluyor. İçerikte bilinmeyen hiçbir bilgi uydurulmadı; cihaz, deneyim yılı ya da 7/24 hizmet iddiası yer almıyor. Çalışma saatleri ve hizmetler panelden güncelleniyor.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS" },
            Highlights = new()
            {
                "Logodan türetilen lacivert-altın tema ve menderes motifi",
                "Online randevu, Pati Takip ve Pati Portalı",
                "Klinik imzalı 15 rehber yazısı",
                "Puandan bağımsız Google yorum daveti",
                "Cihaz ya da 7/24 iddiası içermeyen temkinli içerik",
            },
            ImageAlt = "Termessos Veteriner Kliniği sitesinin lacivert-altın temalı ana sayfası",
            Visible = false,
        },
        new()
        {
            Slug = "orfe-tatil-koyu",
            Title = "Orfe Tatil Köyü",
            Category = "web", Kind = "Otel ve rezervasyon sitesi",
            Client = "Orfe Tatil Köyü · Döşemealtı", Year = "Eylül 2026", Status = "Yerelde tamamlandı",
            Summary = "Döşemealtı'ndaki Orfe Tatil Köyü için Salurian Company ekibiyle hazırladığımız, online rezervasyonlu çift dilli otel sitesi ve vektör logo yenilemesi.",
            Body = new()
            {
                "Orfe Tatil Köyü, Döşemealtı Yeşilbayır'da at kulübü, bungalov ve göl evleri, havuz ve düğün alanları bulunan bir tatil köyü. Salurian Company ekibiyle işletmeye sunulmak üzere Türkçe ve İngilizce bir otel sitesi ile yönetim paneli geliştirdik.",
                "Sitede müsaitlik ve sezon fiyatlarıyla çalışan online rezervasyon, düğün ve organizasyon talep formu ile binicilik randevusu bulunuyor. Yönetim paneli üç ayrı yetki rolüyle çalışıyor. Otel, oda, etkinlik alanı ve binicilik kulübü için yapılandırılmış veri (JSON-LD), hreflang'li site haritası ve llms.txt hazırlandı.",
                "İşletmenin mevcut JPEG logosunu vektöre çevirdik; SVG, PNG, favicon ve sosyal medya paylaşım kartlarından oluşan bir logo paketi hazırladık. Proje ASP.NET Core 9, C# ve Razor ile geliştirildi; veriler JSON tabanlı tutuluyor. Site yerelde tamamlandı, yayına alınmadı.",
            },
            Role = "Altyapı ve mimari, yönetim paneli altyapısı",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "HTML & CSS", "JavaScript" },
            Highlights = new()
            {
                "Müsaitlik ve sezon fiyatlı online rezervasyon",
                "Düğün talebi ve binicilik randevusu",
                "Türkçe ve İngilizce içerik",
                "Üç yetki rollü yönetim paneli",
                "Vektör logo paketi (SVG, PNG, favicon)",
            },
            ImageAlt = "Orfe Tatil Köyü logosunun yeniden çizilmiş vektör sürümü",
            Visible = false,
        },
        new()
        {
            Slug = "avrupa-tekstil",
            Title = "Avrupa Tekstil",
            Category = "web", Kind = "Kurumsal web sitesi",
            Client = "Avrupa Tekstil · Antalya", Year = "Mayıs 2026", Status = "Yapım aşamasında",
            Summary = "Antalya'da kurumsal üniforma, iş kıyafeti ve nakış hizmeti veren Avrupa Tekstil için geliştirdiğim, kategorili ürün sayfaları ve yönetim paneli olan web sitesi.",
            Body = new()
            {
                "Avrupa Tekstil, Antalya Muratpaşa'da kurumsal üniforma, iş kıyafeti, nakış ve tekstil baskı alanında çalışan bir işletme. Firma için ürünleri kategorilere ayırarak sergileyen ve teklif taleplerini WhatsApp'a yönlendiren bir web sitesi geliştirdim.",
                "Ürün sayfaları yazlık ve kışlık üst ve alt giyim, üniformalar ve diğer tasarımlar olmak üzere altı kategoriye ayrılıyor. Yönetim panelinden ürün ve referans görselleri eklenip silinebiliyor, sürükle-bırak ile sıralanabiliyor; veriler JSON dosyalarında tutuluyor. İletişim formu, mesajları e-postayla gönderecek şekilde kuruldu.",
                "Site ASP.NET Core 9, C# ve Razor ile geliştirildi; yerel işletme yapılandırılmış verisi (JSON-LD), Open Graph etiketleri ve sayfa bazlı başlık ve açıklamalarla arama motorlarına hazırlandı. avrupatekstil.com adresi için hazırlanan site şu an yayında değil; otellere yönelik SEO gibi bazı işler henüz tamamlanmadı.",
            },
            Role = "Geliştirme, yönetim paneli ve arama motoru düzenlemeleri",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "JSON", "Bootstrap", "MailKit" },
            Highlights = new()
            {
                "Altı kategoriye ayrılmış ürün sayfaları",
                "Panelden ürün ve referans yönetimi, sürükle-bırak sıralama",
                "Ürün numarasıyla WhatsApp teklif bağlantısı",
                "LocalBusiness JSON-LD ve Open Graph etiketleri",
                "Yapım aşamasında; şu an yayında değil",
            },
            ImageAlt = "Avrupa Tekstil web sitesinin ana sayfası ve ürün kategorileri",
            OwnProject = true,
            Visible = false,
        },
        new()
        {
            Slug = "sirinoglu-frigo-lojistik-tasimacilik",
            Title = "Şirinoğlu Frigo Lojistik & Taşımacılık",
            Category = "web", Kind = "Kurumsal web sitesi",
            Client = "Şirinoğlu Frigo Lojistik · Konya", Year = "Kasım 2025", Status = "Yerelde tamamlandı",
            Summary = "Konya merkezli frigo ve komple taşımacılık firması Şirinoğlu Lojistik için geliştirdiğim, galeri yönetim panelli kurumsal web sitesi.",
            Body = new()
            {
                "Şirinoğlu Frigo Lojistik, Konya merkezli; şehir içi ve şehirler arası taşımacılık, frigo (soğuk zincir) sevkiyat ve depolama hizmeti veren bir firma. Firma için hizmetlerini, çalışma sürecini ve iletişim bilgilerini anlatan kurumsal bir web sitesi geliştirdim.",
                "Sitede hizmetler, üç adımlı çalışma süreci, harita destekli iletişim sayfası ve WhatsApp bağlantısı bulunuyor. Yetkili girişiyle açılan yönetim panelinden galeriye JPG ve PNG görsel yüklenip silinebiliyor; iletişim formu alanları sunucu tarafında doğrulanıyor.",
                "Hazır bir lojistik site şablonunu ASP.NET Core 9 MVC yapısına uyarladım; proje C#, Razor ve Bootstrap ile geliştirildi. Bu sürüm yerelde tamamlandı ve yayına alınmadı; firmanın bugünkü sitesi farklı bir altyapı kullanıyor.",
            },
            Role = "Şablon uyarlaması, geliştirme ve galeri yönetim paneli",
            Stack = new() { "ASP.NET Core 9", "C#", "Razor", "Bootstrap", "HTML & CSS" },
            Highlights = new()
            {
                "Hizmetler, çalışma süreci ve iletişim bölümleri",
                "Oturum tabanlı yönetim girişi",
                "Panelden galeri görseli yükleme ve silme",
                "Doğrulamalı iletişim formu ve harita",
                "Yerelde tamamlandı; yayına alınmadı",
            },
            ImageAlt = "Şirinoğlu Lojistik web sitesinin hizmetler bölümü",
            OwnProject = true,
            Visible = false,
        },
        new()
        {
            Slug = "oto-8-antalya",
            Title = "Oto 8 Antalya",
            Category = "web", Kind = "Tanıtım sitesi (WordPress)",
            Client = "Oto 8 Antalya · Kepez", Year = "Mart 2025", Status = "Tasarım taslağı",
            Summary = "Kepez'deki Yeşil Antalya Sanayi Sitesi'nde hizmet veren oto tamir servisi Oto 8 Antalya için WordPress üzerinde hazırladığım tanıtım sitesi taslağı.",
            Body = new()
            {
                "Oto 8 Antalya, Kepez'deki Yeşil Antalya Sanayi Sitesi'nde otomobil bakım ve onarımı yapan bir servis. İşletmenin mail order ile ödeme alabilmesi için bir web sitesine ihtiyacı vardı; bu talep üzerine tanıtım sitesini hazırladım.",
                "Site, WordPress'in blok tabanlı site düzenleyicisiyle kuruldu. Ana sayfada işletmenin fotoğrafları, hizmet verilen otomobil markaları, deneyim, işçilik ve hızlı servis vurgulu hizmet maddeleri, hakkında bölümü, adres ve Instagram bağlantısı yer alıyor.",
                "Site oto8antalya.com adresi için planlandı. Buradaki görüntüler, sitenin Mart 2025'teki şablon düzenleme aşamasına ait.",
            },
            Role = "WordPress şablon düzenleme ve içerik yerleşimi",
            Stack = new() { "WordPress", "WordPress blok düzenleyici", "Site düzenleyici şablonları" },
            Highlights = new()
            {
                "Yeşil Antalya Sanayi Sitesi'ndeki oto servisi için",
                "WordPress blok düzenleyiciyle ana sayfa şablonu",
                "Hizmet verilen otomobil markaları ve hizmet maddeleri",
                "Hakkında, adres ve Instagram bağlantılı yan sütun",
                "Mart 2025 tarihli tasarım taslağı",
            },
            ImageAlt = "Oto 8 Antalya sitesinin WordPress düzenleyicideki ana sayfa şablonu",
            OwnProject = true,
            Visible = false,
        },
        new()
        {
            Slug = "borsa-botu-prototipi",
            Title = "Borsa Botu Prototipi",
            Category = "diger", Kind = "Alım-satım otomasyonu prototipi",
            Client = "Kişisel proje", Year = "Kasım 2025", Status = "Prototip",
            Summary = "Hisse senetleri için alış ve satış eşiği kuralları tanımlanabilen, arka planda kural motoru çalışan ve ASP.NET Core 9 ile geliştirdiğim borsa botu prototipi.",
            Body = new()
            {
                "Borsa Botu, kullanıcının bir hisse için \"şu fiyatın altına düşerse al, şu fiyatın üstüne çıkarsa sat\" kuralları tanımlayabileceği bir kontrol paneli prototipi. Kasım 2025'te mimarisini kurup arayüzünü geliştirdim.",
                "Sunucu tarafında ASP.NET Core 9 ile hesap, kural ve emir uç noktaları, kuralları iki saniyede bir değerlendiren arka plan kural motoru ve aracı kurumlar için ortak bir istemci arayüzü bulunuyor. Veriler şimdilik bellekte tutuluyor.",
                "Fiyatlar simüle ediliyor, aracı kurum istemcileri yer tutucu ve arayüz henüz sunucuya bağlanmadı; gerçek emir gönderimi yok. Arayüz HTML, CSS ve saf JavaScript ile hazırlandı; hisse kodu arama kutusu ve işlem günlüğü içeriyor. Proje prototip aşamasında kaldı.",
            },
            Role = "Mimari tasarım ve prototip geliştirme",
            Stack = new() { "ASP.NET Core 9", "C#", "Minimal API", "BackgroundService", "HTML, CSS & JavaScript" },
            Highlights = new()
            {
                "Alış ve satış eşiğine dayalı kural tanımlama",
                "Arka planda çalışan kural motoru",
                "Aracı kurum istemcileri için ortak arayüz",
                "Hisse kodu arama ve işlem günlüğü paneli",
                "Simülasyon verisiyle çalışan prototip; gerçek emir yok",
            },
            ImageAlt = "Borsa Botu kontrol panelinde kural ekleme ve bot durumu kartları",
            OwnProject = true,
            Visible = false,
        },
    };
}
