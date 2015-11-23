using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Results;
using RMM.Web.Api.Models;
using RMM.Web.Api.Services;

namespace RMM.Web.Api
{
    public class AccountController : ApiController
    {
        private readonly IUserRepository _repository;

        public AccountController(IUserRepository repository)
        {
            this._repository = repository;
        }

        public IHttpActionResult Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return this.BadRequest("Username and password must be non empty");
            }

            return Ok(_repository.GetUser(userName, password));
        }

        public IHttpActionResult Register(string companyName, string email, string password)
        {
            if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return this.BadRequest("Company Name, Email or Password must be not empty");
            }

            if (IsValidEmail(email))
            {
                return this.BadRequest("not implemented");
            }

            return this.BadRequest("Invalid Email");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
    }
}
