using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customer_API.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromQuery] int customerId, [FromQuery] decimal initialBalance)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            throw new NotImplementedException();
        }
    }
}
