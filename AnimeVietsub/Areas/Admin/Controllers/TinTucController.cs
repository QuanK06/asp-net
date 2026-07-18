using AnimeVietsub.Filters;
using AnimeVietsub.Database;
using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [BossOnly]
    public class TinTucController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;
        public TinTucController(ApplicationDbContext db, IWebHostEnvironment env) { _db = db; _env = env; }

        public async Task<IActionResult> Index()
        {
            var ds = await _db.TinTucs.OrderByDescending(t => t.NgayDang).ToListAsync();
            return View(ds);
        }

        [HttpGet]
        public async Task<IActionResult> ThemSua(int id = 0)
        {
            if (id == 0) return View(new TinTuc());
            var tt = await _db.TinTucs.FindAsync(id);
            if (tt == null) return NotFound();
            return View(tt);
        }

        [HttpPost]
        public async Task<IActionResult> ThemSua(TinTuc model, Microsoft.AspNetCore.Http.IFormFile fileAnhDaiDien)
        {
            TinTuc tt;
            if (model.TinTucId == 0)
            {
                tt = new TinTuc();
                _db.TinTucs.Add(tt);
            }
            else
            {
                tt = await _db.TinTucs.FindAsync(model.TinTucId);
                if (tt == null) return NotFound();
            }

            tt.TieuDe = model.TieuDe;
            tt.MoTaNgan = model.MoTaNgan;
            tt.NoiDung = model.NoiDung; // HTML tu Summernote, da chua san img src neu dung upload rieng
            tt.AnHien = model.AnHien;
            tt.SlugUrl = model.TieuDe?.Trim().ToLower().Replace(" ", "-");

            if (fileAnhDaiDien != null)
            {
                string thuMuc = Path.Combine(_env.WebRootPath, "uploads/tintuc");
                Directory.CreateDirectory(thuMuc);
                string tenFile = $"{Guid.NewGuid()}{Path.GetExtension(fileAnhDaiDien.FileName)}";
                using var stream = new FileStream(Path.Combine(thuMuc, tenFile), FileMode.Create);
                await fileAnhDaiDien.CopyToAsync(stream);
                tt.AnhDaiDien = $"/uploads/tintuc/{tenFile}";
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Lab 12: endpoint rieng de Summernote upload anh chen vao noi dung
        [HttpPost]
        public async Task<IActionResult> UploadAnhSummernote(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null) return Json(new { success = false });

            string thuMuc = Path.Combine(_env.WebRootPath, "uploads/tintuc");
            Directory.CreateDirectory(thuMuc);
            string tenFile = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using (var stream = new FileStream(Path.Combine(thuMuc, tenFile), FileMode.Create))
                await file.CopyToAsync(stream);

            // Summernote can dung dinh dang tra ve: { url: "..." }
            return Json(new { url = $"/uploads/tintuc/{tenFile}" });
        }

        [HttpPost]
        public async Task<IActionResult> Xoa(int id)
        {
            var tt = await _db.TinTucs.FindAsync(id);
            if (tt == null) return Json(new { success = false });
            _db.TinTucs.Remove(tt);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
