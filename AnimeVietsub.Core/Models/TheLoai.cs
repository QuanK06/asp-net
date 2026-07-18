using System.ComponentModel.DataAnnotations;

namespace AnimeVietsub.Core.Models
{
    // Thay the bang "Category/Chuyen muc" ben ban goc -> Lab 10: Chuc nang Chuyen muc
    public class TheLoai
    {
        [Key]
        public int TheLoaiId { get; set; }

        [Required(ErrorMessage = "Vui long nhap ten the loai")]
        [StringLength(100)]
        public string TenTheLoai { get; set; } // vd: Hanh dong, Isekai, Hoc duong...

        [StringLength(150)]
        public string SlugUrl { get; set; } // dung cho URL than thien: /the-loai/hanh-dong

        public bool HienThi { get; set; } = true;

        public ICollection<AnimeTheLoai> DanhSachAnime { get; set; }
    }

    // Bang trung gian nhieu-nhieu: 1 Anime co nhieu The loai, 1 The loai co nhieu Anime
    public class AnimeTheLoai
    {
        public int AnimeId { get; set; }
        public Anime Anime { get; set; }

        public int TheLoaiId { get; set; }
        public TheLoai TheLoai { get; set; }
    }
}
