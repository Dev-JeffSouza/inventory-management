namespace InventoryManagement.Web.Models.Location
{
    public class EditViewModel
    {
        public int CodLocal { get; set; }

        public required string Description { get; set; }

        public required string Active { get; set; }
    }
}
