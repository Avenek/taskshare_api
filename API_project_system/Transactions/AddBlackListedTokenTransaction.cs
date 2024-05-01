using Newtonsoft.Json.Linq;
using API_project_system.Entities;
using API_project_system.Repositories;

namespace API_project_system.Transactions
{
    public class AddBlackListedTokenTransaction : ITransaction
    {
        private readonly IRepository<BlackListedToken> blackListedTokensRepository;
        private readonly int userId;
        private readonly string jwtToken;

        public AddBlackListedTokenTransaction(IRepository<BlackListedToken> blackListedTokensRepository, int userId, string token)
        {
            this.blackListedTokensRepository = blackListedTokensRepository;
            this.userId = userId;
            this.jwtToken = token;
        }

        public void Execute()
        {
            BlackListedToken tokenToAdd = new BlackListedToken() { Token = jwtToken, UserId = userId };
            blackListedTokensRepository.Add(tokenToAdd);
        }
    }
}
