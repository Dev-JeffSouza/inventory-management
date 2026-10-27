using InventoryManagement.Repositories;
using InventoryManagement.Services.Models.User;

namespace InventoryManagement.Services
{
    public interface IUserService
    {
        ValidateLoginResult ValidateLogin(string Login, string Password);
    }

    public class UserService : IUserService
    {   
        private readonly IUserRepository _userRepository;

        public UserService (IUserRepository userRepository)
        {
            _userRepository = userRepository;
        } 
        
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
                result.ErroMenssage = "A senha é obrigatória.";
                return result;
            }

            var user = _userRepository.GetByLogin(Login);

            if (user == null)
            {
                result.ErroMenssage = "Usuário ou senha inválidos.";
                return result;
            }

            if (user.Password != Password)
            {
                result.ErroMenssage = "Usuário ou senha inválidos.";
                return result;
            }

            result.Sucess = true;
            return result;
        }


    }
}
