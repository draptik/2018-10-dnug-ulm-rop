using System;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using RopDemo.ApplicationServices;
using RopDemo.Domain;
using RopDemo.DTOs;
using Xunit;

namespace RopDemo.Tests
{
    public class RegistrationServiceClassicTests
    {
        private static ICustomerRepository _customerRepository;
        private static IMailer _mailer;
        private static IValidator _validator;

        private static RegistrationServiceClassic DefaultSetup(Customer customer)
        {
            _validator = Substitute.For<IValidator>();
            _validator.IsValidClassic(Arg.Any<Customer>()).Returns(true);

            _customerRepository = Substitute.For<ICustomerRepository>();
            _customerRepository.SaveClassic(Arg.Any<Customer>()).Returns(customer);

            _mailer = Substitute.For<IMailer>();
            _mailer.SendWelcomeClassic(Arg.Any<Customer>()).Returns(customer);

            var sut = new RegistrationServiceClassic(_validator, _customerRepository, _mailer);
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

            _customerRepository.SaveClassic(Arg.Any<Customer>())
                .Throws(new Exception("db error"));

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("db error");
        }

        [Fact]
        public void Registering_a_new_customer_with_mail_failure_returns_error_message()
        {
            // Arrange
            var customer = new Customer {Id = 42};
            var sut = DefaultSetup(customer);

            _mailer.SendWelcomeClassic(Arg.Any<Customer>())
                .Throws(new Exception("mail error"));

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("mail error");
        }

        [Fact]
        public void Registering_a_new_customer_with_invalid_customer_returns_error_message()
        {
            // Arrange
            var customer = new Customer {Id = 42};

            var sut = DefaultSetup(customer);

            _validator.IsValidClassic(Arg.Any<Customer>()).Returns(false);

            // Act
            var result = sut.RegisterCustomer(new RegisterCustomerVm());

            // Assert
            result.ErrorMessage.Should().Be("Name is empty!");
        }
    }
}