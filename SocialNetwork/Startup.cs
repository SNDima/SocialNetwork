using Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartupAttribute(typeof(SocialNetwork.Startup))]
namespace SocialNetwork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // By default, this is IPrincipal.Identity.Name, but this can be changed 
            // by registering an implementation of IUserIdProvider with the global host.
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new CustomUserIdProvider());

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.User.Identity.GetUserId();
        }
    }
}