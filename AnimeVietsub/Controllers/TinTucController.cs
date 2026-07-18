using AnimeVietsub.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Controllers
{
    public class TinTucController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TinTucController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var ds = await _db.TinTucs.Where(t => t.AnHien).OrderByDescending(t => t.NgayDang).ToListAsync();
            return View(ds);
        }

        public async Task<IActionResult> ChiTiet(int id)
        {
            var tt = await _db.TinTucs.FirstOrDefaultAsync(t => t.TinTucId == id);
            if (tt == null) return NotFound();
            return View(tt);
        }
    }
}
