using Authentication.Contracts;
using Authentication.DataAccess;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentificationController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public IdentificationController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest request, AuthService authService)
        {
            var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Login == request.Login);
            if (existingUser != null)
            {
                return BadRequest("Пользователь уже зарегистрирован");
            }

            var newUser = new User
            {
                Login = request.Login,
                Email = request.Email,
                Password = request.Password,
                Roles = request.Roles
            };

            _appDbContext.Users.Add(newUser);
            await _appDbContext.SaveChangesAsync();

            // Generate authentication token
            var token = authService.GenerateToken(newUser);

            return Ok(token);
        }
        
        [HttpPost("authorization")]
        public async Task<IActionResult> Authorization([FromBody] AuthUserRequest request, AuthService authService)
        {
            var existingUser =
                await _appDbContext.Users.FirstOrDefaultAsync(u =>
                    (u.Login == request.Login && u.Password == request.Password && u.Roles == request.Roles));
            if (existingUser == null)
            {
                return BadRequest("Такого пользователя не существует");
            }
            
            var token = authService.GenerateToken(existingUser);
            return Ok(token);
        }

        [HttpGet("/signin")]
        [Authorize(Roles = "admin")]
        public IActionResult Signin()
        {
            return Ok("Вы успешно авторизировались!");
        }
    }
}