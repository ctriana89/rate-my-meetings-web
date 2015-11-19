using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RMM.Web.Startup))]
namespace RMM.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
