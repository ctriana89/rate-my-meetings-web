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
    public class AccountControllerTest
    {
        private IUserRepository _repository;
        private AccountController _controller;
        public AccountControllerTest()
        {
            _repository = Substitute.For<IUserRepository>();

            _controller = new AccountController(_repository);    
        }

        [Fact]
        public void LoginShouldReturnNullWhenUserNameIsEmpty()
        {
            var result = _controller.Login(null, "password").As<BadRequestErrorMessageResult>();

            result.Message.Should().Be("Username and password must be non empty");
        }

        [Fact]
        public void LoginShouldReturnNullWhenPasswordIsEmpty()
        {
            var result = _controller.Login("asdfasdf", null).As<BadRequestErrorMessageResult>();

            result.Message.Should().Be("Username and password must be non empty");
        }

        [Fact]
        public void LoginShouldReturnUserWhenUserFound()
        {
            _repository.GetUser("aUser", "aPassword").Returns(
                new User {UserName = "aUser", Password = "aPassword"});

            IHttpActionResult message = _controller.Login("aUser", "aPassword");
            var user = message.As<OkNegotiatedContentResult<User>>().Content;

            user.UserName.Should().Be("aUser");
            user.Password.Should().Be("aPassword");
        }

        [Fact]
        public void LoginShouldReturnUserWhenUserNotFound()
        {
            _repository
                .GetUser(Arg.Any<string>(), Arg.Any<string>())
                .ReturnsNull();

            IHttpActionResult message = _controller.Login("aUser", "aPassword");

            User user = message.As<OkNegotiatedContentResult<User>>().Content;

            user.Should().BeNull();
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
        public void SomeTest()
        {
            true.Should().BeTrue();
        }
    }
}