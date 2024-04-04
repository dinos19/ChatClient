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

        public List<Account> Accounts { get; set; }
        public Account MyAccount { get; set; }
        public Account CurrentChatroom { get; set; }
        public Account PreviousChatroom { get; set; }
    }
}