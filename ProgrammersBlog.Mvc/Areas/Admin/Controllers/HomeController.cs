using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
