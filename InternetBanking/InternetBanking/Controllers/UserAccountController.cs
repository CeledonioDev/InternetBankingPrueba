using Core.Contracts;
using Core.Manager;
using Core.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private UserAccountManager _userAccountManager;
        private readonly ClaimsPrincipal _caller;
        public UserAccountController(UserAccountManager userAccountManager, IHttpContextAccessor httpContextAccessor)
        {
            _userAccountManager = userAccountManager;
            _caller = httpContextAccessor.HttpContext.User;
        }

        [HttpPut("TransferAccount")]
        public async Task<IActionResult> TransferAccount([FromBody] UserAccountDto account)
        {
            int userId = Convert.ToInt32( _caller.Claims?.FirstOrDefault(c => c.Type == "id")?.Value);

            IOperationResult<UserAccountDto> operationResult = await _userAccountManager.TransferAccount(account, userId);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Messages);
            }

            return Ok(operationResult.Entity);
        }
    }
}
