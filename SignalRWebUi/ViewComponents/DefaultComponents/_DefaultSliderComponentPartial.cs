using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SignalRWebUi.ViewComponents.DefaultComponents
{
    public class _DefaultSliderComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();  
        }
    }
}
