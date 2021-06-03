using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snowdrop.Auth.Managers.JwtAuthManager;
using Snowdrop.Auth.Models;
using Snowdrop.BL.Tests.Unit.Services.Users;
using Snowdrop.Infrastructure.Dto.Users;

namespace Snowdrop.Api.Controllers
{
    [AllowAnonymous]
    [Controller]
    [Route("[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IJwtAuthManager m_authManager = default;
        private readonly IUserService m_userService = default;

        public AuthController(
            IJwtAuthManager authManager,
            IUserService userService)
        {
            m_authManager = authManager;
            m_userService = userService;
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<JwtAuthResult>> SignUp([FromBody] SignUpRequest request)
        {
            UserResponse userResult = await m_userService.SignUp(request);
            JwtAuthResult authResult = await m_authManager.GenerateToken(userResult.UserName, userResult.Claims);
            
            return Ok(authResult);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<JwtAuthResult>> SignIn([FromBody] SignInRequest request)
        {
            UserResponse userResult = await m_userService.SignIn(request);
            JwtAuthResult authResult = await m_authManager.GenerateToken(userResult.UserName, userResult.Claims);
            
            return Ok(authResult);
        }
        
        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public new ActionResult SignOut()
        {
            m_authManager.RemoveRefreshToken(User.Identity?.Name);
            return Ok();
        }
    }
}