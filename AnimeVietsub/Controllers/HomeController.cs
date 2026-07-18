using AnimeVietsub.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) => _db = db;

        // GET: / -> cung cap du lieu cho cac section cua template: Hero, Trending, Popular, Recent, Live, sidebar
        public async Task<IActionResult> Index()
        {
            // Hero slider: cac anime duoc danh dau "Noi bat" (Lab 11 - checkbox NoiBat khi them Anime)
            ViewBag.HeroSlides = await _db.Animes
                .Include(a => a.DanhSachTheLoai).ThenInclude(t => t.TheLoai)
                .Where(a => a.NoiBat && a.AnHien)
                .OrderByDescending(a => a.NgayTao)
                .Take(3)
                .ToListAsync();

            // Trending Now: xem nhieu nhat
            ViewBag.TrendingNow = await _db.Animes
                .Include(a => a.DanhSachTap)
                .Where(a => a.AnHien)
                .OrderByDescending(a => a.LuotXem)
                .Take(6)
                .ToListAsync();

            // Popular Shows: diem danh gia cao nhat
            ViewBag.PopularShows = await _db.Animes
                .Include(a => a.DanhSachTap)
                .Where(a => a.AnHien)
                .OrderByDescending(a => a.DiemDanhGia)
                .Take(6)
                .ToListAsync();

            // Recently Added: moi cap nhat
            ViewBag.RecentlyAdded = await _db.Animes
                .Include(a => a.DanhSachTap)
                .Where(a => a.AnHien)
                .OrderByDescending(a => a.NgayTao)
                .Take(6)
                .ToListAsync();

            // Live Action: dang chieu
            ViewBag.LiveAction = await _db.Animes
                .Include(a => a.DanhSachTap)
                .Where(a => a.AnHien && a.TrangThai == "Đang chiếu")
                .OrderByDescending(a => a.NgayTao)
                .Take(6)
                .ToListAsync();

            // Sidebar: Top Views
            ViewBag.TopViews = await _db.Animes
                .Where(a => a.AnHien)
                .OrderByDescending(a => a.LuotXem)
                .Take(5)
                .ToListAsync();

            // Sidebar: Binh luan moi nhat
            ViewBag.BinhLuanMoi = await _db.BinhLuans
                .Include(b => b.Anime)
                .Where(b => !b.DaAn)
                .OrderByDescending(b => b.NgayBinhLuan)
                .Take(4)
                .ToListAsync();

            return View();
        }

        public IActionResult Error() => View();

        public IActionResult KhongCoQuyen() => View();

        public IActionResult LienHe() => View();

        public IActionResult Donate() => View();
    }
}
