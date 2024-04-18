using Blazored.LocalStorage;
using ChatClient;
using ChatClient.Infrastructure;
using ChatClient.Infrastructure.Repositories.Abstraction;
using ChatClient.Infrastructure.Repositories;
using ChatClient.Layout;
using ChatClient.Models;
using ChatClient.Services;
using ChatClient.Services.SignalR;
using ChatClient.Services.SignalR.Hubs;
using ChatClient.Services.ViewServices;
using ChatClient.Util;
using ChatClient.ViewModel;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
using ChatClient.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSqliteWasmDbContextFactory<ClientDbContext>(options => options.UseSqlite("Data Source=ClientDb.sqlite3"));

//ViewModels
builder.Services.AddScoped<RegisterViewModel>();
builder.Services.AddScoped<ChatViewModel>();

//Services
builder.Services.AddSingleton<ChatService>();
builder.Services.AddScoped<UserStateService>();

//repositories
builder.Services.AddTransient<AccountRepository>();
builder.Services.AddTransient<ChatMessageRepository>();
builder.Services.AddTransient<LatestUpdatesRepository>();
builder.Services.AddTransient<UserConnectionRepository>();
builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();

//handlers
builder.Services.AddScoped<RegisterHandler>();
builder.Services.AddScoped<ChatHandler>();
builder.Services.AddScoped<SyncHandler>();

//state
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserState>();

//hubs
builder.Services.AddScoped<ChatHub>();
builder.Services.AddScoped<SyncHub>();

await builder.Build().RunAsync();