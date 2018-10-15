using CSharpFunctionalExtensions;
using FluentAssertions;
using NSubstitute;
using RopDemo.ApplicationServices;
using RopDemo.Domain;
using RopDemo.DTOs;
using Xunit;

namespace RopDemo.Tests
{
    public class RegistrationServiceRopTests
    {
        private static IValidator _validator;
        private static ICustomerRepository _customerRepository;
        private static IMailer _mailer;

        public RegistrationServiceRopTests()
        {
            _validator = Substitute.For<IValidator>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _mailer = Substitute.For<IMailer>();
        }

        private static RegistrationServiceRop DefaultSetup(Result<Customer> customerResult)
        {
            _validator.IsValid(Arg.Any<Customer>()).Returns(customerResult);
            _customerRepository.Save(Arg.Any<Customer>()).Returns(customerResult);
            _mailer.SendWelcome(Arg.Any<Customer>()).Returns(customerResult);

            var sut = new RegistrationServiceRop(_validator, _customerRepository, _mailer);
            return sut;
        }
        
        [Fact]
        public void Registering_a_new_customer_returns_correct_id()
        {
            // Arrange
            var sut = DefaultSetup(Result.Ok(new Customer {Id = 42}));

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.CustomerId.Should().Be(42);
        }

        [Fact]
        public void Registering_a_new_customer_with_db_failure_returns_error_message()
        {
            // Arrange
            var sut = DefaultSetup(Result.Ok(new Customer {Id = 42}));

            _customerRepository.Save(Arg.Any<Customer>())
                .Returns(Result.Fail<Customer, RopError>(
                    new RopError {Message = "db error"}));

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("db error");
        }

        [Fact]
        public void Registering_a_new_customer_with_mail_failure_returns_error_message()
        {
            // Arrange
            var sut = DefaultSetup(Result.Ok(new Customer {Id = 42}));

            _mailer.SendWelcome(Arg.Any<Customer>())
                .Returns(Result.Fail<Customer, RopError>(
                    new RopError {Message = "mail error"}));

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("mail error");
        }

        [Fact]
        public void Registering_a_new_customer_with_invalid_customer_returns_error_message()
        {
            // Arrange
            var sut = DefaultSetup(Result.Ok(new Customer {Id = 42}));

            _validator.IsValid(Arg.Any<Customer>())
                .Returns(Result.Fail<Customer>("Name is empty!"));
            
            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("Name is empty!");
        }
    }
}