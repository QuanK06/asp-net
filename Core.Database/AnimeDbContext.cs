using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database
{
    public class AnimeDbContext : DbContext
    {
        public AnimeDbContext(DbContextOptions<AnimeDbContext> options) : base(options)
        {
        }

        // Khai báo các bảng dữ liệu cho Web Anime
        public DbSet<Category> Categories { get; set; }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<Episode> Episodes { get; set; } // Đặc thù web phim
        public DbSet<Article> Articles { get; set; }

        // Các bảng hệ thống phân quyền
        public DbSet<Group> Groups { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Authorize> Authorizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bạn có thể cấu hình thêm fluent API tại đây nếu cần thiết
        }
    }
}