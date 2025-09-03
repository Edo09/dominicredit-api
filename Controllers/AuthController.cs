using dominicredit_api.Dtos.Auth;
using dominicredit_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dominicredit_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new { Token = "fake-jwt-tokend" });
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }
                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }


}
