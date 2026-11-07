

namespace InventoryManagement.Web.Models.Product
{
    public class ListViewModel
    {
        public int CodProd { get; set; }
        public required string Description { get; set; }

        public required string DescrVol { get; set; }

        public required decimal Cost { get; set; }

        public required decimal Quantity { get; set; }

        public required string Local { get; set; }

        public required string Supplier { get; set; }

        public required string Mark { get; set; }

        public required decimal Price { get; set; }

        public required DateTime DhCad { get; set; }

        public DateTime? DhAlter { get; set; }

        public required string Active { get; set; }

    }
}
