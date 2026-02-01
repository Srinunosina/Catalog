using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api
{
    public class HomeController : Controller
    {
        ILoggerProvider loggerProvider = null!;
        public IActionResult Index()
        {
            return View();
        }
    }
}
