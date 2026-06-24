using Core.Models;
using Microsoft.EntityFrameworkCore;



namespace web.Models.EF
{
    public class AnimeContext : DbContext
    {
        public AnimeContext(DbContextOptions<AnimeContext> options) : base(options) { }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Authorized> Authorizeds { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Anime_Details> Anime_Details { get; set; }
    }
}
