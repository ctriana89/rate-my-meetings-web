using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

using RMM.Web.Api;

namespace RMM.Web.Api.Tests
{
    public class FirstTestClass
    {
        [Fact]
        public void FirstTestMethod()
        {
            true.Should().BeTrue();
        }
    }
}
