using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cap24Team3.Startup))]
namespace Cap24Team3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
