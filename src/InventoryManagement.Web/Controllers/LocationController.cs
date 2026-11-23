using InventoryManagement.Services;
using InventoryManagement.Web.Mappings;
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
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }


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
            if (!ModelState.IsValid)
            {
                LoadSelect();
                return View(model);
            }

            var result = _locationService.Create(model.MapToCreateLocation());

            if (!result.Sucess)
            {
                ViewData["Erro"] = result.ErroMenssage;
                return View(model);
            }

            return RedirectToAction("List","Location");
        }

        [Route("list")]
        public IActionResult List()
        {
            var listRequest = _locationService.List();
            var listResult = listRequest?.Select(l => l.MapToListViewModel()).ToList();
            return View(listResult);
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
