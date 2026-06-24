using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Code { get; set; }
        [ForeignKey("CategoriesID")]
        public Guid? CategoriesID { get; set; }
        public Categories? Categories { get; set; }
        public ICollection<Authorized> authorizeds { get; set; } = new HashSet<Authorized>();

    }
}
