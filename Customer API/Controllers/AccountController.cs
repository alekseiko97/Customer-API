using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customer_API.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        /// <summary>
        /// Create new account for existing user
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="initialBalance"></param>
        /// <returns></returns>
        [HttpPost]   
        public async Task<IActionResult> CreateAccount([FromQuery] int customerId, [FromQuery] decimal initialBalance)
        {
            var account = await _accountService.CreateAccountAsync(customerId, initialBalance);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id, account });
        }

        /// <summary>
        /// Get an account by accountId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var account = await _accountService.GetAccountAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }
    }
}
