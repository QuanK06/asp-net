using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Interfaces;

[Table("Member")]
public class Member : IAuditable
{
    [Key] public Guid ID { get; set; }
    [Required][MaxLength(100)] public string Name { get; set; }
    [Required][MaxLength(50)] public string LoginName { get; set; }
    [Required] public string Password { get; set; }
    [MaxLength(150)] public string Email { get; set; }
    public DateTime? LastLogin { get; set; }
    [MaxLength(250)] public string Picture { get; set; }
    public Guid GroupID { get; set; }
    [ForeignKey("GroupID")] public virtual Group Group { get; set; }

    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}