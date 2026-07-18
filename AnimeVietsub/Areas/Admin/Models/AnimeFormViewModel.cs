using AnimeVietsub.Core.Models;
using Microsoft.AspNetCore.Http;

namespace AnimeVietsub.Areas.Admin.Models
{
    // Lab 11: ViewModel dung cho form Them/Sua Anime (co upload file nen khong dung thang Model)
    public class AnimeFormViewModel
    {
        public Anime Anime { get; set; } = new Anime();
        public List<TheLoai> TatCaTheLoai { get; set; } = new List<TheLoai>();
        public List<int> TheLoaiDaChon { get; set; } = new List<int>();

        public IFormFile FilePoster { get; set; }
        public IFormFile FileBanner { get; set; }
    }
}
