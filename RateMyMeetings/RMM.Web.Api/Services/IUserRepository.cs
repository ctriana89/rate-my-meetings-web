using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMM.Web.Api.Models;

namespace RMM.Web.Api.Services
{
    public interface IUserRepository
    {
        User GetUser(string user, string password);

        User GetUser(string user);
        User CreateUser(string user, string company, string password);
    }
}
