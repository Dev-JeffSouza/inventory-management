using InventoryManagement.Services;
using InventoryManagement.Web.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController (IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _userService.ValidateLogin(model.Login, model.Password);

        }
    }
}
