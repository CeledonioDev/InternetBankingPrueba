using Core.Contracts;
using Core.Models;
using Core.Models.Dto;
using Core.Models.Entities;
using Core.Ports.Repositories;
using System;
using System.Threading.Tasks;

namespace Core.Manager
{
    public sealed class UserAccountManager
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IAccountLogRepository _accountLogRepository;
        public UserAccountManager(IUserAccountRepository userAccountRepository, IAccountLogRepository accountLogRepository)
        {
            _userAccountRepository = userAccountRepository;
            _accountLogRepository = accountLogRepository;
        }

        public async Task<IOperationResult<UserAccountDto>> TransferAccount(UserAccountDto account, int userId)
        {
            try
            {
                var VerifyAmountAccount = await _userAccountRepository.FindAsync(x => x.UserId == userId, x => x.User);


                if (VerifyAmountAccount.AmountAccount == 0 || VerifyAmountAccount.AmountAccount < account.AmountAccount)
                {
                    return BasicOperationResult<UserAccountDto>.Fail("Su cuenta no contiene suficente saldo para la transacción");
                }

                UserAccount userAccount = await _userAccountRepository.FindAsync(x => x.User.AccountNumber == account.AccountNumber,x => x.User);

                userAccount.AmountAccount = userAccount.AmountAccount + account.AmountAccount;

                _userAccountRepository.Update(userAccount);
                await _userAccountRepository.SaveAsync();

                AccountLog accountLog = new AccountLog
                {
                    AccountPrimary = VerifyAmountAccount.User.AccountNumber,
                    AccountSecundary = userAccount.User.AccountNumber,
                    AmountAccountPrimary = userAccount.AmountAccount,
                    AmountAccountSecundary = userAccount.AmountAccount,
                    CreationDate = DateTime.Now
                };

                await _accountLogRepository.CreateAsync(accountLog);
                await _accountLogRepository.SaveAsync();

                return BasicOperationResult<UserAccountDto>.Ok(account);
            }
            catch
            {
                return BasicOperationResult<UserAccountDto>.Fail("Ocurrió un error al realizar su transacción, por favor intentar más tarde");
            }
        }
    }
}
