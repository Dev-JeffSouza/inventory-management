using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Repositories.Entities
{
    public class Partner
    {
        public int CodParc { get; set; }

        public required string NameParc { get; set; }

        public string? RazaoSocial { get; set; }

        public string? TypePerson { get; set; }

        public string? Phone { get; set; }

        public string? CpfCnpj { get; set; }

        public required string Active { get; set; }

        public required DateTime DhCad { get; set; }

        public DateTime? Dhalter { get; set; }

    }
}
