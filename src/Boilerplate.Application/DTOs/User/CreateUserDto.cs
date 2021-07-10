using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Application.DTOs.User
{
    public class CreateUserDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}
