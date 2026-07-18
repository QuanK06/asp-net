using System.ComponentModel.DataAnnotations;

namespace AnimeVietsub.Core.Models
{
    // Bang MOI hoan toan, khong co trong khoa hoc goc.
    // Ly do them: web ban hang chi ban 1 san pham, con web xem anime 1 Anime co NHIEU TAP.
    // Duoc tao va quan ly trong Lab 11 (gop chung voi quan ly Anime).
    public class TapPhim
    {
        [Key]
        public int TapPhimId { get; set; }

        public int AnimeId { get; set; }
        public Anime Anime { get; set; }

        public int SoTap { get; set; } // Tap so 1, 2, 3...

        [StringLength(150)]
        public string TieuDeTap { get; set; } // vd: "Tap 1: Khoi dau"

        // Lab 06 (Upload hinh anh Ajax) ap dung cho Poster/Thumbnail
        public string Thumbnail { get; set; }

        // DOI: khong upload file that len server nua (Somee free khong du dung luong),
        // chuyen sang nhap LINK video (mp4 truc tiep hoac link nhung YouTube/Google Drive...)
        [Required]
        public string DuongDanVideo { get; set; } // co the la link .mp4 truc tiep, hoac link embed (vd youtube.com/embed/xxx)

        [StringLength(20)]
        public string LoaiNguonVideo { get; set; } = "TrucTiep"; // "TrucTiep" (the <video>) hoac "Embed" (the <iframe>, vd YouTube)

        public int ThoiLuongGiay { get; set; } // thoi luong tap phim (giay)

        public int LuotXem { get; set; } = 0;

        public DateTime NgayDang { get; set; } = DateTime.Now;

        public ICollection<LichSuXem> LichSuXemTap { get; set; }
    }
}
