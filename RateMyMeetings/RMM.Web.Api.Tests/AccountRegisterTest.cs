using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using RMM.Web.Api.Models;
using RMM.Web.Api.Services;
using Xunit;

namespace RMM.Web.Api.Tests
{
    public class AccountRegisterTest
    {
        private AccountController _controller;
        private IUserRepository _repository;
        private IEmailValidator _validator;

        public AccountRegisterTest()
        {
            _repository = Substitute.For<IUserRepository>();
            _validator = Substitute.For<IEmailValidator>();

            _controller=new AccountController(_repository, _validator);
        }

        [Fact]
        public void RegisterShouldReturnBadRequestWhenCompanyNameIsEmpty()
        {
            var result = _controller.Register(null, "email@test.com", "password");

            result.As<BadRequestErrorMessageResult>()
                .Message
                .Should()
                .Be("Company Name, Email or Password must be not empty");
        }

        [Fact]
        public void RegisterShouldReturnBadRequestWhenEmailIsEmpty()
        {
            var result = _controller.Register("companyName", null, "password");
            result.As<BadRequestErrorMessageResult>()
                .Message
                .Should()
                .Be("Company Name, Email or Password must be not empty");
        }

        [Fact]
        public void RegisterShouldReturnBadRequestWhenPasswordIsEmpty()
        {
            var result = _controller.Register("companyName", "email@test.com", null);
            result.As<BadRequestErrorMessageResult>()
                .Message
                .Should()
                .Be("Company Name, Email or Password must be not empty");
        }

        [Fact]
        public void RegisterShouldReturnBadRequestWhenEmailIsInvalid()
        {
            IHttpActionResult result = _controller.Register("companyName", "email.test.com", "password");

            result.As<BadRequestErrorMessageResult>()
                .Message
                .Should()
                .Be("Invalid Email");
        }

        [Fact]
        public void RegisterShouldReturnBadRequest()
        {
            _repository.GetUser(Arg.Any<string>())
                .Returns(new User());
            _validator.IsValidEmail(Arg.Any<string>())
                .Returns(true);

            var respose = _controller.Register("aCompany", "aEmail", "aPass")
                .As<BadRequestErrorMessageResult>();

            respose.Message.Should().Be("Existing User");
        }

        [Fact]
        public void RegisterShouldReturnOkWhenRegisteringANewUser()
        {
            _repository.GetUser(Arg.Any<string>())
               .ReturnsNull();
            _repository.CreateUser("aUser", "company", "pass")
                .Returns(new User {UserName = "aUser"});

            _validator.IsValidEmail(Arg.Any<string>())
                .Returns(true);

            var message = _controller.Register("company", "aUser", "pass")
                .As<OkNegotiatedContentResult<User>>();

            message.Content.UserName.Should().Be("aUser");
        }
    }
}