using InventoryManagement.Web.Models.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Controllers
{
    [Route("location")]
    [Authorize]

    public class LocationController : Controller
    {

        [Route("create")]
        public IActionResult Create()
        {
            LoadSelect();
            return View();
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            return View();
        }

        private List<SelectListItem> ListItens()
        {
            return new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "S",
                    Text = "Sim",
                    Selected = true
                },

                new SelectListItem
                {
                    Value = "N",
                    Text = "Não",
                    Selected = false
                }
            };
        }

        private void LoadSelect()
        {
            ViewBag.Active = ListItens();
        }

    }
}
