using CSharpFunctionalExtensions;
using RopDemo.Domain;

namespace RopDemo.ApplicationServices
{
    public interface IValidator
    {
        bool IsValidClassic(Customer customer);
        Result<Customer> IsValid(Customer customer);
    }
}