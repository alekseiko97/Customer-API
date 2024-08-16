using Customer_API.Models;
using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customer_API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountService"></param>
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
            try
            {
                var account = await _accountService.CreateAccountAsync(customerId, initialBalance);
                return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
            }
            catch (KeyNotFoundException knfe)
            {
                return NotFound(new { message = knfe.Message });
            }
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
            
            if (account == null) return NotFound($"Account with id {id} doesn't exist");
            
            return Ok(account);
        }

        /// <summary>
        /// Get all transactions for account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet("{accountId}/transactions")]
        public async Task<IActionResult> GetAllTransaction(int accountId)
        {
            try
            {
                var transactions = await _accountService.GetAllTransactions(accountId);
            
                if (transactions == null || !transactions.Any()) return NotFound($"No transaction(s) were found for accountId {accountId}");

                return Ok(transactions);
            }
            catch (KeyNotFoundException knfe)
            {
                return NotFound(new { message = knfe.Message });
            }
        }
    }
}
