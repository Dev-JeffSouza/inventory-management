using InventoryManagement.Repositories.Entities;

namespace InventoryManagement.Repositories
{   
    public interface IUserRepository
    {
        User GetById(string Login);
    }


    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository (string connectionString) :base(connectionString)
        {

        }


        public User GetById(string Login)
        {

        }


    }
}
