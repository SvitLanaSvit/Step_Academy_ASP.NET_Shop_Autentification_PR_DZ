using System.ComponentModel.DataAnnotations;

namespace ASP_Meeting_18.Data
{
    public class Photo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name of the file must be specified!")]
        public string Filename { get; set; } = default!;

        [Required(ErrorMessage = "The url of the photo must be specified!")]
        [Display(Name = "Photo's url")]
        public string PhotoUrl { get; set; } = default!;

        public int ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
