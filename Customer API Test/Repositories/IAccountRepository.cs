using Customer_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_API_Test.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> AddAccountAsync(Account account);
    }
}
