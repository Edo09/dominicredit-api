using dominicredit_api.Dtos.Auth;
using dominicredit_api.Interfaces;
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
        private readonly ItokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ItokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;

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
                        return Ok(new NewUserDto
                        {
                            UserName = user.UserName ?? string.Empty,
                            Email = user.Email ?? string.Empty,
                            Token = _tokenService.CreateToken(user)
                        });
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
                return StatusCode(500, "Error Interno del Servidor: " + e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userManager.FindByNameAsync(loginDto.Username);
                if (user == null) return Unauthorized("Usuario invalido.");
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (result.Succeeded)
                {
                    return Ok(new NewUserDto
                    {
                        UserName = user.UserName ?? string.Empty,
                        Email = user.Email ?? string.Empty,
                        Token = _tokenService.CreateToken(user)
                    });
                }

                return Unauthorized("Invalid login attempt.");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error Interno del Servidor: " + e.Message);
            }
        }

    }
}
