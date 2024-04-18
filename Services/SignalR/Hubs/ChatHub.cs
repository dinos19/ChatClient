using ChatClient.Models;
using ChatClient.Util;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClient.Services.SignalR.Hubs
{
    public class ChatHub : ISignalRWrapper
    {
        public HubConnection Connection { get; set; }

        public ChatHub()
        {
            Connection = new HubConnectionBuilder()
                //.WithUrl("https://localhost:44312/chat").WithAutomaticReconnect()
                .WithUrl($"{Constants.ChatServerUrl}/chat").WithAutomaticReconnect()
                .Build();
        }

        public async Task ConnectHubAsync()
        {
            try
            {
                await Connection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<string> JoinChatroomAsync(UserConnection connection)
        {
            var res = string.Empty;
            try
            {
                string result = string.Empty;
                if (Connection.State == HubConnectionState.Connected)
                    result = await Connection.InvokeCoreAsync<string>("JoinChat", new object[] { connection });
                else if (Connection.State == HubConnectionState.Disconnected)
                    await Connection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION HAPPENED ON JoinChatroomAsync " + ex);
            }

            return res;
        }

        public async Task<string> JoinSpecificChatRoomAsync(Account connection)
        {
            var res = string.Empty;
            try
            {
                string result = string.Empty;
                if (Connection.State == HubConnectionState.Connected)
                    result = await Connection.InvokeCoreAsync<string>("JoinSpecificChatRoom", new object[] { connection });
                else if (Connection.State == HubConnectionState.Disconnected)
                    await Connection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION HAPPENED ON JoinChatroomAsync " + ex);
            }

            return res;
        }

        public async Task<UserConnection> SayHello(Account account)
        {
            return await Connection.InvokeCoreAsync<UserConnection>("SayHello", new object[] { account });
        }

        public async Task<ChatMessage> SendMessage(ChatMessage chatMessage)
        {
            return await Connection.InvokeCoreAsync<ChatMessage>("SendMessage", new object[] { chatMessage });
        }
    }
}