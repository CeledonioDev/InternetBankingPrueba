using Core.Models.Entities;
using Core.Ports.Repositories;
using Persistence.Context;

namespace Persistence.Repositories
{
    public sealed class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly InternetBankingContext _context;

        public UserRepository(InternetBankingContext  context) : base(context)
            => _context = context;
    }
}
