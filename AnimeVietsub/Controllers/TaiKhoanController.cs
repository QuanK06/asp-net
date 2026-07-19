using AnimeVietsub.Core.Models;
using AnimeVietsub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Encodings.Web;

namespace AnimeVietsub.Controllers
{
    // Lab 07: Thiet ke trang dang nhap (tai khoan nguoi dung, khong phai admin)
    public class TaiKhoanController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public TaiKhoanController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult DangNhap(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(string email, string matKhau, bool ghiNho, string returnUrl = null)
        {
            var result = await _signInManager.PasswordSignInAsync(email, matKhau, ghiNho, lockoutOnFailure: false);
            if (result.Succeeded)
                return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
            ViewBag.Email = email; // giu lai email da nhap, nguoi dung chi can sua mat khau
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        public IActionResult DangKy() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(string hoTen, string email, string matKhau)
        {
            var user = new ApplicationUser { UserName = email, Email = email, HoTen = hoTen };
            var result = await _userManager.CreateAsync(user, matKhau);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View();
        }

        public async Task<IActionResult> DangXuat()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> HoSo()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        // ===================== QUEN MAT KHAU =====================

        [HttpGet]
        public IActionResult QuenMatKhau() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuenMatKhau(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // Du user co ton tai hay khong cung bao thanh cong -> tranh lo email nao da dang ky
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var tokenBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

                var linkDatLai = Url.Action("DatLaiMatKhau", "TaiKhoan",
                    new { email = user.Email, token = tokenBase64 }, protocol: Request.Scheme);

                var noiDung = $@"
                    <h3>Đặt lại mật khẩu AnimeVietsub</h3>
                    <p>Xin chào {user.HoTen ?? user.UserName},</p>
                    <p>Bạn (hoặc ai đó) vừa yêu cầu đặt lại mật khẩu cho tài khoản này. Bấm vào link bên dưới để đặt mật khẩu mới:</p>
                    <p><a href='{HtmlEncoder.Default.Encode(linkDatLai)}'>Đặt lại mật khẩu</a></p>
                    <p>Nếu không phải bạn yêu cầu, hãy bỏ qua email này.</p>";

                try
                {
                    await _emailSender.GuiEmailAsync(user.Email, "Đặt lại mật khẩu - AnimeVietsub", noiDung);
                }
                catch
                {
                    // SMTP chua cau hinh dung (xem appsettings.json > EmailSettings) -> khong lam sap trang, chi bao loi nhe
                    ViewBag.LoiGuiMail = "Không gửi được email. Vui lòng kiểm tra lại cấu hình SMTP trong appsettings.json (mục EmailSettings).";
                    return View();
                }
            }

            ViewBag.DaGuiThanhCong = true;
            return View();
        }

        [HttpGet]
        public IActionResult DatLaiMatKhau(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return RedirectToAction("DangNhap");

            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DatLaiMatKhau(string email, string token, string matKhauMoi)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Khong tiet lo email khong ton tai, van bao thanh cong nhu binh thuong
                return RedirectToAction("DatLaiMatKhauThanhCong");
            }

            string tokenGoc;
            try
            {
                tokenGoc = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            }
            catch
            {
                ModelState.AddModelError("", "Link đặt lại mật khẩu không hợp lệ hoặc đã hết hạn");
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, tokenGoc, matKhauMoi);
            if (result.Succeeded)
                return RedirectToAction("DatLaiMatKhauThanhCong");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }

        [HttpGet]
        public IActionResult DatLaiMatKhauThanhCong() => View();
    }
}
