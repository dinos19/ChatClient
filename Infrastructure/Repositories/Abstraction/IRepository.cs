using System.Linq.Expressions;

namespace ChatClient.Infrastructure.Repositories.Abstraction
{
    public interface IRepository<T>
    {
        Task<List<T>> FindAllAsync();

        Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);

        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity); // UpdateAsync implementation might not need to be async. See below.

        Task<T> DeleteAsync(T entity);
    }
}