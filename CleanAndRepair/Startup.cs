using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CleanAndRepair.Startup))]
namespace CleanAndRepair
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
