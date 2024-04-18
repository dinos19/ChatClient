using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Models;
using ChatClient.Services.SignalR.Hubs;
using ChatClient.Services;

namespace ChatClient.Handlers
{
    public class SyncHandler
    {
        public event Action<ChatMessage> OnMessageReceived;

        public SyncHandler(IRepositoryWrapper repos, UserStateService userStateService)
        {
            Repos = repos;
            UserStateService = userStateService;
        }

        public IRepositoryWrapper Repos { get; set; }
        public UserStateService UserStateService { get; set; }

        public async Task<LatestUpdates> GetLatestUpdates()
        {
            var res = (await Repos.LatestUpdates.FindAllAsync()).FirstOrDefault();

            if (res == null)
            {
                res = new LatestUpdates
                {
                    CurrentAccount = UserStateService.CurrentState.MyAccount,
                    Account = DateTime.Now,
                    ChatMessage = DateTime.Now,
                    UserConnection = DateTime.Now,
                };
                await Repos.LatestUpdates.CreateAsync(res);
            }

            res.CurrentAccount = UserStateService.CurrentState.MyAccount;
            return res;
        }

        public async Task StoreUpdates(LatestUpdatesResponse latestUpdates)
        {
            var saveAccounts = Task.Run(async () =>
            {
                foreach (var account in latestUpdates.Accounts)
                    try
                    {
                        await Repos.Account.CreateAsync(account);

                    }
                    catch (Exception)
                    {

                        continue;
                    }
            });

            var saveMessages = Task.Run(async () =>
            {
                foreach (var message in latestUpdates.Messages)
                    try
                    {
                        await Repos.ChatMessage.CreateAsync(message);

                    }
                    catch (Exception)
                    {
                        continue;
                    }
            });
            var saveLatestUpdates = Task.Run(async () =>
            {
                var update = new LatestUpdates
                {
                    ChatMessage = DateTime.Now,
                    Account = DateTime.Now
                };
            });

            Task[] tasks = new Task[] { saveAccounts, saveMessages, saveLatestUpdates };
            await Task.WhenAll(tasks);
        }
    }
}