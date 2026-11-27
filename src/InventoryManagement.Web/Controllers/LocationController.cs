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
            return RedirectToAction("List", "Location");
        }

        [Route("list")]
        public IActionResult List()
        {
            var listRequest = _locationService.List();
            var listResult = listRequest?.Select(l => l.MapToListViewModel()).ToList();
        

            return View(listResult);
        }

        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var location = _locationService.GetById(id);
            var model = location?.MapToEditViewModel();
            LoadSelect(model?.Active);

            return View(model);
        }

        [Route("edit/{id}")]
        [HttpPost]
        public IActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _locationService.Update(model.MaptToEditRequest());

            if (!result.Sucess)
            {
                ViewData["Erro"] = result.ErroMenssage;
                LoadSelect(model?.Active);
                return View(model);
            }
            return RedirectToAction("List", "Location");
        }

        [Route("delete/{id}")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _locationService.Delete(id);

            if (!result.Sucess)
            {
                ViewData["Erro"] = result.ErroMenssage;
                return RedirectToAction("Edit", "Location");
            }

            return RedirectToAction("List", "Location");
        }

        private List<SelectListItem> ListItens(string? selected = null)
        {
            var defaultSlc = selected;
            defaultSlc ??= "S";

            return new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "S",
                    Text = "Sim",
                    Selected = defaultSlc == "S"
                },

                new SelectListItem
                {
                    Value = "N",
                    Text = "Não",
                    Selected = defaultSlc == "N"
                }
            };
        }

        private void LoadSelect(string? selected = null)
        {
            ViewBag.Active = ListItens(selected);
        }

    }
}
