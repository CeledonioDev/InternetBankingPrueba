using Core.Contracts;
using Core.Manager;
using Core.Models.Dto;
using Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternetBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager _userManager;

        public UserController(UserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("user-registration")]
        public async Task<IActionResult> RegistrationUser([FromBody] UserDto user)
        {
            IOperationResult<UserDto> operationResult = await _userManager.CreationUser(user);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Messages);
            }

            return Ok(operationResult.Entity);
        }

        [HttpGet("GetDataUserAccount/{accountNumber}")]
        public async Task<IActionResult> GetDataUserAccount(string accountNumber)
        {
            IOperationResult<UserDto> operationResult = await _userManager.GetDataAccount(accountNumber);

            if (!operationResult.Success)
            {
                return BadRequest(operationResult.Messages);
            }

            return Ok(operationResult.Entity);
        }
    }
}
