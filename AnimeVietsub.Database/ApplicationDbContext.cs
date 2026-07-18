using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimeVietsub.Database
{
    // Lab 02: Ket noi voi SQL Server
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<AnimeTheLoai> AnimeTheLoais { get; set; }
        public DbSet<TapPhim> TapPhims { get; set; }
        public DbSet<BinhLuan> BinhLuans { get; set; }
        public DbSet<TinTuc> TinTucs { get; set; }
        public DbSet<TheoDoi> TheoDois { get; set; }
        public DbSet<LichSuXem> LichSuXems { get; set; }
        public DbSet<GopY> GopYs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Khoa chinh gop cho bang trung gian AnimeTheLoai (nhieu-nhieu)
            builder.Entity<AnimeTheLoai>()
                .HasKey(at => new { at.AnimeId, at.TheLoaiId });

            builder.Entity<AnimeTheLoai>()
                .HasOne(at => at.Anime)
                .WithMany(a => a.DanhSachTheLoai)
                .HasForeignKey(at => at.AnimeId);

            builder.Entity<AnimeTheLoai>()
                .HasOne(at => at.TheLoai)
                .WithMany(t => t.DanhSachAnime)
                .HasForeignKey(at => at.TheLoaiId);

            // 1 Anime - nhieu Tap, xoa Anime thi xoa het Tap (Cascade)
            builder.Entity<TapPhim>()
                .HasOne(t => t.Anime)
                .WithMany(a => a.DanhSachTap)
                .HasForeignKey(t => t.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tranh 1 User theo doi trung 1 Anime nhieu lan
            builder.Entity<TheoDoi>()
                .HasIndex(t => new { t.UserId, t.AnimeId })
                .IsUnique();

            // Cac quan he con lai de EF Core tu suy luan (Restrict mac dinh de tranh loi multiple cascade path)
            builder.Entity<BinhLuan>()
                .HasOne(b => b.Anime)
                .WithMany(a => a.DanhSachBinhLuan)
                .HasForeignKey(b => b.AnimeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BinhLuan>()
                .HasOne(b => b.NguoiDung)
                .WithMany(u => u.DanhSachBinhLuan)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TheoDoi>()
                .HasOne(t => t.NguoiDung)
                .WithMany(u => u.DanhSachTheoDoi)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LichSuXem>()
                .HasOne(l => l.NguoiDung)
                .WithMany(u => u.LichSuXem)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LichSuXem>()
                .HasOne(l => l.TapPhim)
                .WithMany(t => t.LichSuXemTap)
                .HasForeignKey(l => l.TapPhimId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
