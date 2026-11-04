using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Repositories.Entities
{
    public class Supplier
    {
        public int CodProd { get; set; }

        public int CodParc { get; set; }

        public required decimal Cost { get; set; }

        public Partner? Partner { get; set; }
    }
}
