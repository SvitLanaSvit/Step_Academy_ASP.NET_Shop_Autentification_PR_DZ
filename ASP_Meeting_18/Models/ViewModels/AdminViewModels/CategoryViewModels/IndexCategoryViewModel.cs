using ASP_Meeting_18.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_Meeting_18.Models.ViewModels.AdminViewModels.CategoryViewModels
{
    public class IndexCategoryViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = default!;
        public SelectList? ParentCategorySL { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
