using System.Linq.Expressions;

namespace ChatClient.Infrastructure.Repositories.Abstraction
{
    public interface IRepositoryWrapper
    {
        AccountRepository Account { get; }
        ChatMessageRepository ChatMessage { get; }
        UserConnectionRepository UserConnection { get; }
        LatestUpdatesRepository LatestUpdates { get; }

        void Save();
    }
}