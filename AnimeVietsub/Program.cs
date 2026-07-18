using AnimeVietsub.Database;
using AnimeVietsub.Core.Models;
using AnimeVietsub.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Lab 02: Ket noi voi SQL Server =====
var connectionString = builder.Configuration.GetConnectionString("AnimeDb");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ===== Quen mat khau qua Email: cau hinh SMTP =====
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

// ===== Lab 07-09: Identity + phan quyen Admin/User =====
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Lam nhe yeu cau mat khau cho de test khi con dang hoc, sieters lai truoc khi len that
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/TaiKhoan/DangNhap";
    options.AccessDeniedPath = "/Home/KhongCoQuyen";
});

// ===== Lab 01: MVC + Areas (Admin) =====
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Route rieng cho Area Admin phai khai bao TRUOC route mac dinh
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=TrangChu}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ===== Lab 08-09: Seed Role + Admin mac dinh khi chay lan dau =====
using (var scope = app.Services.CreateScope())
{
    await SeedData.KhoiTaoAsync(scope.ServiceProvider);
}

app.Run();
