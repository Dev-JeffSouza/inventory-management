
namespace InventoryManagement.Repositories.Entities
{
    public class User
    {
        public int CodUsu { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Ativo { get; set; }
        public int Login { get; set; }
    }
}
