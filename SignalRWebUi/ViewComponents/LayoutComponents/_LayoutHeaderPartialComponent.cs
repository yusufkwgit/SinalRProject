using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUi.ViewComponents.LayoutComponent
{
    public class _LayoutHeaderPartialComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
