using Microsoft.AspNetCore.Mvc;

namespace NestQuest.Controllers
{
    public class HostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
