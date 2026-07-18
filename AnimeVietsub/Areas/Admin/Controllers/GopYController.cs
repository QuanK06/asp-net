using AnimeVietsub.Filters;
using AnimeVietsub.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [BossOnly]
    public class GopYController : Controller
    {
        private readonly ApplicationDbContext _db;
        public GopYController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var ds = await _db.GopYs.OrderByDescending(g => g.NgayGui).ToListAsync();
            return View(ds);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleXuLy(int id)
        {
            var gy = await _db.GopYs.FindAsync(id);
            if (gy == null) return Json(new { success = false });
            gy.DaXuLy = !gy.DaXuLy;
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Xoa(int id)
        {
            var gy = await _db.GopYs.FindAsync(id);
            if (gy == null) return Json(new { success = false });
            _db.GopYs.Remove(gy);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
