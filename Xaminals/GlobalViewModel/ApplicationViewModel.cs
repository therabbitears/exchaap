using Microsoft.AspNet.SignalR.Client;
using Xaminals.ViewModels;

namespace Loffers.GlobalViewModel
{
    public partial class ApplicationViewModel : BaseViewModel
    {
        public HubConnection hubConnection;
        public IHubProxy chatHubProxy;

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
        }
    }
}
