using AnimeVietsub.Database;
using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnimeVietsub.Controllers
{
    [Authorize]
    public class BinhLuanController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BinhLuanController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // POST (Ajax): /BinhLuan/Them
        [HttpPost]
        public async Task<IActionResult> Them(int animeId, string noiDung, int? soSao)
        {
            if (string.IsNullOrWhiteSpace(noiDung))
                return Json(new { success = false, message = "Vui lòng nhập nội dung bình luận" });

            var user = await _userManager.GetUserAsync(User);
            var bl = new BinhLuan
            {
                AnimeId = animeId,
                UserId = user.Id,
                NoiDung = noiDung.Trim(),
                SoSao = soSao
            };
            _db.BinhLuans.Add(bl);
            await _db.SaveChangesAsync();

            return Json(new
            {
                success = true,
                hoTen = string.IsNullOrEmpty(user.HoTen) ? user.UserName : user.HoTen,
                noiDung = bl.NoiDung,
                ngay = bl.NgayBinhLuan.ToString("dd/MM/yyyy HH:mm")
            });
        }
    }
}
