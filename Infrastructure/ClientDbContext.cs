using ChatClient.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatClient.Infrastructure
{
    public class ClientDbContext : DbContext
    {
        public ClientDbContext(DbContextOptions<ClientDbContext> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<LatestUpdates> LatestUpdates { get; set; }
        public DbSet<ChatFile> ChatFile { get; set; }
    }
}