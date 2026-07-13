using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Models;

[Table("Group")]
public class Group
{
    [Key] public Guid ID { get; set; }
    [Required][MaxLength(150)] public string Name { get; set; }
    public virtual ICollection<Member> Members { get; set; }
    public virtual ICollection<Authorize> Authorizes { get; set; }
}