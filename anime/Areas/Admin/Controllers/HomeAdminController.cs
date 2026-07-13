using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
