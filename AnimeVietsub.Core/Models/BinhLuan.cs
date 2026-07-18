using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeVietsub.Core.Models
{
    public class BinhLuan
    {
        [Key]
        public int BinhLuanId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser NguoiDung { get; set; }

        public int AnimeId { get; set; }
        public Anime Anime { get; set; }

        [Required]
        [StringLength(1000)]
        public string NoiDung { get; set; }

        public int? SoSao { get; set; } // danh gia 1-5 sao, co the null neu chi la binh luan thuong

        public DateTime NgayBinhLuan { get; set; } = DateTime.Now;
        public bool DaAn { get; set; } = false; // Admin an binh luan vi pham
    }

    // Lab 12: Chuc nang Tin tuc, upload hinh anh trong Summernote
    public class TinTuc
    {
        [Key]
        public int TinTucId { get; set; }

        [Required]
        [StringLength(250)]
        public string TieuDe { get; set; }

        [StringLength(300)]
        public string SlugUrl { get; set; }

        public string AnhDaiDien { get; set; }

        [StringLength(500)]
        public string MoTaNgan { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string NoiDung { get; set; } // HTML tu Summernote, co the chua nhieu anh upload rieng

        public DateTime NgayDang { get; set; } = DateTime.Now;
        public bool AnHien { get; set; } = true;
    }
}
