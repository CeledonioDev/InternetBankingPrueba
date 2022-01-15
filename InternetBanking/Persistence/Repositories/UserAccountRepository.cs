using Core.Models.Entities;
using Core.Ports.Repositories;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class UserAccountRepository : BaseRepository<UserAccount>, IUserAccountRepository
    {
        private readonly InternetBankingContext _context;

        public UserAccountRepository(InternetBankingContext context) : base(context)
            => _context = context;
    }
}
