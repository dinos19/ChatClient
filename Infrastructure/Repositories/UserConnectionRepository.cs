using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Models;
using SqliteWasmHelper;

namespace ChatClient.Infrastructure.Repositories
{
    public class UserConnectionRepository : RepositoryBase<UserConnection>
    {
        public UserConnectionRepository(ISqliteWasmDbContextFactory<ClientDbContext> _dbContext) : base(_dbContext)
        {
        }
    }
}