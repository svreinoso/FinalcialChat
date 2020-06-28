using FinalcialChat.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalcialChat.Startup))]
namespace FinalcialChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalHost.DependencyResolver.Register(typeof(MessageHub), () => new MessageHub(new ChatServices()));
            app.MapSignalR();
        }
    }
}
