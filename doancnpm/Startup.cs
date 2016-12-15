using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(doancnpm.Startup))]
namespace doancnpm
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
