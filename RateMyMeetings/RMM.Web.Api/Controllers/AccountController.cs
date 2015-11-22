using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RMM.Web.Api.Models;
using RMM.Web.Api.Services;

namespace RMM.Web.Api
{
    public class AccountController : ApiController
    {
        private IUserRepository _repository;

        public AccountController(IUserRepository repository)
        {
            this._repository = repository;
        }

        public User Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            return _repository.GetUser(userName, password);
        }
    }
}
