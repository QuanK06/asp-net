using AnimeVietsub.Core.Models;
using AnimeVietsub.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Controllers
{
    public class AnimeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public AnimeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Anime/DanhSach?theLoaiId=&dinhDang=&phuongThucDich=&trangThai=&nam=&trang=
        public async Task<IActionResult> DanhSach(int? theLoaiId, string dinhDang, string phuongThucDich, string trangThai, int? nam, int trang = 1)
        {
            const int soDongMoiTrang = 12;

            var query = _db.Animes.Include(a => a.DanhSachTap).Where(a => a.AnHien).AsQueryable();
            if (theLoaiId.HasValue)
                query = query.Where(a => a.DanhSachTheLoai.Any(t => t.TheLoaiId == theLoaiId));
            if (!string.IsNullOrEmpty(dinhDang))
                query = query.Where(a => a.DinhDangPhatSong == dinhDang);
            if (!string.IsNullOrEmpty(phuongThucDich))
                query = query.Where(a => a.PhuongThucDich == phuongThucDich);
            if (!string.IsNullOrEmpty(trangThai))
                query = query.Where(a => a.TrangThai == trangThai);
            if (nam.HasValue)
                query = query.Where(a => a.NamPhatHanh == nam);

            var tongSo = await query.CountAsync();
            var ds = await query.OrderByDescending(a => a.NgayTao)
                .Skip((trang - 1) * soDongMoiTrang)
                .Take(soDongMoiTrang)
                .ToListAsync();

            ViewBag.TongSoTrang = (int)Math.Ceiling(tongSo / (double)soDongMoiTrang);
            ViewBag.TrangHienTai = trang;
            ViewBag.TheLoais = await _db.TheLoais.Where(t => t.HienThi).ToListAsync();
            ViewBag.TheLoaiIdHienTai = theLoaiId;
            ViewBag.TopViews = await _db.Animes.Where(a => a.AnHien).OrderByDescending(a => a.LuotXem).Take(5).ToListAsync();

            // Lab "Loc nang cao": danh sach cac gia tri co san de do vao dropdown loc
            ViewBag.DinhDangHienTai = dinhDang;
            ViewBag.PhuongThucDichHienTai = phuongThucDich;
            ViewBag.TrangThaiHienTai = trangThai;
            ViewBag.NamHienTai = nam;
            ViewBag.DanhSachNam = await _db.Animes.Where(a => a.NamPhatHanh != null)
                .Select(a => a.NamPhatHanh.Value).Distinct().OrderByDescending(n => n).ToListAsync();

            if (theLoaiId.HasValue)
            {
                var tl = await _db.TheLoais.FindAsync(theLoaiId.Value);
                ViewBag.TenTrang = tl?.TenTheLoai ?? "Danh sách Anime";
            }
            else
            {
                ViewBag.TenTrang = "Tất cả Anime";
            }

            return View(ds);
        }

        // GET: /Anime/TimKiem?tuKhoa=
        public async Task<IActionResult> TimKiem(string tuKhoa)
        {
            var ds = string.IsNullOrWhiteSpace(tuKhoa)
                ? new List<Anime>()
                : await _db.Animes
                    .Include(a => a.DanhSachTap)
                    .Where(a => a.AnHien && (a.TenAnime.Contains(tuKhoa) || a.TenKhac.Contains(tuKhoa)))
                    .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TopViews = await _db.Animes.Where(a => a.AnHien).OrderByDescending(a => a.LuotXem).Take(5).ToListAsync();
            return View(ds);
        }

        // GET: /Anime/ChiTiet/5
        public async Task<IActionResult> ChiTiet(int id)
        {
            var anime = await _db.Animes
                .Include(a => a.DanhSachTap.OrderBy(t => t.SoTap))
                .Include(a => a.DanhSachTheLoai).ThenInclude(t => t.TheLoai)
                .Include(a => a.DanhSachBinhLuan.Where(b => !b.DaAn)).ThenInclude(b => b.NguoiDung)
                .FirstOrDefaultAsync(a => a.AnimeId == id);

            if (anime == null) return NotFound();

            anime.LuotXem++;
            await _db.SaveChangesAsync();

            // "You might like" sidebar: anime khac cung the loai
            var theLoaiIds = anime.DanhSachTheLoai.Select(t => t.TheLoaiId).ToList();
            ViewBag.CoTheThich = await _db.Animes
                .Where(a => a.AnimeId != id && a.AnHien && a.DanhSachTheLoai.Any(t => theLoaiIds.Contains(t.TheLoaiId)))
                .Take(4)
                .ToListAsync();

            ViewBag.SoNguoiTheoDoi = await _db.TheoDois.CountAsync(t => t.AnimeId == id);

            return View(anime);
        }

        // GET: /Anime/Xem/{tapPhimId}  -> trang xem 1 tap (player)
        public async Task<IActionResult> Xem(int tapPhimId)
        {
            var tap = await _db.TapPhims
                .Include(t => t.Anime).ThenInclude(a => a.DanhSachTap.OrderBy(x => x.SoTap))
                .Include(t => t.Anime).ThenInclude(a => a.DanhSachBinhLuan.Where(b => !b.DaAn)).ThenInclude(b => b.NguoiDung)
                .FirstOrDefaultAsync(t => t.TapPhimId == tapPhimId);

            if (tap == null) return NotFound();

            tap.LuotXem++;
            await _db.SaveChangesAsync();

            // Cho thanh dieu khien duoi player: tap tiep theo + trang thai theo doi
            ViewBag.TapTiepTheo = tap.Anime.DanhSachTap
                .Where(t => t.SoTap > tap.SoTap)
                .OrderBy(t => t.SoTap)
                .FirstOrDefault();

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                ViewBag.DangTheoDoi = await _db.TheoDois.AnyAsync(t => t.UserId == userId && t.AnimeId == tap.AnimeId);
            }

            return View(tap);
        }
    }
}
