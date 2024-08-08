using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;

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
            throw new NotImplementedException();
        }
    }
}
