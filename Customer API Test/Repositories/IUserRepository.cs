using Customer_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_API_Test.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
    }
}
