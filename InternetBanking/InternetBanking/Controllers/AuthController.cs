using Core.Contracts;
using Core.Manager;
using Core.Models;
using Core.Models.Dto;
using Core.Models.Entities;
using Core.Services;
using InternetBanking.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwIssuerOptions _jwtOptions;

        public AuthController(UserManager userManager, IJwtFactory jwtFactory, IOptions<JwIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserResponse credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.AccountNumber, credentials.Password);

            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Numero Cuenta o contraseña invalidos.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.AccountNumber, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string accountNumber, string password)
        {
            if (string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            IOperationResult<User> userToVerify = await _userManager.FindByAccount(accountNumber);

            if (!userToVerify.Success)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }

            if (!userToVerify.Success) return await Task.FromResult<ClaimsIdentity>(null);


            if (userToVerify.Entity.Password == password && userToVerify.Entity.AccountNumber == accountNumber)
            {

                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(accountNumber, userToVerify.Entity.Id.ToString()));
            }


            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
