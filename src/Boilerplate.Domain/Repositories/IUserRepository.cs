using System.Threading.Tasks;
using Boilerplate.Domain.Core.Interfaces;
using Boilerplate.Domain.Entities;

namespace Boilerplate.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
