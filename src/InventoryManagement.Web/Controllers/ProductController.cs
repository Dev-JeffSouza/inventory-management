using InventoryManagement.Repositories.Entities;
using InventoryManagement.Services;
using InventoryManagement.Web.Mappings;
using InventoryManagement.Web.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Controllers
{
    [Route("product")]
    [Authorize]

    public class ProductController : Controller
    {
        public readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //Criar
        [Route("create")]
        public IActionResult Create()
        {
            LoadSelects();
            return View();
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create (CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {  
                LoadSelects();
                return View(model);
            }

            var result = _productService.Create(model.MapToProduct());

            if (!result.Sucess)
            {
                ViewData["Erro"] = result.ErroMenssage;
                LoadSelects();
                return View(model);
            }

            return RedirectToAction("List", "Product"); 

        }

        //Listar
        [Route("list")]
        public IActionResult List()
        {   
            var listResult = _productService.List();
            var result = listResult.Select(p => p.MapToProductListViewModel()).ToList();
            return View(result);
        }

        //Editar

        [Route("edit/{id}/{idloc}/{idsup}")]
        [HttpGet]
        public IActionResult Edit (int id, int idloc, int idsup)
        {
            var product = _productService.GetById(id, idloc, idsup);

            if (product == null)
            {
                ViewData["Erro"] = "Não foi possível encontrar o produto.";
                return RedirectToAction("List", "Product");
            }

            var model = product!.MapToEditViewModel();
            LoadSelects(Codvol: model.CodVol, CodParc: model.CodSupplier, codLoc: model.CodLocal);
            return View(model);
        }

        [Route("edit/{id}/{idloc}/{idsup}")]
        [HttpPost]
        public IActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {   //implemntar aqui a seleção padrão que vem do banco
                LoadSelects();
                return View(model);
            }

            var request = model.MapToProductRequest();
            var result = _productService.Edit(request);

            if (!result.Sucess)
            {
                ViewData["Erro"] = result.ErroMenssage;
                LoadSelects();
                return View(model);
            }

            return RedirectToAction("List","Product");
        }

        private List<SelectListItem> GetVolume(int? selectedCodvol = null)
        {
            var listResult = _productService.GetVolume();

            var volumes = listResult.Select(v => new SelectListItem
            {
                Value = v.Codvol.ToString(),
                Text = v.Description,
                Selected = v.Codvol == selectedCodvol
            }).ToList();

            return volumes;
        }

        private List<SelectListItem> GetPartner(int? selectedCodParc = null)
        {
            var listResult = _productService.GetPartner();
            var partners = listResult.Select(p => new SelectListItem
            {
                Value = p.CodParc.ToString(),
                Text = p.NameParc,
                Selected = p.CodParc == selectedCodParc
            }).ToList();
            return partners;
        }

        private List<SelectListItem> GetLocation(int? selectCodLoc = null)
        {
            var listResult = _productService.GetLocation();
            var locations = listResult.Select(l => new SelectListItem
            {
                Value = l.CodLocal.ToString(),
                Text = l.Description,
                Selected = l.CodLocal == selectCodLoc
            }).ToList();
            return locations;
        }

        private void LoadSelects (int? Codvol = null, int? CodParc = null, int? codLoc = null)
        {
            ViewBag.Volume = GetVolume(Codvol);
            ViewBag.Partner = GetPartner(CodParc);
            ViewBag.Location = GetLocation(codLoc);
        }


    }
}
