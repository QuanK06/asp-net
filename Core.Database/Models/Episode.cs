using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Database.Interfaces;

namespace Database.Models
{
    [Table("Episode")]
    public class Episode : IAuditable
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public int EpisodeNumber { get; set; } // Tập 1, Tập 2...

        [Required]
        public string VideoUrl { get; set; } // Đường dẫn video (m3u8, mp4, iframe...)

        // Mối quan hệ ngoại với bảng Anime
        public Guid AnimeID { get; set; }
        [ForeignKey("AnimeID")]
        public virtual Anime Anime { get; set; }

        //IAuditable
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}