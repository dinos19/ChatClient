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

                //await AddFakeMessages(ctx,myAccountId,otherAccountId);
                messages = await ctx.ChatMessages
                            .Include(x => x.FromAccount)
                            .Include(x => x.ToAccount)
                            .Where(x => (x.FromAccountId == myAccountId && x.ToAccountId == otherAccountId)
                                     || (x.FromAccountId == otherAccountId && x.ToAccountId == myAccountId))
                            .OrderBy(x => x.CreatedDate)
                            .ToListAsync();
            }
            catch (Exception ex)
            {
            }

            return messages;
        }

        private async Task AddFakeMessages(ClientDbContext ctx, int myAccountId, int otherAccountId)
        {
            await ctx.ChatMessages.AddAsync(new ChatMessage { Body = "test body1", FromAccountId = myAccountId, ToAccountId = otherAccountId, });
            await ctx.SaveChangesAsync();

            await ctx.ChatMessages.AddAsync(new ChatMessage { Body = "test body1", FromAccountId = otherAccountId, ToAccountId = myAccountId });
            await ctx.SaveChangesAsync();
        }
    }
}