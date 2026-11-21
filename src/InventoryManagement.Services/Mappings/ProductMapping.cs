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

        public static IList<ProductResult> MapToListProductResult(this IEnumerable<Product> products)
        {
            return (from model in products

                    let stocks = model.Stock?.Any() == true ? model.Stock : [new Stock { Quantity = 0, Active = "N", StockMax = 0 , StockMin = 0 }]
                    let suppliers = model.Supplier?.Any() == true ? model.Supplier : [new Supplier { Cost = 0 }]

                    from stock in stocks
                    from supplier in suppliers
                    select new ProductResult
                    {
                        CodProd = model.CodProd,
                        Description = model.Description,
                        CodVol = model.Codvol,
                        DescrVol = model.Volume!.DescrVol, 
                        Cost = supplier.Cost,
                        Quantity = stock.Quantity,
                        StockMin = stock.StockMin,
                        StockMax = stock.StockMax,
                        CodLocal = stock.Codlocal,
                        Local = stock.Location!.Description,
                        Supplier = supplier.Partner!.NameParc,
                        CodSuplier = supplier.CodParc,
                        Mark = model.Mark,
                        Price = model.Price,
                        DhCad = model.DhCad,
                        DhAlter = model.Dhalter ?? null, 
                        Active = model.Active
                    }).ToList();
        }

        public static Product MapToProduct(this EditProductRequest model)
        {
            var product = new Product
            {
                CodProd = model.CodProd,
                Description = model.Description,
                Codvol = model.CodVol,
                Mark = model.Mark,
                Price = model.Price,
                Active = model.Active,
                DhCad = DateTime.Now, 

                Stock = new List<Stock>
                {
                    new Stock
                    {   
                        Codprod = model.CodProd,
                        Codlocal = model.CodLocal,
                        Quantity = model.Quantity,
                        StockMin = model.StockMin,
                        StockMax = model.StockMax,
                        Active = 'S'.ToString() //Depois rever essa linha, pra fazer sentido dentro da lógica de transferência de locais.
                    }
                },

                Supplier = new List<Supplier>
                {
                    new Supplier

                    {   CodProd = model.CodProd,
                        CodParc = model.CodSupplier,
                        Cost = model.Cost
                    }
                }
            };
            return product;
        }

        public static ProductResult MapToProductResult(this Product product)
        {
            return new ProductResult
            {
                CodProd = product.CodProd,
                Description = product.Description,
                DescrVol = product.Volume?.DescrVol,
                CodVol = product.Codvol,
                CodLocal = product.Stock!.FirstOrDefault()!.Codlocal,
                CodSuplier = product.Supplier!.FirstOrDefault()!.CodParc,
                Local = product.Stock?.FirstOrDefault()?.Location?.Description,
                DhCad = product.DhCad,
                Mark = product.Mark,
                Active = product.Active,
                Price = product.Price,
                Quantity = product.Stock?.FirstOrDefault()?.Quantity ?? 0,
                StockMin = product.Stock?.FirstOrDefault()?.StockMin ?? 0,
                StockMax = product.Stock?.FirstOrDefault()?.StockMax ?? 0,
                Supplier = null,
                Cost = product.Supplier?.FirstOrDefault()?.Cost ?? 0
            };
        }
    }
}