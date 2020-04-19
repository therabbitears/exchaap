using Xaminals.ViewModels;

namespace Xaminals.Views.Common.Alerts.ViewModels
{
    public class AlertViewModel : BaseViewModel
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged("Message"); }
        }

    }
}
