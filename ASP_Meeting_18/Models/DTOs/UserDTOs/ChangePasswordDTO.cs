
using System.ComponentModel.DataAnnotations;

namespace ASP_Meeting_18.Models.DTOs.UserDTOs
{
    public class ChangePasswordDTO
    {
        public string Id { get; set; } = default!;

        public string Email { get; set; } = default!;

        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = default!;
    }
}