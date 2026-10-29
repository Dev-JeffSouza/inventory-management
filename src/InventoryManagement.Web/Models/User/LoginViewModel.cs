using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Models.User
{
    public class LoginViewModel
    {
        [Required (ErrorMessage ="O login é obrigatório.")]
        public required string Login { get; set; }

        [Required (ErrorMessage ="A senha é obrigatória.")]
        public required string Password { get; set; }

    }
}
