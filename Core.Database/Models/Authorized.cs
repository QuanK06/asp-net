using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Authorize")]
public class Authorize
{
    [Key] public Guid ID { get; set; }
    public Guid GroupID { get; set; }
    [ForeignKey("GroupID")] public virtual Group Group { get; set; }
    public Guid RoleID { get; set; }
    [ForeignKey("RoleID")] public virtual Role Role { get; set; }
}