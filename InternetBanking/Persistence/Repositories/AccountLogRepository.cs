using Core.Models.Entities;
using Core.Ports.Repositories;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class AccountLogRepository : BaseRepository<AccountLog>, IAccountLogRepository
    {
        private readonly InternetBankingContext _context;

        public AccountLogRepository(InternetBankingContext context) : base(context)
            => _context = context;
    }
}
