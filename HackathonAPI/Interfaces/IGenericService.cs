using HackathonAPI.Models;
using HackathonAPI.Parameters;

namespace HackathonAPI.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<TResult> AddAsync<TSource, TResult>(TSource entity);
        Task<List<TResult>> GetAllAsync<TResult>();
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task<TResult> GetByIdAsync<TResult>(int? id);
        Task RemoveAsync(int? id);
        Task UpdateAsync<TSource>(int id, TSource source);
    }
}
