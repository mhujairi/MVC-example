using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using WebApplication1.Models;
using WebApplication1.Models.Validation;

namespace WebApplication1.Data
{
    public class FilePartsRepository : IRepository<string, Part>
    {
        private class RootObject
        {
            public List<Part> Parts { get; set; }
        }

        private readonly string filePath;
        private readonly IValidator<Part> validator;
        private RootObject rootObject;

        public FilePartsRepository(string filePath, IValidator<Part> validator)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath));
            }

            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }


            this.filePath = filePath;
            this.validator = validator;

            if (File.Exists(filePath))
            {
                //throw new ArgumentException($"'{nameof(filePath)}' of \"{filePath}\" does not exist.", nameof(filePath));
                Task.WaitAll(ReadFileAsync());
            }
            else
            {
                rootObject = new RootObject
                {
                    Parts = new List<Part>()
                };
                Task.WaitAll(UpdateFileAsync());
            }
        }

        private async Task ReadFileAsync()
        {
            try
            {

                var json = await File.ReadAllTextAsync(this.filePath);
                this.rootObject = JsonConvert.DeserializeObject<RootObject>(json);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private async Task UpdateFileAsync()
        {
            await File.WriteAllTextAsync(
                this.filePath,
                JsonConvert.SerializeObject(this.rootObject)
            );
        }


        public async Task AddAsync(Part item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await validator.ValidateAsync(item);
            Part duplicatePart = GetPart(item.PartNumberCommonized);
            if (duplicatePart != null)
            {
                throw new ArgumentException($"'A part with a {nameof(item.PartNumberCommonized)}' of {item.PartNumberCommonized} already exists.", nameof(item.PartNumberCommonized));
            }

            this.rootObject.Parts.Add(item);

            await UpdateFileAsync();
        }

        public async Task DeleteAsync(string partNumberCommonized)
        {
            Part partToDelete = GetPart(partNumberCommonized);
            if(partToDelete == null)
            {
                throw new ArgumentException($"'A part with a {nameof(partNumberCommonized)}' of {partNumberCommonized} cannot be found.", nameof(partNumberCommonized));
            }

            this.rootObject.Parts.Remove(partToDelete);

            await UpdateFileAsync();
        }

        private Part GetPart(string partNumberCommonized)
        {
            if (string.IsNullOrWhiteSpace(partNumberCommonized))
            {
                throw new ArgumentException($"'{nameof(partNumberCommonized)}' cannot be null or whitespace.", nameof(partNumberCommonized));
            }

            return this.rootObject.Parts.FirstOrDefault(part => string.Equals(partNumberCommonized, part.PartNumberCommonized));
        }

        public async Task<Part> GetAsync(string partNumberCommonized)
        {
            if (string.IsNullOrWhiteSpace(partNumberCommonized))
            {
                throw new ArgumentException($"'{nameof(partNumberCommonized)}' cannot be null or whitespace.", nameof(partNumberCommonized));
            }

            var part= GetPart(partNumberCommonized);
            if (part == null)
            {
                throw new ArgumentException($"'A part with a {nameof(partNumberCommonized)}' of {partNumberCommonized} cannot be found.", nameof(partNumberCommonized));
            }

            return part;
        }

        public async Task<IEnumerable<Part>> GetAllAsync()
        {
            return this.rootObject?.Parts?.AsEnumerable() ?? Enumerable.Empty<Part>();
        }

        public async Task UpdateAsync(string partNumberCommonized, Part item)
        {

            if (string.IsNullOrWhiteSpace(partNumberCommonized))
            {
                throw new ArgumentException($"'{nameof(partNumberCommonized)}' cannot be null or whitespace.", nameof(partNumberCommonized));
            }

            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await validator.ValidateAsync(item);

            Part partToUpdate = GetPart(partNumberCommonized);
            if (partToUpdate == null)
            {
                throw new ArgumentException($"'A part with a {nameof(partNumberCommonized)}' if {partNumberCommonized} cannot be found.", nameof(partNumberCommonized));
            }

            if (!string.Equals(item.PartNumberCommonized, partNumberCommonized))
            {
                Part duplicatePart = GetPart(item.PartNumberCommonized);
                if (duplicatePart != null)
                {
                    throw new ArgumentException($"'A part with a {nameof(item.PartNumberCommonized)}' of {item.PartNumberCommonized} already exists.", nameof(item.PartNumberCommonized));
                }
            }

            partToUpdate.Copy(item);


            await UpdateFileAsync();
        }
    }
}
