
namespace InventoryManagement.Services.Models.Location
{
    public class EditLocationRequest
    {
        public int CodLocal { get; set; }

        public required string Description { get; set; }

        public required string Active { get; set; }
    }
}
