using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Repositories.Entities
{
    public class Location
    {
        public int CodLocal { get; set; }

        public required string Description { get; set; }

        public required string Active { get; set; }

    }
}
