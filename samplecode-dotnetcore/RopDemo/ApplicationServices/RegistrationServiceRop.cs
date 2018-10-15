using System;
using System.Linq;
using CSharpFunctionalExtensions;
using RopDemo.Domain;
using RopDemo.DTOs;

namespace RopDemo.ApplicationServices
{
    public class RegistrationServiceRop
    {
        private readonly IValidator _validator;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMailer _mailConfirmer;

        public RegistrationServiceRop(IValidator validator, ICustomerRepository customerRepository, IMailer mailer)
        {
            _validator = validator;
            _customerRepository = customerRepository;
            _mailConfirmer = mailer;
        }

        public CustomerRegisteredVm RegisterCustomer(RegisterCustomerVm vm)
        {
            return RegisterCustomerRop(vm);    
            //return RegisterCustomerRopWithExtractedMethod(vm);    
        }

        private CustomerRegisteredVm RegisterCustomerRop(RegisterCustomerVm vm)
        {
            var customer = vm.Map2Domain();
            var result = _validator.IsValid(customer)
                .OnSuccess(c => _customerRepository.Save(c))
                .OnSuccess(c => _mailConfirmer.SendWelcome(c))
                .OnBoth(cEnd => cEnd.IsSuccess
                    ? new CustomerRegisteredVm {CustomerId = cEnd.Value.Id}
                    : new CustomerRegisteredVm {ErrorMessage = cEnd.Error});
            return result;
        }

        private CustomerRegisteredVm RegisterCustomerRopWithExtractedMethod(RegisterCustomerVm vm)
        {
            var customer = vm.Map2Domain();
            var result = _validator.IsValid(customer)
                .OnSuccess(CombinedSavingAndSending)
                .OnBoth(cEnd => cEnd.IsSuccess
                    ? new CustomerRegisteredVm {CustomerId = cEnd.Value.Id}
                    : new CustomerRegisteredVm {ErrorMessage = cEnd.Error});
            return result;
        }

        private Result<Customer> CombinedSavingAndSending(Customer c) 
            => Result.Ok(c)
                .OnSuccess(x => _customerRepository.Save(x))
                .OnSuccess(x => _mailConfirmer.SendWelcome(x));

        private string RegisterCustomerRopWithString(RegisterCustomerVm vm)
        {
            var customer = vm.Map2Domain();

            var result = _validator.IsValid(customer)
                .OnSuccess(c => _customerRepository.Save(c))
                .OnSuccess(c => _mailConfirmer.SendWelcome(c))
                .OnBoth(cEnd => cEnd.IsSuccess
                    ? cEnd.Value.Id.ToString()
                    : cEnd.Error);
            
            return result;
        }
    }
}