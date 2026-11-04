
namespace InventoryManagement.Repositories.Entities
{
    public class Product
    {
        public int CodProd { get; set; }

        public required string Description { get; set; }

        public int Codvol { get; set; }

        public required string Mark { get; set; }

        public required decimal Price { get; set; }

        public required string Active { get; set; }

        public required DateTime DhCad { get; set; }

        public DateTime? Dhalter { get; set; }

        //Relacionamentos

        public Volume? volume { get; set; } 

        public ICollection<Stock>? Stock { get; set; } = [];

        public ICollection<Supplier>? Supplier { get; set; } = [];

    }
}
