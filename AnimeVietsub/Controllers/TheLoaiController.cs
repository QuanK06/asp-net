using AnimeVietsub.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Controllers
{
    // Lab 10: Active thanh dieu huong va chuc nang Chuyen muc (o day la The loai)
    public class TheLoaiController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TheLoaiController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var ds = await _db.TheLoais.Where(t => t.HienThi).ToListAsync();
            return View(ds);
        }
    }
}
