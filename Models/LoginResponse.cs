namespace ChatClient.Models
{
    public class LoginResponse
    {
        public bool IsLoggedIn { get; set; }
        public List<Account> OnlineAccounts { get; set; }
    }
}