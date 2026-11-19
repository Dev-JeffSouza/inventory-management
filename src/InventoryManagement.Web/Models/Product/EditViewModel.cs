using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Models.Product
{
    public class EditViewModel
    {
        public int CodProd { get; set; }

        [Required(ErrorMessage = "O campo descrição é obrigatório.")]
        public required string Description { get; set; }


        [Required(ErrorMessage = "O campo volume é obrigatório.")]
        public int CodVol { get; set; }


        [Required(ErrorMessage = "O fornecedor é obrigatório.")]
        public int CodSupplier { get; set; }


        [Required(ErrorMessage = "O campo custo é obrigatório.")]
        public decimal Cost { get; set; }


        [Required(ErrorMessage = "O campo volume é obrigatório.")]
        public int CodLocal { get; set; }


        [Required(ErrorMessage = "O campo quantidade é obrigatório.")]
        public decimal Quantity { get; set; }


        [Required(ErrorMessage = "O campo estoque mínimo é obrigatório.")]
        public decimal StockMin { get; set; }


        [Required(ErrorMessage = "O campo estoque máximo é obrigatório.")]
        public decimal StockMax { get; set; }


        [Required(ErrorMessage = "O campo marca é obrigatório.")]
        public required string Mark { get; set; }


        [Required(ErrorMessage = "O campo preço é obrigatório.")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "O campo ativo é obrigatório.")]
        public required string Active { get; set; }


        /*public List<SelectListItem>? Volume { get; set; }

        public List<SelectListItem>? Partner { get; set; }

        public List<SelectListItem>? Location { get; set; }*/

    }
}
