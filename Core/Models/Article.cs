using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Models
{
    [Table("Article")]

    public class Article : IAuditable , IMetas
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
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? KeyWord { get; set; }
        public string? Des { get; set; }

    }
}
 