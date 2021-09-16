using System.Threading.Tasks;

namespace WebApplication1.Models.Validation
{
    public interface IValidator<T>
    {
        Task ValidateAsync(T item);
        void Validate(T item);
    }
}
