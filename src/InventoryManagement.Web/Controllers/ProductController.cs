using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers
{
    [Route("create")]
    public class ProductController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }




    }
}
