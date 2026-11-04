using InventoryManagement.Repositories.Entities;
using InventoryManagement.Services.Models.Location;
using InventoryManagement.Services.Models.Partner;
using InventoryManagement.Services.Models.Product;
using InventoryManagement.Services.Models.Volume;

namespace InventoryManagement.Services.Mappings
{
    public static class ProductMapping
    {
        public static Product MapToProduct(this CreateProductRequest model)
        {
            var product = new Product
            {
                Description = model.Description,
                Codvol = model.Codvol,
                Mark = model.Mark,
                Price = model.Price,
                DhCad = DateTime.Now,
                Active = "S",

                Stock = new List<Stock>
                {
                    new Stock

                    {   Codlocal = model.CodLocal,
                        Quantity = model.Quantity,
                        StockMin = model.StockMin,
                        StockMax = model.StockMax,
                        Active = "S"
                    }
                },

                Supplier = new List<Supplier>
                {
                    new Supplier
                    {   
                        CodParc = model.CodParc, 
                        Cost = model.Cost
                    }
                }
            };

            return product;
        }

        public static VolumeResult MapToProductVolumeResult(this Volume model)
        {
            return new VolumeResult
            {
                Codvol = model.Codvol,
                Description = model.DescrVol
            };

        }

        public static PartnerResult MapToProductPartnerResult(this Partner model)
        {
            return new PartnerResult
            {
                CodParc = model.CodParc,
                NameParc = model.NameParc
            };
        }

        public static LocationResult MapToProductLocationResult(this Location model)
        {
            return new LocationResult
            {
                CodLocal = model.CodLocal,
                Description = model.Description,
                Active = model.Active
            };
        }

    }
}