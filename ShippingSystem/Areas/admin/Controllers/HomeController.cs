using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LapshopPro.Areas.admin.Controllers
{
 
    [Area("admin")]
    [Authorize(Roles = "Admin,Reviewer,Operation,Operation Manager")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
