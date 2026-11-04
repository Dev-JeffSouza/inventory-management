using InventoryManagement.Repositories;
using InventoryManagement.Services.Mappings;
using InventoryManagement.Services.Models.Location;
using InventoryManagement.Services.Models.Partner;
using InventoryManagement.Services.Models.Product;
using InventoryManagement.Services.Models.Volume;

namespace InventoryManagement.Services
{
    public interface IProductService
    {
        CreateProductResult Create(CreateProductRequest request);
        IList<VolumeResult> GetVolume();
        IList<PartnerResult> GetPartner();
        IList<LocationResult> GetLocation();
    }

    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //Método para criar produto

        public CreateProductResult Create(CreateProductRequest request)
        {
            var result = new CreateProductResult();

            var affectedRows = _productRepository.Insert(request.MapToProduct());

            if (affectedRows == 0 || affectedRows == null)
            {
                result.ErroMenssage = "Erro ao criar o produto.";
                return result;
            }

            result.Sucess = true;
            return result;
        }

        //Obtendo a lista de volumes do banco de dados

        public IList<VolumeResult> GetVolume()
        {
            var volumeList = _productRepository.GetVolume();
            var listResult = volumeList.Select(v => v.MapToProductVolumeResult()).ToList();

            return listResult;
        }

        //Obtendo a lista de parceiros do banco de dados

        public IList<PartnerResult> GetPartner()
        {
            var partnerList = _productRepository.GetPartner();
            var listResult = partnerList.Select(p => p.MapToProductPartnerResult()).ToList();

            return listResult;
        }

        //Obtendo a lista de locais do banco de dados

        public IList<LocationResult> GetLocation()
        {
            var locationList = _productRepository.GetLocation();
            var listResult = locationList.Select(l => l.MapToProductLocationResult()).ToList();
            return listResult;
        }

    };

}
