using EfecanSungur.Web.Models;

namespace EfecanSungur.Web.Services;

public interface IContentService
{
    /// <summary>
    /// Güncel içerik. Dönen nesne YALNIZCA OKUNUR kabul edilir: değişiklik her zaman
    /// <see cref="Save"/> üzerinden yapılır (kopya üzerinde değiştirilip atomik olarak değiştirilir).
    /// </summary>
    SiteContent Get();
    void Save(Action<SiteContent> mutate);
    void AddMessage(ContactMessage message);

    /// <summary>Disk yazılamıyorsa veya content.json okunamadıysa dolu gelir; panel bunu uyarı olarak gösterir.</summary>
    string? StorageWarning { get; }

    /// <summary>
    /// content.json bozuk bulunduğunda true: içerik yedekten/tohumdan servis edilir ve diske
    /// HİÇBİR ŞEY yazılmaz (canlı veri tohumla ezilmesin). Yönetici yedek geri yükleyince ya da
    /// içeriği sıfırlayınca <see cref="AllowWrites"/> ile kalkar.
    /// </summary>
    bool ReadOnly { get; }

    /// <summary>Salt okunur kilidi kaldırır (yalnız bilinçli geri yükleme / sıfırlama öncesi).</summary>
    void AllowWrites();
}
