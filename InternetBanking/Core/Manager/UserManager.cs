using Core.Contracts;
using Core.Models;
using Core.Models.Dto;
using Core.Models.Entities;
using Core.Ports.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Manager
{
    public sealed class UserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        public UserManager(IUserRepository userRepository, IUserAccountRepository userAccountRepository)
        {
            _userRepository = userRepository;
            _userAccountRepository = userAccountRepository;
        }

        public async Task<IOperationResult<UserDto>> CreationUser(UserDto user)
        {
            try
            {

                var findEmail = await _userRepository.FindAllAsync(x => x.Email == user.Email);

                if (findEmail.Count() > 0)
                {
                    return BasicOperationResult<UserDto>.Fail("Este usuario ya existe");
                }

                Random rnd = new Random();
                User userModel = new User
                {
                    AccountNumber = rnd.Next(10000000, 99999999).ToString(),
                    Email = user.Email,
                    Name = user.Name,
                    LastName = user.LastName,
                    CreationDate = DateTime.Now,
                    Password = user.Password
                };

                var saveData = await _userRepository.CreateAsync(userModel);
                await _userRepository.SaveAsync();

                var getUser = await _userRepository.FindAsync(x => x.AccountNumber == saveData.Entity.AccountNumber);

                UserAccount userAccount = new UserAccount
                {
                    UserId = getUser.Id,
                    AmountAccount = 0,
                    CreationDate = DateTime.Now
                };
                var saveAccountData = await _userAccountRepository.CreateAsync(userAccount);
                await _userAccountRepository.SaveAsync();

                return BasicOperationResult<UserDto>.Ok(user);
            }
            catch
            {
                return BasicOperationResult<UserDto>.Fail("Ocurrió un error al intentar registrar un usuario..");
            }
        }

        public async Task<IOperationResult<User>> FindByAccount(string accountNumber)
        {
            try
            {
                User result = await _userRepository.FindAsync(user => user.AccountNumber == accountNumber);

                if (result == null)
                {
                    return BasicOperationResult<User>.Fail("usuario o password incorrectos..");
                }

                return BasicOperationResult<User>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<User>.Fail(ex.Message);
            }
        }

        public async Task<IOperationResult<UserDto>> GetDataAccount(string accountNumber)
        {
            try
            {
                User result = await _userRepository.FindAsync(user => user.AccountNumber == accountNumber);

                if (result == null)
                {
                    return BasicOperationResult<UserDto>.Fail("Esta cuenta no existe, por favor ingresar una válida..");
                }

                UserDto UserMapped = GetUserMapped(result);

                return BasicOperationResult<UserDto>.Ok(UserMapped);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<UserDto>.Fail(ex.Message);
            }
        }

        private UserDto GetUserMapped(User result)
            => new UserDto
            {
                Id = result.Id,
                FullName = $"{result.Name} {result.LastName}",
                Email = result.Email,
            };
    }
}
