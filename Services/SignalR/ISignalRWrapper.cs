using ChatClient.Models;

namespace ChatClient.Services.SignalR
{
    public interface ISignalRWrapper
    {
        public Task ConnectSignalRAsync();

        public Task<string> JoinChatroomAsync(UserConnection connection);
    }
}