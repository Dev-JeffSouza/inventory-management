using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.Models.Location
{
    public class CreateViewModel
    {
        public required string Description { get; set; }

        public required string Active { get; set; }
    }
}
