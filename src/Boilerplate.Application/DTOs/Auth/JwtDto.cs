using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.DTOs.Auth
{
    public class JwtDto
    {
        public string Token { get; set; }
        public DateTime ExpDate { get; set; }
    }
}
