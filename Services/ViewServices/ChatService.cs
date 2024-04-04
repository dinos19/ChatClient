using ChatClient.Infrastructure;
using ChatClient.Models;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace ChatClient.Services.ViewServices
{
    public class ChatService
    {
        private ISqliteWasmDbContextFactory<ClientDbContext> BbContextFactory;

        public ChatService(ISqliteWasmDbContextFactory<ClientDbContext> bbContextFactory)
        {
            BbContextFactory = bbContextFactory;
        }

        public async Task<List<ChatMessage>> LoadSingleChatroom(int myAccountId, int otherAccountId)
        {
            List<ChatMessage> messages = new List<ChatMessage>();
            try
            {
                using var ctx = await BbContextFactory.CreateDbContextAsync();

                await ctx.ChatMessages.AddAsync(new ChatMessage { Body = "test body1", FromAccountId = 0, ToAccountId = 1, });
                await ctx.SaveChangesAsync();

                await ctx.ChatMessages.AddAsync(new ChatMessage { Body = "test body1", FromAccountId = 1, ToAccountId = 0 });
                await ctx.SaveChangesAsync();
                messages = ctx.ChatMessages.Where(x => (x.FromAccountId == myAccountId && x.ToAccountId == otherAccountId) || (x.FromAccountId == otherAccountId && x.ToAccountId == myAccountId)).ToList();
            }
            catch (Exception ex)
            {
            }

            return messages;
        }
    }
}