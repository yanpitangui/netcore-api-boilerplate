using System.ComponentModel.DataAnnotations;
using Boilerplate.Domain.Core.Entities;

namespace Boilerplate.Domain.Entities
{
    public class User : Entity
    {
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}

