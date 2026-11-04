using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Models.Product
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "Descrição obrigatória.")]
        public string? Description { get; set; }


        [Required(ErrorMessage = "A unidade de volume é obrigatório.")]
        public int Codvol { get; set; }


        [Required(ErrorMessage = "A marca é obrigatória.")]
        public string? Mark { get; set; }


        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal? Price { get; set; }


        [Required(ErrorMessage = "O fornecedor é obrigatório.")]
        public int CodParc { get; set; }


        [Required(ErrorMessage = "O custo é obrigatório.")]
        [Range(0, 999999.99, ErrorMessage = "O custo não pode ser negativo.")]
        public decimal? Cost { get; set; }


        [Required(ErrorMessage = "O estoque inicial é obrigatório.")]
        public decimal? Quantity { get; set; }


        [Required(ErrorMessage = "O local de estoque é obrigatório.")]
        public int CodLocal { get; set; }


        [Required(ErrorMessage = "O estoque mínimo é obrigatório.")]
        public decimal? StockMin { get; set; }


        [Required(ErrorMessage = "O estoque máximo é obrigatório.")]
        public decimal? StockMax { get; set; }


        //Preenchimento dos inputs selects com os dados do banco de dados

        public List<SelectListItem>? Volume { get; set; } = [];

        public List<SelectListItem>? Partner { get; set; } = [];

        public List<SelectListItem>? Location { get; set; }
    }
}