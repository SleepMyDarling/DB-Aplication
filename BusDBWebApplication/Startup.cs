using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BusDBWebApplication.Startup))]
namespace BusDBWebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
