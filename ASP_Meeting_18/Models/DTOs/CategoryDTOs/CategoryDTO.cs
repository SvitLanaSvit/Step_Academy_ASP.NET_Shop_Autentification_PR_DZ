using ASP_Meeting_18.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_Meeting_18.Models.DTOs.CategoryDTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the category must be specified!")]
        public string Title { get; set; } = default!;

        [ForeignKey(nameof(ParentCategory))]
        public int? ParentCategoryId { get; set; }

        [Display(Name = "Parent category")]
        public Category? ParentCategory { get; set; }

        public List<Category>? ChildCategoies { get; set; }

        public List<Product>? Products { get; set; }
    }
}
