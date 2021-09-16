using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public interface IRepository<TKey,TType>
    {
        Task<IEnumerable<TType>> GetAllAsync();
        Task<TType> GetAsync(TKey key);
        Task AddAsync(TType item);
        Task UpdateAsync(TKey key,TType item);
        Task DeleteAsync(TKey key);
    }
}
