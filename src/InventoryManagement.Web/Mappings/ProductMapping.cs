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
                Price = model.Price,
                CodParc = model.CodParc,
                Cost = model.Cost,
                Quantity = model.Quantity,
                CodLocal = model.CodLocal,
                StockMin = model.StockMin,
                StockMax = model.StockMax
            };
        }

        public static ListViewModel MapToProductListViewModel(this ProductResult model)
        {
            return new ListViewModel
            {
                CodProd = model.CodProd,
                Description = model.Description,
                CodVol = model.CodVol,
                DescrVol = model.DescrVol!,
                Cost = model.Cost,
                Quantity = model.Quantity,
                CodLocal = model.CodLocal,
                Local = model.Local!,
                CodSuplier = model.CodSuplier,
                Supplier = model.Supplier!,
                Mark = model.Mark!,
                Price = model.Price,
                DhCad = model.DhCad,
                DhAlter = model.DhAlter,
                Active = model.Active!
            };
        }

        public static EditProductRequest MapToProductRequest(this EditViewModel model)
        {
            return new EditProductRequest
            {
                CodProd = model.CodProd,
                Description = model.Description,
                CodVol = model.CodVol,
                CodLocal = model.CodLocal,
                Cost = model.Cost,
                Quantity = model.Quantity,
                StockMin = model.StockMin,
                StockMax = model.StockMax,
                CodSupplier = model.CodSupplier,
                Mark = model.Mark,
                Price = model.Price,
                Active = model.Active
            };
        }

        public static EditViewModel MapToEditViewModel(this ProductResult model)
        {
            return new EditViewModel
            {
                CodProd = model.CodProd,
                Description = model.Description,
                Cost = model.Cost,
                Quantity = model.Quantity,
                Mark = model.Mark,
                Price = model.Price,
                Active = model.Active,
                StockMin = model.StockMin,
                StockMax = model.StockMax
            };
        }
    }
}

