using Boilerplate.Application.DTOs.Auth;
using Boilerplate.Domain.Entities;

namespace Boilerplate.Application.Interfaces
{
    public interface IAuthService
    {
        JwtDto GenerateToken(User user);
    }
}
