using InventoryManagement.Services.Models.Location;
using InventoryManagement.Web.Models.Location;

namespace InventoryManagement.Web.Mappings
{
    public static class LocationMapping
    {
        public static CreateLocationRequest MapToCreateLocation(this CreateViewModel request)
        {
            return new CreateLocationRequest
            {
                Description = request.Description,
                Active = request.Active
            };
        }

        public static ListViewModel MapToListViewModel(this LocationResult result)
        {
            return new ListViewModel
            {
                CodLocal = result.CodLocal,
                Description = result.Description,
                Active = result.Active
            };
        }
    }
}
