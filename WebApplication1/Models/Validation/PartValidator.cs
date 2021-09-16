using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApplication1.Models.Validation
{
    public class PartValidator : IValidator<Part>
    {
        public void Validate(Part item)
        {
            var context = new ValidationContext(item);
            Validator.ValidateObject(item, context);
        }

        public async Task ValidateAsync(Part item)
        {
            Validate(item);
        }
    }
}
