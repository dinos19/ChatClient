using ChatClient;
using ChatClient.Infrastructure;
using ChatClient.Layout;
using ChatClient.Models;
using ChatClient.Services.SignalR;
using ChatClient.Services.ViewServices;
using ChatClient.Util;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSingleton<UserState>();
builder.Services.AddSingleton<ChatService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<ISignalRWrapper, SignalRWrapper>();
builder.Services.AddSqliteWasmDbContextFactory<ClientDbContext>(options => options.UseSqlite("Data Source=ClientDb.sqlite3"));
await builder.Build().RunAsync();