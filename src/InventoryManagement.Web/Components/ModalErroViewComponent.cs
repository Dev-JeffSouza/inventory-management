using InventoryManagement.Web.Models.ModalErro;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Components
{
    public class ModalErroViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() 
        {
            var modal = new ModalErroViewModel
            {
                Msg = ViewData["Erro"] as string ?? TempData["Erro"] as string
            };

            return View(modal);
        }
    }
}
