using InventoryManagement.Services.Models.Product;
using InventoryManagement.Web.Models.Product;

namespace InventoryManagement.Web.Mappings
{
    public static class ProductMapping
    {
        public static CreateProductRequest MapToProduct (this CreateViewModel model)
        {
            return new CreateProductRequest
            {
                Description = model.Description!,
                Codvol = model.Codvol,
                Mark = model.Mark!,
                Price = (decimal)model.Price!,
                CodParc = model.CodParc,
                Cost = (decimal)model.Cost!,
                Quantity = (decimal)model.Quantity!,
                CodLocal = model.CodLocal,
                StockMin = (decimal)model.StockMin!,
                StockMax = (decimal)model.StockMax!
            };
        }
    }
}

