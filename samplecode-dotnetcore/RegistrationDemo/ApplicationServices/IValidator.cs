using RegistrationDemo.Domain;

namespace RegistrationDemo.ApplicationServices
{
    public interface IValidator
    {
        bool IsValid(Customer customer);
    }
}