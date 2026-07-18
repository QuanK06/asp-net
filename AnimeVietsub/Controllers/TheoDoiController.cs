using AnimeVietsub.Database;
using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Controllers
{
    // Thay the cho GioHangController / DonHangController cua ban goc (Lab 09 gio hang, Lab 13 don hang)
    [Authorize]
    public class TheoDoiController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TheoDoiController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /TheoDoi  -> danh sach anime dang theo doi + lich su xem tiep
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            ViewBag.DangTheoDoi = await _db.TheoDois
                .Include(t => t.Anime).ThenInclude(a => a.DanhSachTap)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.NgayTheoDoi)
                .ToListAsync();

            ViewBag.XemTiep = await _db.LichSuXems
                .Include(l => l.TapPhim).ThenInclude(t => t.Anime)
                .Where(l => l.UserId == userId && !l.DaXemXong)
                .OrderByDescending(l => l.LanXemCuoi)
                .Take(10)
                .ToListAsync();

            return View();
        }

        // POST (Ajax): /TheoDoi/Toggle  -> bam nut Theo doi / Bo theo doi
        [HttpPost]
        public async Task<IActionResult> Toggle(int animeId)
        {
            var userId = _userManager.GetUserId(User);
            var existed = await _db.TheoDois.FirstOrDefaultAsync(t => t.UserId == userId && t.AnimeId == animeId);

            bool dangTheoDoi;
            if (existed != null)
            {
                _db.TheoDois.Remove(existed);
                dangTheoDoi = false;
            }
            else
            {
                _db.TheoDois.Add(new TheoDoi { UserId = userId, AnimeId = animeId });
                dangTheoDoi = true;
            }
            await _db.SaveChangesAsync();

            return Json(new { success = true, dangTheoDoi });
        }

        // POST (Ajax, goi tu player bang JS moi vai giay): luu vi tri xem do dang -> tinh nang "xem tiep"
        [HttpPost]
        public async Task<IActionResult> LuuTienDo(int tapPhimId, int giayDaXem, bool daXemXong = false)
        {
            var userId = _userManager.GetUserId(User);
            var lichSu = await _db.LichSuXems.FirstOrDefaultAsync(l => l.UserId == userId && l.TapPhimId == tapPhimId);

            if (lichSu == null)
            {
                lichSu = new LichSuXem { UserId = userId, TapPhimId = tapPhimId };
                _db.LichSuXems.Add(lichSu);
            }
            lichSu.GiayDaXem = giayDaXem;
            lichSu.DaXemXong = daXemXong;
            lichSu.LanXemCuoi = DateTime.Now;

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
