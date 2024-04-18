using ChatClient.Layout;
using ChatClient.Models;
using System.Text.Json;
using Blazored.LocalStorage;
using System.Linq;

namespace ChatClient.Services
{
    public class UserStateService
    {
        private readonly ILocalStorageService _localStorage;
        public UserState CurrentState { get; private set; } = new UserState();

        public event Action OnChange;

        public UserStateService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task InitializeAsync()
        {
            var state = await _localStorage.GetItemAsStringAsync("UserState");
            if (!string.IsNullOrWhiteSpace(state))
            {
                CurrentState = JsonSerializer.Deserialize<UserState>(state) ?? new UserState();
            }
            else
                CurrentState = new UserState(new Account(), new Account(), new Account(), new UserConnection());
            NotifyStateChanged();
        }

        public async Task SetMyAccount(Account account)
        {
            if (CurrentState.MyAccount.AccountId != account.AccountId)
            {
                CurrentState.MyAccount = account;
                await SaveStateAsync();
            }
        }

        public async Task AddAccounts(List<Account> accounts)
        {
            foreach (var account in accounts)
            {
                if (!CurrentState.Accounts.Contains(account))
                {
                    CurrentState.Accounts.Add(account);
                    await SaveStateAsync();
                }
            }
        }

        public async Task SetCurrentChatroom(Account chatroom)
        {
            CurrentState.PreviousChatroom = CurrentState.CurrentChatroom;
            CurrentState.CurrentChatroom = chatroom;
            await SaveStateAsync();
        }

        private async Task SaveStateAsync()
        {
            await _localStorage.SetItemAsStringAsync("UserState", JsonSerializer.Serialize(CurrentState));
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}