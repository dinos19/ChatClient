using ChatClient.Infrastructure.Repositories.Abstraction;
using SqliteWasmHelper;

namespace ChatClient.Infrastructure.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ISqliteWasmDbContextFactory<ClientDbContext> dbContext;
        private AccountRepository _account;

        public AccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(dbContext);
                }
                return _account;
            }
        }

        private LatestUpdatesRepository _latestUpdates;

        public LatestUpdatesRepository LatestUpdates
        {
            get
            {
                if (_latestUpdates == null)
                {
                    _latestUpdates = new LatestUpdatesRepository(dbContext);
                }
                return _latestUpdates;
            }
        }

        private ChatMessageRepository _chatMessage;

        public ChatMessageRepository ChatMessage
        {
            get
            {
                if (_chatMessage == null)
                {
                    _chatMessage = new ChatMessageRepository(dbContext);
                    return _chatMessage;
                }

                return _chatMessage;
            }
        }

        private UserConnectionRepository _userConnection;

        public UserConnectionRepository UserConnection
        {
            get
            {
                if (_userConnection == null)
                {
                    _userConnection = new UserConnectionRepository(dbContext);
                    return _userConnection;
                }

                return _userConnection;
            }
        }

        public RepositoryWrapper(ISqliteWasmDbContextFactory<ClientDbContext> _dbContext)
        {
            dbContext = _dbContext;
        }

        public void Save()
        {
        }
    }
}