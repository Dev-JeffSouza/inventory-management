
namespace InventoryManagement.Services.Models.Product
{
    public class CreateProductRequest
    {
        public required string Description { get; set; }

        public int Codvol { get; set; }

        public required string Mark { get; set; }

        public required decimal Price { get; set; }

        public required int CodParc { get; set; }

        public required decimal Cost { get; set; }

        public required decimal Quantity { get; set; }

        public int CodLocal { get; set; }

        public decimal StockMin { get; set; }

        public required decimal StockMax { get; set; }
    }
}
