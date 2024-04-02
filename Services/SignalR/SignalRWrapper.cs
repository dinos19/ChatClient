using ChatClient.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClient.Services.SignalR
{
    public class SignalRWrapper : ISignalRWrapper
    {
        public Microsoft.AspNetCore.SignalR.Client.HubConnection Connection;

        public SignalRWrapper()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44312/chat").WithAutomaticReconnect()
                .Build();
        }

        public async Task ConnectSignalRAsync()
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

        public async Task<string> JoinSpecificChatRoomAsync(UserConnection connection)
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
    }
}