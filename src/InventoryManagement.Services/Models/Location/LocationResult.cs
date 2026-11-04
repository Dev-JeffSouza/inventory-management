using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Services.Models.Location
{
    public class LocationResult
    {
        public int CodLocal { get; set; }

        public required string Description { get; set; }

        public required string Active { get; set; }
    }
}
