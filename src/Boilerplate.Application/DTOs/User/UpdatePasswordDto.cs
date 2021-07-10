using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Application.DTOs.User
{
    public class UpdatePasswordDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
