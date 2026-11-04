using InventoryManagement.Services;
using InventoryManagement.Web.Mappings;
using InventoryManagement.Web.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        public readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [Route("create")]
        public IActionResult Create()
        {
            var model = new CreateViewModel
            {
                Volume = GetVolume(),
                Partner = GetPartner(),
                Location = GetLocation()
            };

            return View(model);
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create (CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _productService.Create(model.MapToProduct());

            if (!result.Sucess)
            {
                ModelState.AddModelError(string.Empty, result.ErroMenssage!);
                return View(model);
            }

            return RedirectToAction("Index", "Home"); //PROVISÓRIO, AINDA NÃO EXISTE UMA PÁGINA DE LISTAGEM DE PRODUTOS

        }

        private List<SelectListItem> GetVolume()
        {
            var listResult = _productService.GetVolume();

            var volumes = listResult.Select(v => new SelectListItem
            {
                Value = v.Codvol.ToString(),
                Text = v.Description
            }).ToList();

            return volumes;
        }

        private List<SelectListItem> GetPartner()
        {
            var listResult = _productService.GetPartner();
            var partners = listResult.Select(p => new SelectListItem
            {
                Value = p.CodParc.ToString(),
                Text = p.NameParc
            }).ToList();
            return partners;
        }

        private List<SelectListItem> GetLocation()
        {
            var listResult = _productService.GetLocation();
            var locations = listResult.Select(l => new SelectListItem
            {
                Value = l.CodLocal.ToString(),
                Text = l.Description
            }).ToList();
            return locations;
        }


    }
}
