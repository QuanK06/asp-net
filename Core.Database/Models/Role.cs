using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Models;

[Table("Role")]
public class Role
{
    [Key] public Guid ID { get; set; }
    [Required][MaxLength(150)] public string Name { get; set; }
    [Required][MaxLength(150)] public string Code { get; set; }
    public Guid CategoryID { get; set; }
    [ForeignKey("CategoryID")] public virtual Category Category { get; set; }
    public virtual ICollection<Authorize> Authorizes { get; set; }
}