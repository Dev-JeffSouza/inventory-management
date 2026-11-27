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

        public static EditViewModel MapToEditViewModel(this LocationResult result)
        {
            return new EditViewModel
            {
                CodLocal = result.CodLocal,
                Description = result.Description,
                Active = result.Active
            };
        }

        public static EditLocationRequest MaptToEditRequest(this EditViewModel request)
        {
            return new EditLocationRequest
            {
                CodLocal = request.CodLocal,
                Description = request.Description,
                Active = request.Active
            };
        }
    }
}
