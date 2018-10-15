using RegistrationDemo.Domain;

namespace RegistrationDemo.ApplicationServices
{
    public interface ICustomerRepository
    {
        Customer Save(Customer customer);
    }
}