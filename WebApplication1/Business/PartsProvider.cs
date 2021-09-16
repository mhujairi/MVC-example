using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Business
{
    public class PartsProvider : IPartsProvider
    {
        private readonly IRepository<string, Part> partsRepository;

        public PartsProvider(IRepository<string, Part> partsRepository)
        {
            if (partsRepository is null)
            {
                throw new ArgumentNullException(nameof(partsRepository));
            }

            this.partsRepository = partsRepository;
        }

        public Task AddAsync(Part part)
        {
            return partsRepository.AddAsync(part);
        }

        public Task DeleteAsync(string key)
        {
            return partsRepository.DeleteAsync(key);
        }

        public Task<IEnumerable<Part>> GetAllAsync()
        {
            return partsRepository.GetAllAsync();
        }

        public Task<Part> GetAsync(string key)
        {
            return partsRepository.GetAsync(key);
        }

        public Task UpdateAsync(string id, Part part)
        {
            return partsRepository.UpdateAsync(id, part);
        }
    }
}
