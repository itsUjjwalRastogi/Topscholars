using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Topscholars.Startup))]
namespace Topscholars
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
