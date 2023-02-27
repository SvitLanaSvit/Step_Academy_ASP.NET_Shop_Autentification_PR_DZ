using ASP_Meeting_18.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ASP_Meeting_18.Models.ViewModels.AdminViewModels.CategoryViewModels
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "The name of the category must be specified!")]
        public string Title { get; set; } = default!;

        public int? ParentCategoryId { get; set; }

        public SelectList? ParentCategory { get; set; } = default!;
    }
}