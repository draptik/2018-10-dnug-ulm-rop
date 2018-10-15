using RegistrationDemo.DTOs;

namespace RegistrationDemo.ApplicationServices
{
    public class RegistrationService
    {
        private readonly IValidator _validator;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMailer _mailConfirmer;

        public RegistrationService(IValidator validator, ICustomerRepository customerRepository, IMailer mailer)
        {
            _validator = validator;
            _customerRepository = customerRepository;
            _mailConfirmer = mailer;
        }

        public CustomerRegisteredVm RegisterCustomer(RegisterCustomerVm vm)
        {
            //return RegisterCustomerOldSchoolHappyCase(vm);
            return RegisterCustomerOldSchoolWithErrorHandling(vm);
        }

        private CustomerRegisteredVm RegisterCustomerOldSchoolHappyCase(RegisterCustomerVm vm)
        {
            var customer = vm.Map2Domain();
            var isValid = _validator.IsValid(customer);
            if (isValid)
            {
                customer = _customerRepository.Save(customer);
                _mailConfirmer.SendWelcome(customer);
                return new CustomerRegisteredVm{CustomerId = customer.Id};
            }
            return new CustomerRegisteredVm{ErrorMessage = "ups"};
        }

        private CustomerRegisteredVm RegisterCustomerOldSchoolWithErrorHandling(RegisterCustomerVm vm)
        {
            var customer = vm.Map2Domain();
            var isValid = _validator.IsValid(customer);
            if (isValid)
            {
                try
                {
                    customer = _customerRepository.Save(customer);
                }
                catch (System.Exception e)
                {
                    return new CustomerRegisteredVm {ErrorMessage = e.Message};
                }
                try
                {
                    _mailConfirmer.SendWelcome(customer);
                }
                catch (System.Exception e)
                {
                    return new CustomerRegisteredVm {ErrorMessage = e.Message};
                }
                
                // Happy case
                return new CustomerRegisteredVm{CustomerId = customer.Id};
            }

            // Failure case
            return new CustomerRegisteredVm{ErrorMessage = "Name is empty!"};
        }
    }
}