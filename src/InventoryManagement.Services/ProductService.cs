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

        EditProductResult Edit(EditProductRequest request);

        DeleteProductResult Delete(int codProd, int codLocal, int codSupplier);

        ProductResult? GetById(int codProd, int idLoc, int idForn);

        IList<ProductResult> List();

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

        public CreateProductResult Create(CreateProductRequest request)
        {
            var result = new CreateProductResult();

            var locActive = _productRepository.GetLocation(request.CodLocal);
            bool isActive = locActive.Any(l => l.Active == "S");

            if (!isActive)
            {
                result.ErroMenssage = "Não foi possível salvar o produto no local de estoque indicado, pois ele está inativo. Reative o local na aba *Locais*.";
                return result;
            }
           
            var affectedRows = _productRepository.Insert(request.MapToProduct());

            if (affectedRows == 0 || affectedRows == null)
            {
                result.ErroMenssage = "Erro ao criar o produto.";
                return result;
            }

            result.Sucess = true;
            return result;
        }

        public EditProductResult Edit(EditProductRequest request)
        {
            var result = new EditProductResult();
            
            //OBS: Desncessário na Action Edit, pois no caso de um update poderá retornar null por não existir correspondência no banco (criei fallbacks no repositótio).
            var existingProduct = _productRepository.GetById(request.CodProd, request.CodLocal, request.CodSupplier);

            if (existingProduct == null)
            {
                result.ErroMenssage = "Produto não encontrado na base de dados.";
                return result;
            }

            var affectedRows = _productRepository.Update(request.MapToProduct());
            if (affectedRows == 0)
            {
                result.ErroMenssage = "Erro ao editar o produto.";
                return result;
            }

            result.Sucess = true;
            return result;
        }
        //Refatorar depois
        public DeleteProductResult Delete(int codProd, int codLocal, int codSupplier)
        {
            var result = new DeleteProductResult();
            
            var exists = _productRepository.GetById(codProd: codProd, idLoc: codLocal, idForn: codSupplier);

            if(exists == null)
            {
                result.ErroMenssage = "Produto não econtrado.";
                return result;
            }

            var affectedRows = _productRepository.Delete(codProd: codProd, codLocal: codLocal, codSupplier: codSupplier) ?? 0;
            if (affectedRows == -2292)
            {
                result.ErroMenssage = "O produto selecionado possui vinculos ativos com outros locais de estoque ou fornecedores. Realize o procedimento de transferência entre locais para efetuar a exclusão definitiva do produto.";
                return result;
            }

            else if (affectedRows == 0)
            {
                result.ErroMenssage = "Não foi possível concluir a exclusão do produto";
                return result;
            }

            result.Sucess = true;
            return result;
        }

        public IList<VolumeResult> GetVolume()
        {
            var volumeList = _productRepository.GetVolume();
            var listResult = volumeList.Select(v => v.MapToProductVolumeResult()).ToList();

            return listResult;
        }

        public IList<PartnerResult> GetPartner()
        {
            var partnerList = _productRepository.GetPartner();
            var listResult = partnerList.Select(p => p.MapToProductPartnerResult()).ToList();

            return listResult;
        }

        public IList<LocationResult> GetLocation()
        {
            var locationList = _productRepository.GetLocation();
            var listResult = locationList.Select(l => l.MapToProductLocationResult()).ToList();
            return listResult;
        }

        public IList<ProductResult> List()
        {
            var productList = _productRepository.List();
            var listResult = productList.MapToListProductResult();
            return listResult;
        }

        public ProductResult? GetById(int codProd, int idloc, int idsup)
        {
            var product = _productRepository.GetById(codProd, idloc, idsup);

            if (product == null)
            {
                return null;
            }

            var result = product.MapToProductResult();
            return result;
        }

      
    };

}
