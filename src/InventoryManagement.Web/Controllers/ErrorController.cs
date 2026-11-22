using InventoryManagement.Web.Models.Erro;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var model = new ErrorViewModel
            {
                ErroMessage = exceptionHandlerFeature == null ? "Erro inesperado" : exceptionHandlerFeature.Error.Message
            };

            return View(model);
        }
    }
}
