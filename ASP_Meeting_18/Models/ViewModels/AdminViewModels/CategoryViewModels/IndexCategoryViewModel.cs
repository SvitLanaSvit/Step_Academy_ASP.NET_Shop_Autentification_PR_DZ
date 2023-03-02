using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_Meeting_18.Models.ViewModels.AdminViewModels.CategoryViewModels
{
    public class IndexCategoryViewModel
    {
        public IEnumerable<CategoryDTO> Categories { get; set; } = default!;
        public SelectList? ParentCategorySL { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
