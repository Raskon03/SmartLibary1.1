using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain;

namespace SmartLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] string fullName)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Email = "student@tu-varna.bg" // Пример
            };

            await _userRepo.CreateAsync(user);
            return Ok(new { user.Id, user.FullName });
        }
    }
}