using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Models;
using SqliteWasmHelper;

namespace ChatClient.Infrastructure.Repositories
{
    public class ChatMessageRepository : RepositoryBase<ChatMessage>
    {
        public ChatMessageRepository(ISqliteWasmDbContextFactory<ClientDbContext> _dbContext) : base(_dbContext)
        {
        }
    }
}