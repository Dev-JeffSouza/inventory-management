

namespace InventoryManagement.Repositories.Entities
{
    public class Stock
    {
        public int Codprod { get; set; }

        public int Codlocal { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal StockMin { get; set; }

        public required decimal StockMax { get; set; }

        public required string Active { get; set; }

        public Location? Location { get; set; }
    }

}