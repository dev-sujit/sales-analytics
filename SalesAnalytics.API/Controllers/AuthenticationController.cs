using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SalesAnalytics.API.Services;
using SalesAnalytics.Application.Services;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interface;
using SalesAnalytics.Core.Interfaces;

namespace SalesAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICommonResponseService _commonResponseService;
        private readonly ISalesDbContext _salesDbContext;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ICommonResponseService commonResponseService, ILogger<AuthenticationController> logger, ISalesDbContext salesDbContext)
        {
            _authenticationService = authenticationService;
            _commonResponseService = commonResponseService;
            _salesDbContext = salesDbContext;
            _logger = logger;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            try
            {
                var user = _salesDbContext.Users.Where(x => x.Username == loginModel.Username).FirstOrDefault();

                if (user == null)
                {
                    var errorResponse = _commonResponseService.CreateErrorResponse<UserLoginModel>("User Is Not Registered.", 404);
                    _logger.LogError("Error occurred: User Is Not Registered.");
                    return NotFound(errorResponse);
                }

                var token = await _authenticationService.Authenticate(loginModel);

                if (token == null)
                    return Unauthorized();

                var commonResponse = _commonResponseService.CreateSuccessResponse(token, "User Logged In Successfully.");

                _logger.LogInformation("$Post: login Data into table");

                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}");
                var errorResponse = _commonResponseService.CreateErrorResponse<UserLoginModel>($"An error occurred while Loggin User:{ex.Message}");
                return StatusCode(errorResponse.StatusCode, errorResponse);
            }
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(UserLoginModel registrationModel)
        {
            try
            {
                var result = await _authenticationService.Register(registrationModel);

                if (!result)
                    return BadRequest("User already exists");

                var commonResponse = _commonResponseService.CreateSuccessResponse(result, "User registered successfully.");
                
                _logger.LogInformation("$Post: Checking Login Data from table");

                return Ok(commonResponse);
            }
            catch (Exception ex) {
                _logger.LogError($"Error occurred: {ex.Message}");
                var errorResponse = _commonResponseService.CreateErrorResponse<UserLoginModel>($"An error occurred while User Register:{ex.Message}");
                return StatusCode(errorResponse.StatusCode, errorResponse);
            }
        }
    }
}
