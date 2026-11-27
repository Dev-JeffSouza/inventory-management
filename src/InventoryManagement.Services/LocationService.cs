
using InventoryManagement.Repositories;
using InventoryManagement.Services.Mappings;
using InventoryManagement.Services.Models.Location;

namespace InventoryManagement.Services
{
    public interface ILocationService
    {
        CreateLocationResult Create(CreateLocationRequest request);

        EditLocationResult Update(EditLocationRequest request);

        DeleteLocationResult Delete(int id);

        LocationResult? GetById(int id);

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

        public EditLocationResult Update(EditLocationRequest request)
        {
            var result = new EditLocationResult();

            var exists = _locationRepository.GetById(request.CodLocal);

            if (exists == null)
            {
                result.ErroMenssage = "Produto não econtrado na base de dados.";
                return result;
            }
            ;

            var affectedRows = _locationRepository.Update(request.MapToLocation());

            if (affectedRows == 0 || affectedRows == null)
            {
                result.ErroMenssage = "Não foi possível atualizar o produto. Tente novamente mais tarde.";
            }
            ;

            result.Sucess = true;
            return result;
        }

        public DeleteLocationResult Delete(int id)
        {
            var result = new DeleteLocationResult();

            var exists = _locationRepository.GetById(id);

            if (exists == null)
            {
                result.ErroMenssage = "Produto não encontrado na base de dados.";
                return result;
            }

            var constr = _locationRepository.GetConstraints(id);

            if (constr > 0)
            {
                result.ErroMenssage = "Não é possível excluir este local porque ele possui vínculos de estoque com produtos.";
                return result;
            }

            var affectedRows = _locationRepository.Delete(id);

            if (affectedRows == 0)
            {
                result.ErroMenssage = "Não foi possível excluir o local de estoque. Tente novamente mais tarde.";
                return result;
            }

            result.Sucess = true;
            return result;
        }

        public LocationResult? GetById(int id)
        {
            var location = _locationRepository.GetById(id);
            var result = location?.MapToProductLocationResult();

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
