using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AnimeVietsub.Filters
{
    // Thay the cho [Authorize(Roles = "Admin")]: chi cho phep tai khoan co DanhHieu = "Boss" vao khu vuc Admin
    public class BossOnlyAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;

            if (!(httpContext.User.Identity?.IsAuthenticated ?? false))
            {
                // Chua dang nhap -> chuyen ve trang dang nhap (kem returnUrl)
                context.Result = new RedirectResult($"/TaiKhoan/DangNhap?returnUrl={Uri.EscapeDataString(httpContext.Request.Path)}");
                return;
            }

            var userManager = httpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.GetUserAsync(httpContext.User);

            if (user == null || user.DanhHieu != "Boss")
            {
                // Da dang nhap nhung khong phai Boss -> khong co quyen
                context.Result = new RedirectResult("/Home/KhongCoQuyen");
            }
        }
    }
}
