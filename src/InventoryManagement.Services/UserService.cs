using InventoryManagement.Services.Models.User;

namespace InventoryManagement.Services
{
    public interface IUserService
    {
        ValidateLoginResult ValidateLogin(string Login, string Password);
    }

    public class UserService : IUserService
    {   
        


        //Login do usuário

        public ValidateLoginResult ValidateLogin(string Login, string Password)
        {
            var result = new ValidateLoginResult();

            if (string.IsNullOrEmpty(Login))
            {
                result.ErroMenssage = "O login é obrigatório.";
                return result;
            }

            if (string.IsNullOrEmpty(Password))
            {
                result.ErroMenssage = "A senha é obrigatóra.";
                return result;
            }

            var usuario = _userRespository.GetByLogin(Login);

        }


    }
}
