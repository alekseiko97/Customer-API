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
        
        /// <summary>
        /// Get existing user/customer by id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create new user 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromQuery] string firstName, [FromQuery] string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                return BadRequest("First name and last name are required!");
            }

            // Check whether a user with given first- and last name already exists in the system
            bool userExists = _userService.GetAllUsersAsync().Result
                .FirstOrDefault(u => u.Name == firstName && u.Surname == lastName) != null;

            if (userExists)
            {
                return BadRequest($"User with first name: {firstName} and last name: {lastName} already exists!");
            }

            var user = await _userService.CreateUserAsync(firstName, lastName);

            return CreatedAtAction(nameof(GetUser), new { customerId = user.ID }, user);
        }
    }
}
