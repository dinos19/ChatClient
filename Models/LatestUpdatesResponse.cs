namespace ChatClient.Models
{
    public class LatestUpdatesResponse
    {
        public Account CurrentAccount { get; set; }
        public List<Account> Accounts { get; set; }
        public List<ChatMessage> Messages { get; set; }
    }
}
