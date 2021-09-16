using System.Collections.Generic;
using System.Threading.Tasks;

using WebApplication1.Models;

namespace WebApplication1.Business
{
    public interface IPartsProvider
    {
        Task<IEnumerable<Part>> GetAllAsync();
        Task<Part> GetAsync(string key);
        Task AddAsync(Part part);
        Task UpdateAsync(string id, Part part);
        Task DeleteAsync(string key);
    }
}
