using InventoryManagement.Web.Models.Panel;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Components
{
    public class PanelViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var panel = new PanelViewModel
            {
                Active = ViewData["Panel"] as Panel? ?? Panel.Home
            };

            return View(panel);
        }
    }
}
