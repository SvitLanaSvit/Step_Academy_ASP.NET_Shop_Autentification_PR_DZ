using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ASP_Meeting_18.Data
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the category must be specified!")]
        public string Title { get; set; } = default!;

        [ForeignKey(nameof(ParentCategory) )]
        public int? ParentCategoryId { get; set; }

        [Display(Name = "Parent category")]
        public Category? ParentCategory { get; set; }

        public List<Category>? ChildCategoies { get; set; }

        public List<Product>? Products { get; set; }

    }
}
