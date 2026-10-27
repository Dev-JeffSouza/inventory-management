using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Services.Models
{
    public class BaseResult
    {
        public bool Sucess { get; set; }
        public string? ErroMenssage{ get; set; }
    }
}
