using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_Meeting_18.Models.DTOs.UserDTOs
{
    public class EditUserDTO
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; } = default!;

        //[Required]
        //[Display(Name = "Password")]
        //public string Password { get; set; } = default!;

        [Required]
        [Display(Name = "Date of birth")]
        public int YearOfBirth { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;
    }
}