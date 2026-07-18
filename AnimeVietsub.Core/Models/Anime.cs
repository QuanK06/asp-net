using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeVietsub.Core.Models
{
    // Thay the bang "Product/San pham" ben ban goc -> Lab 11: Chuc nang quan ly san pham
    // Khac biet quan trong: ben goc Product la bang PHANG (ban 1 san pham = 1 dong)
    // Ben nay Anime chi la THONG TIN CHUNG, con tung TAP xem duoc tach rieng ra bang TapPhim (quan he 1-nhieu)
    public class Anime
    {
        [Key]
        public int AnimeId { get; set; }

        [Required(ErrorMessage = "Vui long nhap ten anime")]
        [StringLength(200)]
        public string TenAnime { get; set; }

        [StringLength(200)]
        public string TenKhac { get; set; } // ten goc tieng Nhat / ten tieng Anh

        [StringLength(250)]
        public string SlugUrl { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string MoTa { get; set; }

        public string Poster { get; set; } // duong dan anh poster doc (wwwroot/uploads/posters)
        public string AnhBanner { get; set; } // anh ngang hien thi o trang chu / hero banner (theo template Vivid)

        public int? NamPhatHanh { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; } // "Dang chieu", "Hoan thanh", "Sap chieu"

        [StringLength(50)]
        public string DinhDangPhatSong { get; set; } // TV Series, Movie, OVA, ONA, Special

        public int SoMua { get; set; } = 1; // Season 1, Season 2... (vd: "One Piece" Season 3)

        [StringLength(50)]
        public string PhuongThucDich { get; set; } // Vietsub, Thuyet minh, Long tieng, Vietsub + Thuyet minh

        [StringLength(50)]
        public string QuocGia { get; set; } // Nhat Ban, Trung Quoc...

        [StringLength(100)]
        public string DaoDien { get; set; } // Dao dien

        [StringLength(100)]
        public string Studio { get; set; } // Studio san xuat

        [StringLength(20)]
        public string ChatLuong { get; set; } = "HD"; // HD, FullHD, 4K

        [StringLength(50)]
        public string Rating { get; set; } // vd: "PG-13 - Teens 13 tuoi tro len"

        public int? ThoiLuongPhut { get; set; } // thoi luong 1 tap/phim, tinh bang phut

        public string TrailerUrl { get; set; } // link youtube/embed trailer

        public double DiemDanhGia { get; set; } = 0; // trung binh so sao danh gia
        public int LuotXem { get; set; } = 0; // dung cho Lab 14: thong ke Anime hot

        public bool NoiBat { get; set; } = false; // hien o hero banner trang chu
        public bool AnHien { get; set; } = true;

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<TapPhim> DanhSachTap { get; set; }
        public ICollection<AnimeTheLoai> DanhSachTheLoai { get; set; }
        public ICollection<BinhLuan> DanhSachBinhLuan { get; set; }
        public ICollection<TheoDoi> DuocTheoDoiBoi { get; set; }
    }
}
