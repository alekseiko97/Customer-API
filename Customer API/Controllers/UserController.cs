using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Customer_API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController(IUserService userService): ControllerBase
    {
        private readonly IUserService _userService = userService;
        
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetUser(int customerId)
        {
            var user = await _userService.GetUserInfoAsync(customerId);
            
            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromQuery] string firstName, [FromQuery] string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                return BadRequest("First name and last name are required!");
            }

            var user = await _userService.CreateUserAsync(firstName, lastName);

            return CreatedAtAction(nameof(GetUser), new { id = user.ID }, user);
        }
    }
}
