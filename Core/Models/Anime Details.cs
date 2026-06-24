using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Anime_Details")]
    public class Anime_Details
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string? Title { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Picture { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? View { get; set; } = "";
        [ForeignKey("CategoriesId")]
        public Guid? CategoriesId { get; set; }
        public Categories? Categories { get; set; }
    }
}
