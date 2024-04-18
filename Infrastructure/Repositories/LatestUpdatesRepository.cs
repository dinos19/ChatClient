using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Models;
using SqliteWasmHelper;

namespace ChatClient.Infrastructure.Repositories
{
    public class LatestUpdatesRepository : RepositoryBase<LatestUpdates>
    {
        public LatestUpdatesRepository(ISqliteWasmDbContextFactory<ClientDbContext> _dbContext) : base(_dbContext)
        {
        }
    }
}