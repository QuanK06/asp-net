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
    public class TheLoaiController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TheLoaiController(ApplicationDbContext db) => _db = db;

        // GET: /Admin/TheLoai -> trang chua bang DataTable rong, du lieu do Ajax do vao
        public IActionResult Index() => View();

        // Lab 04: Ajax lay danh sach du lieu do vao JDatatable
        [HttpGet]
        public async Task<IActionResult> LayDanhSach()
        {
            var ds = await _db.TheLoais
                .OrderByDescending(t => t.TheLoaiId)
                .Select(t => new { t.TheLoaiId, t.TenTheLoai, t.SlugUrl, t.HienThi })
                .ToListAsync();

            return Json(new { data = ds });
        }

        // Lab 05: Ajax them / sua co ban -> dung chung 1 action, phan biet bang Id
        [HttpPost]
        public async Task<IActionResult> LuuTheLoai(int theLoaiId, string tenTheLoai, bool hienThi)
        {
            if (string.IsNullOrWhiteSpace(tenTheLoai))
                return Json(new { success = false, message = "Vui lòng nhập tên thể loại" });

            string slug = TaoSlug(tenTheLoai);

            if (theLoaiId == 0)
            {
                var tl = new TheLoai { TenTheLoai = tenTheLoai.Trim(), SlugUrl = slug, HienThi = hienThi };
                _db.TheLoais.Add(tl);
            }
            else
            {
                var tl = await _db.TheLoais.FindAsync(theLoaiId);
                if (tl == null) return Json(new { success = false, message = "Không tìm thấy thể loại" });
                tl.TenTheLoai = tenTheLoai.Trim();
                tl.SlugUrl = slug;
                tl.HienThi = hienThi;
            }

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> ChiTiet(int id)
        {
            var tl = await _db.TheLoais.FindAsync(id);
            if (tl == null) return Json(new { success = false });
            return Json(new { success = true, data = tl });
        }

        // Lab 05: Ajax xoa
        [HttpPost]
        public async Task<IActionResult> Xoa(int id)
        {
            var tl = await _db.TheLoais.FindAsync(id);
            if (tl == null) return Json(new { success = false, message = "Không tìm thấy" });

            _db.TheLoais.Remove(tl);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        private static string TaoSlug(string chuoi)
        {
            string s = chuoi.Trim().ToLower();
            string[] dau = { "à","á","ạ","ả","ã","â","ầ","ấ","ậ","ẩ","ẫ","ă","ằ","ắ","ặ","ẳ","ẵ",
                "è","é","ẹ","ẻ","ẽ","ê","ề","ế","ệ","ể","ễ","ì","í","ị","ỉ","ĩ",
                "ò","ó","ọ","ỏ","õ","ô","ồ","ố","ộ","ổ","ỗ","ơ","ờ","ớ","ợ","ở","ỡ",
                "ù","ú","ụ","ủ","ũ","ư","ừ","ứ","ự","ử","ữ","ỳ","ý","ỵ","ỷ","ỹ","đ" };
            string[] khongDau = { "a","a","a","a","a","a","a","a","a","a","a","a","a","a","a","a","a",
                "e","e","e","e","e","e","e","e","e","e","e","i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u","y","y","y","y","y","d" };
            for (int i = 0; i < dau.Length; i++) s = s.Replace(dau[i], khongDau[i]);
            s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-z0-9\s-]", "");
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", "-").Trim('-');
            return s;
        }
    }
}
