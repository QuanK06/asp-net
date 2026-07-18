using AnimeVietsub.Core.Models;
using AnimeVietsub.Database;
using Microsoft.AspNetCore.Mvc;

namespace AnimeVietsub.Controllers
{
    public class GopYController : Controller
    {
        private readonly ApplicationDbContext _db;
        public GopYController(ApplicationDbContext db) => _db = db;

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Gui(string hoTen, string email, string noiDung)
        {
            if (string.IsNullOrWhiteSpace(noiDung))
                return Json(new { success = false, message = "Vui lòng nhập nội dung góp ý" });

            _db.GopYs.Add(new GopY
            {
                HoTen = string.IsNullOrWhiteSpace(hoTen) ? "Khách" : hoTen.Trim(),
                Email = email?.Trim(),
                NoiDung = noiDung.Trim()
            });
            await _db.SaveChangesAsync();

            return Json(new { success = true });
        }
    }
}
