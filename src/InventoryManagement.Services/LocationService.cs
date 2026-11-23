
using InventoryManagement.Repositories;
using InventoryManagement.Services.Mappings;
using InventoryManagement.Services.Models.Location;

namespace InventoryManagement.Services
{   
    public interface ILocationService
    {
        CreateLocationResult Create(CreateLocationRequest request);

        IList<LocationResult>? List();
    }


    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }


        public CreateLocationResult Create(CreateLocationRequest request)
        {
            var result = new CreateLocationResult();

            var affectedRows = _locationRepository.Insert(request.MapToLocation());

            if (affectedRows == 0)
            {
                result.ErroMenssage = "Erro: Não foi possível cadastrar o local de estoque.";
                return result;
            }

            result.Sucess = true;
            return result;
        }

        public IList<LocationResult>? List()
        {
            var locationList = _locationRepository.List();
            var locationResult = locationList?.Select(l => l.MapToLocation()).ToList();
            return locationResult;
        }
    }
}
