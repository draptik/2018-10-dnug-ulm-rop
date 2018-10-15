using System;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RegistrationDemo.ApplicationServices;
using RegistrationDemo.Domain;
using RegistrationDemo.DTOs;
using Xunit;

namespace RegistrationDemo.Tests
{
    public class RegistrationServiceTests
    {
        private static ICustomerRepository _customerRepository;
        private static IMailer _mailer;
        private static IValidator _validator;

        public RegistrationServiceTests()
        {
            _validator = Substitute.For<IValidator>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _mailer = Substitute.For<IMailer>();
        }

        private static RegistrationService DefaultSetup(Customer customer)
        {
            _validator.IsValid(Arg.Any<Customer>()).Returns(true);
            _customerRepository.Save(Arg.Any<Customer>()).Returns(customer);
            _mailer.SendWelcome(Arg.Any<Customer>()).Returns(customer);

            var sut = new RegistrationService(_validator, _customerRepository, _mailer);
            return sut;
        }

        [Fact]
        public void Registering_a_new_customer_returns_correct_id()
        {
            // Arrange
            var customer = new Customer {Id = 42};

            var sut = DefaultSetup(customer);

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.CustomerId.Should().Be(42);
        }

        [Fact]
        public void Registering_a_new_customer_with_db_failure_returns_error_message()
        {
            // Arrange
            var customer = new Customer {Id = 42};
            var sut = DefaultSetup(customer);

            _customerRepository.Save(Arg.Any<Customer>())
                .Throws(new Exception("db error"));

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("db error");
        }

        [Fact]
        public void Registering_a_new_customer_with_invalid_customer_returns_error_message()
        {
            // Arrange
            var customer = new Customer {Id = 42};

            var sut = DefaultSetup(customer);

            _validator.IsValid(Arg.Any<Customer>()).Returns(false);

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("Name is empty!");
        }

        [Fact]
        public void Registering_a_new_customer_with_mail_failure_returns_error_message()
        {
            // Arrange
            var customer = new Customer {Id = 42};
            var sut = DefaultSetup(customer);

            _mailer.SendWelcome(Arg.Any<Customer>())
                .Throws(new Exception("mail error"));

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("mail error");
        }
    }
}