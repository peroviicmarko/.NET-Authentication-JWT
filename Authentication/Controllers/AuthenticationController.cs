using Authentication.Common;
using Authentication.DTOs.User;
using Authentication.Errors;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private AuthenticationService _authenticationService;
        public AuthenticationController(IConfiguration configuration)
        {
            _authenticationService = new AuthenticationService(configuration);
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public IActionResult SignIn([FromBody] UserLogInRequest request)
        {
            User? user = _authenticationService.Authenticate(request);

            if (user is null)
            {
                throw new APIError(Constants.UserNotFound, StatusCodes.Status404NotFound);
            }

            UserBase user_ = new()
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role,
            };

            string token = _authenticationService.GenerateJwt(user);

            UserLogInResponse response = new()
            {
                Token = token,
                User = user_
            };

            return Ok(response);
        }
    }
}
