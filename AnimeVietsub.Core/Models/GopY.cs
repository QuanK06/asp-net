using System.ComponentModel.DataAnnotations;

namespace AnimeVietsub.Core.Models
{
    // Bang moi: luu gop y/phan hoi tu nguoi dung (co the la khach chua dang nhap)
    public class GopY
    {
        [Key]
        public int GopYId { get; set; }

        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(1000)]
        public string NoiDung { get; set; }

        public DateTime NgayGui { get; set; } = DateTime.Now;
        public bool DaXuLy { get; set; } = false;
    }
}
