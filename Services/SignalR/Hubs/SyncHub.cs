using ChatClient.Handlers;
using ChatClient.Models;
using ChatClient.Util;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatClient.Services.SignalR.Hubs
{
    public class SyncHub : ISignalRWrapper
    {
        public HubConnection Connection { get; set; }
        public SyncHandler SyncHandler { get; set; }

        public SyncHub(SyncHandler syncHandler)
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"{Constants.ChatServerUrl}/Sync").WithAutomaticReconnect()
                .Build();

            SyncHandler = syncHandler;
        }

        public async Task ConnectHubAsync()
        {
            try
            {
                if(Connection.State != HubConnectionState.Connected)
                    await Connection.StartAsync();

                //fetch sync object from db
                LatestUpdates latestUpdates = await SyncHandler.GetLatestUpdates();
                //send it to the server
                var res = await SyncAsync(latestUpdates);

                //store in db and cache the new dates
                await SyncHandler.StoreUpdates(res);
                //expect updates
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<LatestUpdatesResponse> SyncAsync(LatestUpdates latestUpdates)
        {
            var res = await Connection.InvokeCoreAsync<LatestUpdatesResponse>("SyncAsync", new object[] { latestUpdates });
            return res;
        }
    }
}