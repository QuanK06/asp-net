using AnimeVietsub.Filters;
using AnimeVietsub.Database;
using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [BossOnly]
    public class TaiKhoanController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaiKhoanController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var ds = await _db.Users.OrderByDescending(u => u.NgayTao).ToListAsync();
            return View(ds);
        }

        // Lab 08: Phan quyen truy cap chuc nang - Admin khoa/mo tai khoan User
        [HttpPost]
        public async Task<IActionResult> ToggleKhoa(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return Json(new { success = false });

            user.BiKhoa = !user.BiKhoa;
            // Dung luon co che lockout co san cua Identity de chan dang nhap that su
            user.LockoutEnd = user.BiKhoa ? DateTimeOffset.MaxValue : null;
            user.LockoutEnabled = true;

            await _userManager.UpdateAsync(user);
            return Json(new { success = true, biKhoa = user.BiKhoa });
        }

        // Gan danh hieu cho user: Fan cung / Boss / Otaku / Mod (chi la nhan hien thi, khong cap quyen that)
        [HttpPost]
        public async Task<IActionResult> GanDanhHieu(string id, string danhHieu)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return Json(new { success = false });

            user.DanhHieu = string.IsNullOrEmpty(danhHieu) ? null : danhHieu;
            await _userManager.UpdateAsync(user);
            return Json(new { success = true, danhHieu = user.DanhHieu });
        }

        // Xoa user - phai xoa het du lieu lien quan truoc (Binh luan, Theo doi, Lich su xem)
        // vi trong DbContext cac quan he nay dang dat Restrict, khong tu dong xoa theo (Cascade)
        [HttpPost]
        public async Task<IActionResult> Xoa(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return Json(new { success = false, message = "Không tìm thấy người dùng" });

            // Khong cho xoa chinh tai khoan Admin dang dang nhap, tranh tu khoa minh ra khoi he thong
            var currentUserId = _userManager.GetUserId(User);
            if (id == currentUserId)
                return Json(new { success = false, message = "Không thể tự xóa chính tài khoản đang đăng nhập" });

            _db.BinhLuans.RemoveRange(_db.BinhLuans.Where(b => b.UserId == id));
            _db.TheoDois.RemoveRange(_db.TheoDois.Where(t => t.UserId == id));
            _db.LichSuXems.RemoveRange(_db.LichSuXems.Where(l => l.UserId == id));
            await _db.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            return Json(new { success = true });
        }
    }
}
