using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigFatDiary.Startup))]
namespace BigFatDiary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
