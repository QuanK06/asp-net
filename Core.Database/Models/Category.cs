using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Database.Models;
using Database.Interfaces;

namespace Database.Models
{
    [Table("Category")]
    public class Category : IAuditable
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public Guid? ParentID { get; set; }

        [ForeignKey("ParentID")]
        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<Anime> Animes { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        //IAuditable
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}