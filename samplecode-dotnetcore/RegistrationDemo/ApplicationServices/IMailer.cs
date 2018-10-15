using RegistrationDemo.Domain;

namespace RegistrationDemo.ApplicationServices
{
    public interface IMailer
    {
        Customer SendWelcome(Customer customer);
    }
}