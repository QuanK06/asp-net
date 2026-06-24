using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Interfaces;
using Core.Models;

namespace Core
{
    [Table("Categories")] 
    public class Categories : IAuditable 
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(150)]
        public string? Name { get; set; }
        [ForeignKey("ParentId")]
        public Guid? ParentId { get; set; }
        public Categories? Parent { get; set; } 
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public ICollection<Anime_Details> Anime_Details { get; set; } = new HashSet<Anime_Details>();
        public ICollection<Categories> ChildCategory { get; set; } = new HashSet<Categories>();
        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
    }
}
