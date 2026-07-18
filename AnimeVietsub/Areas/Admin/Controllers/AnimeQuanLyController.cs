using AnimeVietsub.Filters;
using AnimeVietsub.Areas.Admin.Models;
using AnimeVietsub.Database;
using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [BossOnly]
    public class AnimeQuanLyController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AnimeQuanLyController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /Admin/AnimeQuanLy
        public IActionResult Index() => View();

        // Lab 04: Ajax do du lieu vao DataTable
        [HttpGet]
        public async Task<IActionResult> LayDanhSach()
        {
            var ds = await _db.Animes
                .OrderByDescending(a => a.AnimeId)
                .Select(a => new
                {
                    a.AnimeId,
                    a.TenAnime,
                    a.Poster,
                    a.TrangThai,
                    a.DinhDangPhatSong,
                    a.SoMua,
                    a.AnHien,
                    SoTap = a.DanhSachTap.Count()
                }).ToListAsync();

            return Json(new { data = ds });
        }

        // GET: /Admin/AnimeQuanLy/ThemSua/5  (id = 0 -> tao moi)
        [HttpGet]
        public async Task<IActionResult> ThemSua(int id = 0)
        {
            var vm = new AnimeFormViewModel
            {
                TatCaTheLoai = await _db.TheLoais.ToListAsync()
            };

            if (id != 0)
            {
                var anime = await _db.Animes
                    .Include(a => a.DanhSachTheLoai)
                    .FirstOrDefaultAsync(a => a.AnimeId == id);
                if (anime == null) return NotFound();

                vm.Anime = anime;
                vm.TheLoaiDaChon = anime.DanhSachTheLoai.Select(t => t.TheLoaiId).ToList();
            }

            return View(vm);
        }

        // POST: /Admin/AnimeQuanLy/ThemSua (Lab 05 + Lab 06: Ajax them/sua + upload hinh anh)
        [HttpPost]
        public async Task<IActionResult> ThemSua(AnimeFormViewModel vm)
        {
            Anime anime;

            if (vm.Anime.AnimeId == 0)
            {
                anime = new Anime();
                _db.Animes.Add(anime);
            }
            else
            {
                anime = await _db.Animes
                    .Include(a => a.DanhSachTheLoai)
                    .FirstOrDefaultAsync(a => a.AnimeId == vm.Anime.AnimeId);
                if (anime == null) return NotFound();
            }

            anime.TenAnime = vm.Anime.TenAnime;
            anime.TenKhac = vm.Anime.TenKhac;
            anime.MoTa = vm.Anime.MoTa;
            anime.NamPhatHanh = vm.Anime.NamPhatHanh;
            anime.TrangThai = vm.Anime.TrangThai;
            anime.DinhDangPhatSong = vm.Anime.DinhDangPhatSong;
            anime.SoMua = vm.Anime.SoMua;
            anime.PhuongThucDich = vm.Anime.PhuongThucDich;
            anime.QuocGia = vm.Anime.QuocGia;
            anime.DaoDien = vm.Anime.DaoDien;
            anime.Studio = vm.Anime.Studio;
            anime.ChatLuong = vm.Anime.ChatLuong;
            anime.Rating = vm.Anime.Rating;
            anime.ThoiLuongPhut = vm.Anime.ThoiLuongPhut;
            anime.TrailerUrl = vm.Anime.TrailerUrl;
            anime.NoiBat = vm.Anime.NoiBat;
            anime.AnHien = vm.Anime.AnHien;
            anime.SlugUrl = TaoSlug(vm.Anime.TenAnime);

            // Lab 06: Upload hinh anh voi Ajax (o day submit form thuong, nguyen ly luu file giong nhau)
            if (vm.FilePoster != null)
                anime.Poster = await LuuFile(vm.FilePoster, "uploads/posters");

            if (vm.FileBanner != null)
                anime.AnhBanner = await LuuFile(vm.FileBanner, "uploads/posters");

            await _db.SaveChangesAsync();

            // Cap nhat lai the loai (xoa cu, them moi)
            var theLoaiCu = _db.AnimeTheLoais.Where(t => t.AnimeId == anime.AnimeId);
            _db.AnimeTheLoais.RemoveRange(theLoaiCu);
            foreach (var tlId in vm.TheLoaiDaChon ?? new List<int>())
                _db.AnimeTheLoais.Add(new AnimeTheLoai { AnimeId = anime.AnimeId, TheLoaiId = tlId });

            await _db.SaveChangesAsync();

            TempData["ThongBao"] = "Lưu anime thành công";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Xoa(int id)
        {
            var anime = await _db.Animes.FindAsync(id);
            if (anime == null) return Json(new { success = false });
            _db.Animes.Remove(anime); // Cascade se xoa luon Tap phim (xem ApplicationDbContext)
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        // ===================== QUAN LY TAP PHIM (Lab 11) =====================

        [HttpGet]
        public async Task<IActionResult> QuanLyTap(int animeId)
        {
            var anime = await _db.Animes
                .Include(a => a.DanhSachTap.OrderBy(t => t.SoTap))
                .FirstOrDefaultAsync(a => a.AnimeId == animeId);
            if (anime == null) return NotFound();
            return View(anime);
        }

        // Lab 06: Upload video that len server (wwwroot/uploads/videos)
        [HttpPost]
        public async Task<IActionResult> LuuTap(int animeId, int tapPhimId, int soTap, string tieuDeTap,
            string duongDanVideo, string loaiNguonVideo, Microsoft.AspNetCore.Http.IFormFile fileThumbnail)
        {
            TapPhim tap;
            if (tapPhimId == 0)
            {
                tap = new TapPhim { AnimeId = animeId };
                _db.TapPhims.Add(tap);
            }
            else
            {
                tap = await _db.TapPhims.FindAsync(tapPhimId);
                if (tap == null) return Json(new { success = false, message = "Không tìm thấy tập" });
            }

            tap.SoTap = soTap;
            tap.TieuDeTap = tieuDeTap;

            if (!string.IsNullOrWhiteSpace(duongDanVideo))
                tap.DuongDanVideo = duongDanVideo.Trim();
            tap.LoaiNguonVideo = string.IsNullOrEmpty(loaiNguonVideo) ? "TrucTiep" : loaiNguonVideo;

            if (fileThumbnail != null)
                tap.Thumbnail = await LuuFile(fileThumbnail, "uploads/thumbnails");

            if (string.IsNullOrEmpty(tap.DuongDanVideo))
                return Json(new { success = false, message = "Vui lòng nhập link video" });

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> XoaTap(int id)
        {
            var tap = await _db.TapPhims.FindAsync(id);
            if (tap == null) return Json(new { success = false });
            _db.TapPhims.Remove(tap);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        // ===================== HAM DUNG CHUNG =====================

        private async Task<string> LuuFile(Microsoft.AspNetCore.Http.IFormFile file, string thuMucTuongDoi)
        {
            string thuMuc = Path.Combine(_env.WebRootPath, thuMucTuongDoi);
            Directory.CreateDirectory(thuMuc);

            string tenFile = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string duongDanDayDu = Path.Combine(thuMuc, tenFile);

            using (var stream = new FileStream(duongDanDayDu, FileMode.Create))
                await file.CopyToAsync(stream);

            return $"/{thuMucTuongDoi}/{tenFile}";
        }

        private static string TaoSlug(string chuoi) => chuoi?.Trim().ToLower().Replace(" ", "-") ?? "";
    }
}
