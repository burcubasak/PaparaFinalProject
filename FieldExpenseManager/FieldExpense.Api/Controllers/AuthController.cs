using FieldExpenseManager.FieldExpense.Application.DTOs.Auth;
using FieldExpenseManager.FieldExpense.Application.Interfaces.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace FieldExpenseManager.FieldExpense.Api.Controllers
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
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var token = await _authService.LoginAsync(loginDto);
                if (token == null)
                {
                    return Unauthorized("Invalid email or password.");
                }
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
