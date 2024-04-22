using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Models;
using ChatClient.Models.ClientAnnouncements;
using ChatClient.Services;
using ChatClient.Services.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChatClient.Handlers
{
    public class ChatHandler
    {
        public event Action<ChatMessage> OnMessageReceived;

        public ChatHandler(IRepositoryWrapper repos, UserStateService userStateService, ChatHub signalr)
        {
            Repos = repos;
            UserStateService = userStateService;
            this.signalr = signalr;
        }

        private ChatHub signalr { get; set; }

        public IRepositoryWrapper Repos { get; set; }
        public UserStateService UserStateService { get; set; }

        private async Task<ChatMessage> HandleNewMessage(string message)
        {
            ChatMessage chatMessage = null;
            try
            {
                chatMessage = JsonConvert.DeserializeObject<ChatMessage>(message);

                switch (chatMessage.Action)
                {
                    case ChatMessageAction.NOACTION:
                        try
                        {
                            await Repos.ChatMessage.CreateAsync(chatMessage);
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex);
                        }
                        chatMessage.FromAccount = UserStateService.CurrentState.CurrentChatroom;
                        chatMessage.ToAccount = UserStateService.CurrentState.MyAccount;

                        break;

                    case ChatMessageAction.ANNOUNCEMENTS:
                        break;

                    case ChatMessageAction.HELLO:
                        //UserStateService.CurrentState.CurrentChatConnection = await signalr.SayHello(UserStateService.CurrentState.MyAccount);
                        await signalr.SayHello(UserStateService.CurrentState.MyAccount);
                        await signalr.JoinSpecificChatRoomAsync(UserStateService.CurrentState.CurrentChatroom);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return chatMessage;
        }

        public async Task<bool> SendReadUpdates(List<ChatMessage> messages)
        {
            try
            {
                foreach (var message in messages)
                {
                    if (message.ReadDate == DateTime.MinValue)
                    {
                        ChatMessage chatMessage = new ChatMessage
                        {
                            Action = ChatMessageAction.ANNOUNCEMENTS,
                            Body = JsonConvert.SerializeObject(new ReadUpdate
                            {
                                ChatMessageId = message.ChatMessageId,
                                ReadDate = DateTime.Now
                            }),
                            FromAccountId = UserStateService.CurrentState.MyAccount.AccountId,
                            ToAccountId = 0,
                            Type = ChatMessageType.TEXT,
                            Status = ChatMessageStatus.INIT,
                            CreatedDate = DateTime.Now
                        };

                        var msg = await signalr.SendMessage(chatMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public async void SendMessage(string body)
        {
            ChatMessage chatMessage = new ChatMessage
            {
                Action = ChatMessageAction.NOACTION,
                Body = body,
                FromAccountId = UserStateService.CurrentState.MyAccount.AccountId,
                ToAccountId = UserStateService.CurrentState.CurrentChatroom.AccountId,
                Type = ChatMessageType.TEXT,
                Status = ChatMessageStatus.INIT,
                CreatedDate = DateTime.Now
            };
            try
            {
                var msg = await signalr.SendMessage(chatMessage);

                await Repos.ChatMessage.CreateAsync(msg);

                msg.FromAccount = UserStateService.CurrentState.MyAccount;
                msg.ToAccount = UserStateService.CurrentState.CurrentChatroom;

                OnMessageReceived?.Invoke(msg);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task SendMessage(UploadResult uploadResult, ChatMessageType type)
        {
            ChatMessage chatMessage = new ChatMessage
            {
                Action = ChatMessageAction.NOACTION,
                Body = JsonConvert.SerializeObject(uploadResult),
                FromAccountId = UserStateService.CurrentState.MyAccount.AccountId,
                ToAccountId = UserStateService.CurrentState.CurrentChatroom.AccountId,
                Type = type,
                Status = ChatMessageStatus.INIT,
                CreatedDate = DateTime.Now
            };

            try
            {
                var msg = await signalr.SendMessage(chatMessage);

                await Repos.ChatMessage.CreateAsync(msg);

                msg.FromAccount = UserStateService.CurrentState.MyAccount;
                msg.ToAccount = UserStateService.CurrentState.CurrentChatroom;

                OnMessageReceived?.Invoke(msg);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<ChatFile> GetFile(ChatMessage message)
        {
            ChatFile chatFile = default(ChatFile);

            try
            {
                var files = await Repos.ChatFile.FindAllAsync();
                foreach (var file in files)
                {
                    Console.WriteLine($"STORED FILE : UploadResultFileName {file.UploadResultFileName} , UploadResultStoredFileName {file.UploadResultStoredFileName}");
                }
                UploadResult uploadResult = JsonConvert.DeserializeObject<UploadResult>(message.Body);
                var chatFiles = await Repos.ChatFile.FindByConditionAsync(x => uploadResult.Id == x.UploadResultId);
                chatFile = chatFiles.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return chatFile;
        }

        public async Task SetUpSignalR()
        {
            await signalr.ConnectHubAsync();

            signalr.Connection.On<string>("MessageReceived", (message) =>
            {
                var msg = JsonConvert.DeserializeObject<ChatMessage>(message);
                OnMessageReceived?.Invoke(msg);
                Console.WriteLine(msg);
            });

            signalr.Connection.On<string>("ReceiveSpecificMessage", (message) =>
            {
                var msg = JsonConvert.DeserializeObject<ChatMessage>(message);
                OnMessageReceived?.Invoke(msg);
                Console.WriteLine(msg);
            });

            signalr.Connection.On<string>("ReceiveMessage", async (message) =>
            {
                var msg = await HandleNewMessage(message);
                if (msg.Action == ChatMessageAction.NOACTION)
                    OnMessageReceived?.Invoke(msg);
                Console.WriteLine(message);
            });
        }
    }
}