using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUi.Controllers
{

    public class ErrorController : Controller
    {
        public IActionResult NotFound404Page()
        {
            return View();
        }
    }
}
