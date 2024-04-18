using ChatClient.Infrastructure;
using ChatClient.Models;
using ChatClient.Services;
using ChatClient.Util;
using Microsoft.AspNetCore.Components;
using SqliteWasmHelper;
using System.Text.Json;
using System.Text;
using System.Windows.Input;
using ChatClient.Handlers;

namespace ChatClient.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        public event Action OnRegistrationSuccess;

        public event Action OnLoginSuccess;

        private Account _account;
        private RegisterHandler _registerHandler;

        public RegisterViewModel(RegisterHandler registerHandler)
        {
            _registerHandler = registerHandler;
            HandleRegistration = new RelayCommand(async () => await RegisterAsync());
            HandleLogin = new RelayCommand(async () => await LoginAsync());
            _account = new Account();
        }

        public Account Account
        {
            get => _account;
            set { _account = value; OnPropertyChanged(); }
        }

        public ICommand HandleRegistration { get; }
        public ICommand HandleLogin { get; }

        private async Task RegisterAsync()
        {
            await _registerHandler.RegisterAccountAsync(Account);
            Console.WriteLine("Registration successful!");
            OnRegistrationSuccess?.Invoke();
        }

        private async Task LoginAsync()
        {
            await _registerHandler.LoginAccountAsync(Account);
            OnLoginSuccess?.Invoke();
        }
    }
}