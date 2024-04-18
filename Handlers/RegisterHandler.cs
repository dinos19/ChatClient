using ChatClient.Infrastructure;
using ChatClient.Models;
using ChatClient.Services;
using SqliteWasmHelper;
using System.Text.Json;
using System.Text;
using ChatClient.Infrastructure.Repositories;
using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Util;

namespace ChatClient.Handlers
{
    public class RegisterHandler
    {
        public RegisterHandler(IRepositoryWrapper repos, UserStateService userStateService)
        {
            Repos = repos;
            UserStateService = userStateService;
        }

        public IRepositoryWrapper Repos { get; set; }
        public UserStateService UserStateService { get; set; }

        public async Task<LoginResponse> LoginAccountAsync(Account Account)
        {
            LoginResponse loginResponse = null;
            var httpClient = new HttpClient();
            var accountJson = JsonSerializer.Serialize(Account);
            var content = new StringContent(accountJson, Encoding.UTF8, "application/json");

            try
            {
                //await UserStateService.InitializeAsync();
                HttpResponseMessage response = await httpClient.PostAsync($"{Constants.ChatServerUrl}/Account/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response body as a string
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the response body string back to an Account object
                    loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (loginResponse.IsLoggedIn)
                    {
                        try
                        {
                            foreach (var account in loginResponse.OnlineAccounts)
                            {
                                try
                                {
                                    var acc = await Repos.Account.CreateAsync(account);
                                }
                                catch (Exception ex)
                                {
                                    //Console.WriteLine(ex);
                                    continue;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex);
                        }
                        await UserStateService.AddAccounts(loginResponse.OnlineAccounts);
                        Account.AccountId = loginResponse.OnlineAccounts.Where(x => x.Email == Account.Email && x.Name == Account.Name).Select(x => x.AccountId).FirstOrDefault();
                        //UserStateService.CurrentState.MyAccount.AccountId = loginResponse.OnlineAccounts.Where(x => x.Email == Account.Email && x.Name == Account.Name).Select(x => x.AccountId).FirstOrDefault();
                        await UserStateService.SetMyAccount(Account);

                        Console.WriteLine($"Login successfull.  {JsonSerializer.Serialize(loginResponse)}");
                    }
                    else
                        Console.WriteLine("Login NOT successfull.");
                }
                else
                {
                    Console.WriteLine($"Failed to Login. Status code: {response.StatusCode}");
                }

                return loginResponse;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return loginResponse;
            }
        }

        public async Task<List<Account>> RegisterAccountAsync(Account Account)
        {
            //await UserStateService.InitializeAsync();

            List<Account> responseAccounts = new();
            var httpClient = new HttpClient();
            var accountJson = JsonSerializer.Serialize(Account);
            var content = new StringContent(accountJson, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync($"{Constants.ChatServerUrl}/Account/Register", content);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response body as a string
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the response body string back to an Account object
                    responseAccounts = JsonSerializer.Deserialize<List<Account>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    Console.WriteLine($"Account registered successfully. ID: {Account?.AccountId}");
                    UserStateService.CurrentState.MyAccount = Account;
                    UserStateService.CurrentState.MyAccount.AccountId = responseAccounts.Where(x => x.Email == Account.Email && x.Name == Account.Name).Select(x => x.AccountId).FirstOrDefault();
                }
                else
                {
                    Console.WriteLine($"Failed to register account. Status code: {response.StatusCode}");
                }

                return responseAccounts;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return responseAccounts;
            }
        }
    }
}