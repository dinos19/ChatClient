using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;
using System.Linq.Expressions;

namespace ChatClient.Infrastructure.Repositories.Abstraction
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected ISqliteWasmDbContextFactory<ClientDbContext> sqlFactory { get; set; }

        public RepositoryBase(ISqliteWasmDbContextFactory<ClientDbContext> factory)
        {
            sqlFactory = factory;
        }

        public async Task<List<T>> FindAllAsync()
        {
            var dbContext = await sqlFactory.CreateDbContextAsync();
            return await dbContext.Set<T>().AsNoTracking().ToListAsync();
        }
        public async Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            var dbContext = await sqlFactory.CreateDbContextAsync();
            return await dbContext.Set<T>().Where(expression).AsNoTracking().ToListAsync();

        }

        public async Task<T> CreateAsync(T entity)
        {
            var dbContext = await sqlFactory.CreateDbContextAsync();

            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var dbContext = await sqlFactory.CreateDbContextAsync();

            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            var dbContext = await sqlFactory.CreateDbContextAsync();

            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
    }
}