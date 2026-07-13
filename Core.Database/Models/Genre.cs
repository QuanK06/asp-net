using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Interfaces;
using Database.Models;

namespace Core.Database.Models
{
    [Table("Category")]
    public class Category: IAuditable
    {
        [Key]
        public Guid? Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string? Name { get; set; }
        [ForeignKey("ParentId")]
        public Guid? ParentId { get; set; }
        public Category? Parent { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public ICollection<Anime> Episodes { get; set; } = new HashSet<Anime>();
        public ICollection<Category> Children { get; set; } = new HashSet<Category>();
        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
    }
}
