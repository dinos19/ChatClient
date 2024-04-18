using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Models;
using SqliteWasmHelper;

namespace ChatClient.Infrastructure.Repositories
{
    public class AccountRepository : RepositoryBase<Account>
    {
        public AccountRepository(ISqliteWasmDbContextFactory<ClientDbContext> _dbContext) : base(_dbContext)
        {
        }
    }
}