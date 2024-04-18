using ChatClient.Layout;
using ChatClient.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClient.Services.SignalR
{
    public interface ISignalRWrapper
    {
        public HubConnection Connection { get; set; }

        public Task ConnectHubAsync();

        //public Task<string> JoinChatroomAsync(UserConnection connection);

        //public Task<string> JoinSpecificChatRoomAsync(UserConnection connection);

        //public Task<UserConnection> SayHello(Account account);
        //public Task<ChatMessage> SendMessage(ChatMessage chatMessage);
    }
}