using Customer_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer_API.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public class UserService(ApplicationDbContext context) : IUserService
    {
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <returns></returns>
        public async Task<User> CreateUserAsync(string name, string surname)
        {
            User user = new()
            {
                Name = name,
                Surname = surname,
                Balance = 0 // initial balance is 0
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<User> GetUserInfoAsync(int customerId)
        {
            // Fetch the user with accounts and transactions included
            var user = await _context.Users
                .Include(u => u.Accounts)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(u => u.ID == customerId);

            if (user == null)
            {
                return null; // Will be handled by controller
            }

            user.Balance = await CalculateUserBalanceAsync(customerId);

            return user;
        }

        /// <summary>
        /// Calculate the user's total balance by summing up the balances of all accounts
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<decimal> CalculateUserBalanceAsync(int customerId)
        {
            var user = await _context.Users
                .Include(u => u.Accounts) // Eager load the accounts
                .SingleOrDefaultAsync(u => u.ID == customerId) ?? throw new ArgumentException("User not found", nameof(customerId));

            // Calculate the total balance by summing the balances of all accounts
            var totalBalance = user.Accounts.Sum(a => a.Balance);

            return totalBalance;
        }
    }
}
