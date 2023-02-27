using System.ComponentModel.DataAnnotations;

namespace ASP_Meeting_18.Models.DTOs.UserDTOs
{
    public class CreateUserDTO
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; } = default!;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = default!;

        [Required]
        [Display(Name = "Date of birth")]
        public int YearOfBirth { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;
    }
}
