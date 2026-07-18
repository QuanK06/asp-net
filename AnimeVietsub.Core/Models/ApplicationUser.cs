using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AnimeVietsub.Core.Models
{
    // Lab 07: mo rong IdentityUser de them thong tin ho so cho web anime
    public class ApplicationUser : IdentityUser
    {
        public string HoTen { get; set; }
        public string AnhDaiDien { get; set; } // duong dan anh avatar
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public bool BiKhoa { get; set; } = false; // dung o Lab 08: khoa/mo tai khoan

        [StringLength(30)]
        public string DanhHieu { get; set; } // null/rong = thanh vien thuong. Cac gia tri: "Fan cung", "Boss", "Otaku", "Mod"

        public ICollection<BinhLuan> DanhSachBinhLuan { get; set; }
        public ICollection<TheoDoi> DanhSachTheoDoi { get; set; }
        public ICollection<LichSuXem> LichSuXem { get; set; }
    }
}
