using AnimeVietsub.Filters;
using AnimeVietsub.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [BossOnly]
    public class BinhLuanController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BinhLuanController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var ds = await _db.BinhLuans
                .Include(b => b.NguoiDung)
                .Include(b => b.Anime)
                .OrderByDescending(b => b.NgayBinhLuan)
                .ToListAsync();
            return View(ds);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAn(int id)
        {
            var bl = await _db.BinhLuans.FindAsync(id);
            if (bl == null) return Json(new { success = false });
            bl.DaAn = !bl.DaAn;
            await _db.SaveChangesAsync();
            return Json(new { success = true, daAn = bl.DaAn });
        }

        [HttpPost]
        public async Task<IActionResult> Xoa(int id)
        {
            var bl = await _db.BinhLuans.FindAsync(id);
            if (bl == null) return Json(new { success = false });
            _db.BinhLuans.Remove(bl);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
