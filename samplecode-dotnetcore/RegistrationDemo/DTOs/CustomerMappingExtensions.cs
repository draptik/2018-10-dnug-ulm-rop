using RegistrationDemo.Domain;

namespace RegistrationDemo.DTOs
{
    public static class CustomerMappingExtensions
    {
        public static Customer Map2Domain(this RegisterCustomerVm vm) => new Customer {Name = vm.Name};
    }
}