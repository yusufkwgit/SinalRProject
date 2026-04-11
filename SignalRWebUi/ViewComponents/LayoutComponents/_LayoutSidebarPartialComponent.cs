using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUi.ViewComponents.LayoutComponents
{
    public class _LayoutSidebarPartialComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
