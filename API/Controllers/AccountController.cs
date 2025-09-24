using Application.Account.DTOs;
using Application.Account.DTOs.Application.Account;
using Application.Account.Services;
using Core.Sharing.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountAppService;

        public AccountController(IAccountService accountAppService)
        {
            _accountAppService = accountAppService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthModel>> Register([FromBody] RegisterModel model)
        {
            var result = await _accountAppService.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthModel>> Login([FromBody] TokenRequestModel model)
        {
            var result = await _accountAppService.GetTokenAsync(model);
            if (!result.IsAuthenticated)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleModel model)
        {
            var result = await _accountAppService.AddRoleAsync(model);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpPost("Unassign-role")]
        public async Task<IActionResult> UnassignRole([FromBody] UnassignRoleModel model)
        {
            var result = await _accountAppService.UnassignRoleAsync(model);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpPost("update-Address")]
        public async Task<ActionResult<AddressDto>> CreateOrUpdate([FromBody] AddressDto dto)
        {
            if (dto == null)
                return BadRequest("Address data is required");

            var result = await _accountAppService.CreateOrUpdateAddress(dto);

            return Ok(result);
        }

        [HttpGet("auth-status")]
        public ActionResult GetAuthState()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false
            });
        }

        [HttpGet("userinfo")]
        public async Task<ActionResult<UserInfoDto>> GetUserInfo()
        {
            var userInfo = await _accountAppService.GetUserInfo();

            if (userInfo == null)
                return NotFound(new { Message = "User not found" });

            return Ok(userInfo);
        }


    }
}
