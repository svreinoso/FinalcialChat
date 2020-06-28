using Autofac;
using Autofac.Integration.Mvc;
using FinalcialChat.Interfaces;
using FinalcialChat.Services;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FinalcialChat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<ChatServices>().As<IChatServices>();
            builder.RegisterType<RoomServices>().As<IRoomServices>();
            builder.RegisterType<HttpClientManager>().As<IHttpClientManager>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
