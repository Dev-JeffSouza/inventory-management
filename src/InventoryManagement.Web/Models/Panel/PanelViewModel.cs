namespace InventoryManagement.Web.Models.Panel
{
    public class PanelViewModel
    {
        public Panel Active { get; set; }
    }

    public enum Panel
    {
        Home,
        Produtos,
        Locais,
        Parceiros,
        Transferências

    }
}
