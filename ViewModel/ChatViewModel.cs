using ChatClient.Handlers;
using ChatClient.Infrastructure;
using ChatClient.Models;
using ChatClient.Services;
using ChatClient.Services.SignalR.Hubs;
using ChatClient.Services.ViewServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SqliteWasmHelper;

namespace ChatClient.ViewModel
{
    public class ChatViewModel : BaseViewModel
    {
        public event Action onMessageReceived;

        public ChatService ChatService { get; set; }
        public UserStateService UserStateService { get; set; }
        public ChatHandler chatHandler { get; set; }
        private ISqliteWasmDbContextFactory<ClientDbContext> dbContext { get; set; }

        private string _newMessageText;

        public string newMessageText
        {
            get => _newMessageText;
            set { _newMessageText = value; OnPropertyChanged(); }
        }

        private int _fileSize;

        public int fileSize
        {
            get => _fileSize;
            set { _fileSize = value; OnPropertyChanged(); }
        }

        private List<ChatMessage> _messages;

        public List<ChatMessage> Messages
        {
            get => _messages;
            set { _messages = value; OnPropertyChanged(); }
        }

        public ChatViewModel(ChatService chatService, UserStateService userStateService, ChatHandler _chatHandler, ISqliteWasmDbContextFactory<ClientDbContext> dbContext)
        {
            Messages = new List<ChatMessage>();
            ChatService = chatService;
            UserStateService = userStateService;
            this.dbContext = dbContext;
            chatHandler = _chatHandler;

            chatHandler.OnMessageReceived += MessageReceived;
        }

        private void MessageReceived(ChatMessage chatMessage)
        {
            Messages.Add(chatMessage);
            newMessageText = "";

            onMessageReceived?.Invoke();
        }
    }
}