﻿using System;
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
        private readonly IEmailValidator _emailValidator;

        public AccountController(IUserRepository repository, IEmailValidator validor)
        {
            this._repository = repository;
            _emailValidator = validor;
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

            if (_emailValidator.IsValidEmail(email))
            {
                User user = _repository.GetUser(email);

                if (user != null)
                {
                    return BadRequest("Existing User");
                }

                return Ok(_repository.CreateUser(email, companyName, password));
            }

            return this.BadRequest("Invalid Email");
        }
    }
}
