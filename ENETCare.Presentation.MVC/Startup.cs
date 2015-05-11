using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ENETCare.Presentation.MVC.Startup))]
namespace ENETCare.Presentation.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
