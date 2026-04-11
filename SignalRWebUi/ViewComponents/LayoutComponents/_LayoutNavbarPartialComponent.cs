using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUi.ViewComponents.LayoutComponent
{
    public class _LayoutNavbarPartialComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
