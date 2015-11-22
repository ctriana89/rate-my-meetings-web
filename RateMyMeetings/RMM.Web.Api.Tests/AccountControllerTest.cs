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
            var result = _controller.Login(null,"password");

            result.Should().BeNull();
        }

        [Fact]
        public void LoginShouldReturnNullWhenPasswordIsEmpty()
        {
            var result = _controller.Login("asdfasdf", null);

            result.Should().BeNull();
        }

        [Fact]
        public void LoginShouldReturnUserWhenUserFound()
        {
            _repository.GetUser("aUser", "aPassword").Returns(
                new User {UserName = "aUser", Password = "aPassword"});

            var user = _controller.Login("aUser", "aPassword");

            user.UserName.Should().Be("aUser");
            user.Password.Should().Be("aPassword");
        }

        [Fact]
        public void LoginShouldReturnUserWhenUserNotFound()
        {
            _repository
                .GetUser(Arg.Any<string>(), Arg.Any<string>())
                .ReturnsNull();

            var user = _controller.Login("user", "password");

            user.Should().BeNull();
        }
    }
}