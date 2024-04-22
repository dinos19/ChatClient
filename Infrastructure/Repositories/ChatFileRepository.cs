using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Models;
using SqliteWasmHelper;

namespace ChatClient.Infrastructure.Repositories
{
    public class ChatFileRepository : RepositoryBase<ChatFile>
    {
        public ChatFileRepository(ISqliteWasmDbContextFactory<ClientDbContext> _dbContext) : base(_dbContext)
        {
        }
    }
}