using System.Threading.Tasks;
using ARSTAGE.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ARSTAGE.Services;

namespace ARSTAGE.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(model);

            if (result.success)
            {
                return Ok(new { result.token, result.message });
            }

            return Unauthorized(new { result.message });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model);

            if (result.success)
            {
                return Ok(new { result.message });
            }

            return BadRequest(new { result.message });
        }
    }
}