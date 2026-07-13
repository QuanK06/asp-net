using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Interfaces;

namespace Database.Models
{
    [Table("Anime")]
    public class Anime : IAuditable, IMeta
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Poster { get; set; } // Thay cho thuộc tính Picture của sản phẩm

        [MaxLength(500)]
        public string Intro { get; set; } // Mô tả ngắn

        public string Detail { get; set; } // Mô tả chi tiết/Nội dung phim

        public int ReleaseYear { get; set; } // Năm phát hành
        public int ViewCount { get; set; } // Lượt xem phim

        // Mối quan hệ 1-Nhiều với Thể loại (Category)
        public Guid CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        // Mối quan hệ 1-Nhiều với Tập phim (Episode)
        public virtual ICollection<Episode> Episodes { get; set; }

        // Triển khai IAuditable & IMeta
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
    }
}