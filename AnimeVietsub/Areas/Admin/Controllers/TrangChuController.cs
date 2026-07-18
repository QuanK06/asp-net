using AnimeVietsub.Filters;
using AnimeVietsub.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [BossOnly]
    public class TrangChuController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TrangChuController(ApplicationDbContext db) => _db = db;

        // Lab 14: Chartjs - Xay dung trang thong ke
        public async Task<IActionResult> Index()
        {
            ViewBag.TongAnime = await _db.Animes.CountAsync();
            ViewBag.TongTap = await _db.TapPhims.CountAsync();
            ViewBag.TongUser = await _db.Users.CountAsync();
            ViewBag.TongBinhLuan = await _db.BinhLuans.CountAsync();

            // Top 5 anime xem nhieu nhat -> ve bieu do cot
            var topAnime = await _db.Animes
                .OrderByDescending(a => a.LuotXem)
                .Take(5)
                .Select(a => new { a.TenAnime, a.LuotXem })
                .ToListAsync();

            ViewBag.NhanTopAnime = topAnime.Select(a => a.TenAnime).ToList();
            ViewBag.SoLieuTopAnime = topAnime.Select(a => a.LuotXem).ToList();

            // Thong ke user dang ky theo thang (7 thang gan nhat) -> ve bieu do duong
            var moc = DateTime.Now.AddMonths(-6);
            var userTheoThang = await _db.Users
                .Where(u => u.NgayTao >= moc)
                .ToListAsync();

            var nhomThang = Enumerable.Range(0, 7)
                .Select(i => moc.AddMonths(i))
                .Select(d => new
                {
                    Nhan = $"{d.Month}/{d.Year}",
                    SoLuong = userTheoThang.Count(u => u.NgayTao.Month == d.Month && u.NgayTao.Year == d.Year)
                }).ToList();

            ViewBag.NhanUserTheoThang = nhomThang.Select(x => x.Nhan).ToList();
            ViewBag.SoLieuUserTheoThang = nhomThang.Select(x => x.SoLuong).ToList();

            return View();
        }
    }
}
