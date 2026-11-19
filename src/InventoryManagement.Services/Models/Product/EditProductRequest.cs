
namespace InventoryManagement.Services.Models.Product
{
    public class EditProductRequest
    {
        public int CodProd { get; set; }
        public required string Description { get; set; }

        public int CodVol { get; set; }

        public decimal Cost { get; set; }

        public decimal Quantity { get; set; }

        public required decimal StockMin { get; set; }

        public required decimal StockMax { get; set; }

        public int CodLocal { get; set; }

        public int CodSupplier { get; set; }

        public required string Mark { get; set; }

        public required decimal Price { get; set; }

        public required string Active { get; set; }
    }
}
