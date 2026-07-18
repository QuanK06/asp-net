namespace AnimeVietsub.Services
{
    public interface IEmailSender
    {
        Task GuiEmailAsync(string diaChiNhan, string tieuDe, string noiDungHtml);
    }
}
