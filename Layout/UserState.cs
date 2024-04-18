using ChatClient.Models;
using Microsoft.AspNetCore.Components;

namespace ChatClient.Layout
{
    public class UserState
    {
        public UserState()
        {
            Accounts = new List<Account>();
        }

        public UserState(Account myAccount, Account currentChatroom, Account previousChatroom, UserConnection currentChatConnection)
        {
            Accounts = new List<Account>();
            MyAccount = myAccount;
            CurrentChatroom = currentChatroom;
            PreviousChatroom = previousChatroom;
            CurrentChatConnection = currentChatConnection;
        }

        public List<Account> Accounts { get; set; }
        public Account MyAccount { get; set; }
        public Account CurrentChatroom { get; set; }
        public Account PreviousChatroom { get; set; }
        public UserConnection CurrentChatConnection { get; set; }
    }
}