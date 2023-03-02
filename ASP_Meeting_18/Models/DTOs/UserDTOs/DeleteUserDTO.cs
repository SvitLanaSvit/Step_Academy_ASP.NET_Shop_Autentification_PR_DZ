using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ASP_Meeting_18.Models.DTOs.UserDTOs
{
    public class DeleteUserDTO
    {
        [Display(Name = "Id")]
        public string Id { get; set; } = default!;

        [Display(Name = "Login")]
        public string Login { get; set; } = default!;

        [Display(Name = "Email")]
        public string Email { get; set; } = default!;

        [Display(Name = "Year of birth")]
        public int YearOfBirth { get; set; }
    }
}
