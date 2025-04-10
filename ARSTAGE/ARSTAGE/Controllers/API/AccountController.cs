﻿using System.Threading.Tasks;
using ARSTAGE.Models;
using ARSTAGE.Services;
using ARSTAGE.Models;
using ARSTAGE.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(model);

            if (result.success)
            {
                return Ok(new { token = result.token, message = result.message });
            }

            return Unauthorized(new { message = result.message });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(model);

            if (result.success)
            {
                return Ok(new { message = result.message });
            }

            return BadRequest(new { message = result.message });
        }
    }
}