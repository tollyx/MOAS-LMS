using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MOAS_LMS.Startup))]
namespace MOAS_LMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
