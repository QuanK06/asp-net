using System.ComponentModel.DataAnnotations;

namespace AnimeVietsub.Core.Models
{
    // THAY THE HOAN TOAN cho chuc nang "Gio hang" cua ban goc.
    // Ben ban goc: Cart -> Order -> OrderDetail (Lab 09 gio hang, Lab 13 quan ly don hang)
    // Ben nay: User "theo doi" 1 Anime (giong bam nut Follow), khong co thanh toan.
    public class TheoDoi
    {
        [Key]
        public int TheoDoiId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser NguoiDung { get; set; }

        public int AnimeId { get; set; }
        public Anime Anime { get; set; }

        public DateTime NgayTheoDoi { get; set; } = DateTime.Now;
    }

    // THAY THE cho OrderDetail / lich su mua hang.
    // Lab 13 (goc: "Quan ly don dat hang") o ban nay tro thanh trang Admin xem thong ke
    // + trang User xem "Lich su xem" / "Xem tiep tap dang do".
    public class LichSuXem
    {
        [Key]
        public int LichSuXemId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser NguoiDung { get; set; }

        public int TapPhimId { get; set; }
        public TapPhim TapPhim { get; set; }

        public int GiayDaXem { get; set; } // vi tri xem do dang (giay) -> tinh nang "xem tiep"
        public bool DaXemXong { get; set; } = false;

        public DateTime LanXemCuoi { get; set; } = DateTime.Now;
    }
}
