using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUi.Controllers
{
    public class UILayout : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
