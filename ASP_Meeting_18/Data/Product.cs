using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_Meeting_18.Data
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the product must be specified!")]
        public string Title { get; set; } = default!;

        [Required(ErrorMessage = "The price of the product must be specified!")]
        public double Price { get; set; }

        [Required(ErrorMessage = "The count of the product must be specified!")]
        public int Count { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public List<Photo>? Photos { get; set; }
    }
}