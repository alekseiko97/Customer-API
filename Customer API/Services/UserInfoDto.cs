using Customer_API.Models;

namespace Customer_API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserInfoDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}