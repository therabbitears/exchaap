using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(loffers.api.Startup))]

namespace loffers.api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR("/signalr", new HubConfiguration() { EnableDetailedErrors= true});
        }
    }
}
