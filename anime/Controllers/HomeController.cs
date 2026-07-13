using anime.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace anime.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
