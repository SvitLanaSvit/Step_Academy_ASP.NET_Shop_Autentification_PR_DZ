using ASP_Meeting_18.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_Meeting_18.Models.ViewModels.AdminViewModels.ProductViewModels
{
    public class IndexProductViewModel
    {
        public IEnumerable<Product> Products { get; set; } = default!;
        public SelectList? CategorySL { get; set; } = default!;
        public int? CategoryId { get; set; }
    }
}
