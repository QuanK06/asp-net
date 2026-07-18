using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeVietsub.Database
{
    // Lab 08-09: Phan quyen truy cap chuc nang
    // Chay 1 lan khi ung dung khoi dong de tao san Role va tai khoan Admin dau tien
    public static class SeedData
    {
        public static async Task KhoiTaoAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // 1. Tao 2 role co ban
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Tao tai khoan Admin mac dinh (doi mat khau ngay sau khi dang nhap lan dau!)
            string emailAdmin = "admin@animevietsub.local";
            var adminUser = await userManager.FindByEmailAsync(emailAdmin);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = emailAdmin,
                    Email = emailAdmin,
                    HoTen = "Quan tri vien",
                    DanhHieu = "Boss", // BossOnlyAttribute chi cho phep danh hieu nay vao /Admin
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else if (adminUser.DanhHieu != "Boss")
            {
                // Tai khoan da ton tai tu truoc (khi con dung [Authorize(Roles="Admin")]) -> backfill danh hieu Boss
                adminUser.DanhHieu = "Boss";
                await userManager.UpdateAsync(adminUser);
            }

            // 3. Seed vai the loai mau (Lab 10)
            if (!context.TheLoais.Any())
            {
                context.TheLoais.AddRange(
                    new TheLoai { TenTheLoai = "Hanh dong", SlugUrl = "hanh-dong" },
                    new TheLoai { TenTheLoai = "Isekai", SlugUrl = "isekai" },
                    new TheLoai { TenTheLoai = "Hoc duong", SlugUrl = "hoc-duong" },
                    new TheLoai { TenTheLoai = "Tinh cam", SlugUrl = "tinh-cam" },
                    new TheLoai { TenTheLoai = "Kinh di", SlugUrl = "kinh-di" },
                    new TheLoai { TenTheLoai = "Gia tuong", SlugUrl = "gia-tuong" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
