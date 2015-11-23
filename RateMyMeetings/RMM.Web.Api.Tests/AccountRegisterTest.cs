using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using NSubstitute;
using RMM.Web.Api.Services;
using Xunit;

namespace RMM.Web.Api.Tests
{
    public class AccountRegisterTest
    {
        private AccountController _controller;
        private IUserRepository _repository;

        public AccountRegisterTest()
        {
            _repository = Substitute.For<IUserRepository>();

            _controller=new AccountController(_repository);
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
    }
}