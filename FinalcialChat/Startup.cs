using Autofac;
using FinalcialChat.Interfaces;
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

            var builder = new ContainerBuilder();
            builder.RegisterType<HttpClientManager>().As<IHttpClientManager>();
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IHttpClientManager>();
                GlobalHost.DependencyResolver.Register(typeof(MessageHub), () => new MessageHub(new ChatServices(service)));
                app.MapSignalR();
            }

        }
    }
}
