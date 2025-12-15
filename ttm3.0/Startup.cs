using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ttm3._0.Startup))]
namespace ttm3._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
