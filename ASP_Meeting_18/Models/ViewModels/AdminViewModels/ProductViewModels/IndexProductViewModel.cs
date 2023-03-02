using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_Meeting_18.Models.ViewModels.AdminViewModels.ProductViewModels
{
    public class IndexProductViewModel
    {
        public IEnumerable<ProductDTO> Products { get; set; } = default!;
        public SelectList? CategorySL { get; set; } = default!;
        public int? CategoryId { get; set; }
    }
}
