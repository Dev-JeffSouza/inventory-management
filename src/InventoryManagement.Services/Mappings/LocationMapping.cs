using InventoryManagement.Repositories.Entities;
using InventoryManagement.Services.Models.Location;

namespace InventoryManagement.Services.Mappings
{
    public static class LocationMapping
    {
        public static Location MapToLocation(this CreateLocationRequest request)
        {
            return new Location
            {
                Description = request.Description.ToUpper(),
                Active = request.Active
            };
        }

        public static LocationResult MapToLocation(this Location result)
        {
            return new LocationResult
            {
                CodLocal = result.CodLocal,
                Description = result.Description,
                Active = result.Active
            };
        }

        public static Location MapToLocation(this EditLocationRequest request)
        {
            return new Location
            {
                CodLocal = request.CodLocal,
                Description = request.Description.ToUpper(),
                Active = request.Active
            };
        }
    }
}
