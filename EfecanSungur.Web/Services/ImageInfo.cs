using System.Collections.Concurrent;

namespace EfecanSungur.Web.Services;

/// <summary>
/// Yerel bir görselin gerçek piksel boyutunu dosya başlığından okur (PNG, JPEG,
/// WebP). <c>_Picture</c> bunu width/height ve srcset genişlik tanımı için kullanır;
/// böylece panelden yüklenen ya da farklı boyutlu dosyalar yanlış oranla basılmaz.
/// Sonuçlar yol + değişiklik zamanına göre önbelleklenir. Okunamazsa null döner —
/// çağıran taraf kendi varsayılan boyutuna düşer (asla hata fırlatmaz).
/// </summary>
public static class ImageInfo
{
    private static readonly ConcurrentDictionary<string, (DateTime Stamp, (int W, int H)? Size)> Cache = new();

    public static (int W, int H)? Read(string physicalPath)
    {
        try
        {
            var info = new FileInfo(physicalPath);
            if (!info.Exists) return null;
            var stamp = info.LastWriteTimeUtc;
            if (Cache.TryGetValue(physicalPath, out var hit) && hit.Stamp == stamp) return hit.Size;

            (int W, int H)? size;
            using (var fs = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var head = new byte[Math.Min(64 * 1024, (int)Math.Min(int.MaxValue, info.Length))];
                var n = 0;
                while (n < head.Length)
                {
                    var r = fs.Read(head, n, head.Length - n);
                    if (r <= 0) break;
                    n += r;
                }
                size = Parse(head.AsSpan(0, n));
            }
            Cache[physicalPath] = (stamp, size);
            return size;
        }
        catch
        {
            return null;
        }
    }

    private static (int W, int H)? Parse(ReadOnlySpan<byte> b)
    {
        // PNG: imza + IHDR (genişlik/yükseklik big-endian, 16. bayttan itibaren)
        if (b.Length >= 24 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47)
        {
            return Ok(BE32(b, 16), BE32(b, 20));
        }

        // WebP: RIFF....WEBP + VP8 / VP8L / VP8X parçası
        if (b.Length >= 30 && b[0] == 'R' && b[1] == 'I' && b[2] == 'F' && b[3] == 'F'
            && b[8] == 'W' && b[9] == 'E' && b[10] == 'B' && b[11] == 'P')
        {
            var chunk = System.Text.Encoding.ASCII.GetString(b.Slice(12, 4));
            switch (chunk)
            {
                case "VP8X":
                    return Ok(1 + (b[24] | b[25] << 8 | b[26] << 16), 1 + (b[27] | b[28] << 8 | b[29] << 16));
                case "VP8L":
                    if (b[20] != 0x2F) return null;
                    var bits = (uint)(b[21] | b[22] << 8 | b[23] << 16 | b[24] << 24);
                    return Ok((int)(bits & 0x3FFF) + 1, (int)((bits >> 14) & 0x3FFF) + 1);
                case "VP8 ":
                    if (b[23] != 0x9D || b[24] != 0x01 || b[25] != 0x2A) return null;
                    return Ok((b[26] | b[27] << 8) & 0x3FFF, (b[28] | b[29] << 8) & 0x3FFF);
            }
            return null;
        }

        // JPEG: SOFn işaretçisine kadar segmentleri atla
        if (b.Length >= 4 && b[0] == 0xFF && b[1] == 0xD8)
        {
            var i = 2;
            while (i + 9 < b.Length)
            {
                if (b[i] != 0xFF) { i++; continue; }
                var marker = b[i + 1];
                if (marker == 0xFF) { i++; continue; }
                if (marker == 0xD8 || marker == 0x01 || (marker >= 0xD0 && marker <= 0xD7)) { i += 2; continue; }
                var len = b[i + 2] << 8 | b[i + 3];
                var isSof = marker >= 0xC0 && marker <= 0xCF && marker != 0xC4 && marker != 0xC8 && marker != 0xCC;
                if (isSof) return Ok(b[i + 7] << 8 | b[i + 8], b[i + 5] << 8 | b[i + 6]);
                if (len < 2) return null;
                i += 2 + len;
            }
        }
        return null;
    }

    private static int BE32(ReadOnlySpan<byte> b, int o) => b[o] << 24 | b[o + 1] << 16 | b[o + 2] << 8 | b[o + 3];

    private static (int W, int H)? Ok(int w, int h) =>
        w > 0 && h > 0 && w < 40000 && h < 40000 ? (w, h) : null;
}
