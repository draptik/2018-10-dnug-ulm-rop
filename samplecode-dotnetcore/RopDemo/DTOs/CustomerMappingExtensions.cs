using RopDemo.Domain;

namespace RopDemo.DTOs
{
    public static class CustomerMappingExtensions
    {
        public static Customer Map2Domain(this RegisterCustomerVm vm) => new Customer {Name = vm.Name};
    }
}