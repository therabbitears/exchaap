using exchaup.Models;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace exchaup.Views.Home.Model
{
    public partial class StartupScreenCardsViewModel
    {
        public ICommand GoToAppCommand { get; set; }
        public ICommand CurrentItemCommand { get; set; }
        public ICommand SaveDataCoammnd { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            GoToAppCommand = new Command(async (object sender) => await ExecureGoCommand(null));
            SaveDataCoammnd = new Command((object sender) => ExecureSaveDataCoammnd(null));
            CurrentItemCommand = new Command(async (object sender) => await ExecureCurrentItemCommand(null));
            SaveDataCoammnd.Execute(null);
            RecordAnalyticsEventCommand.Execute(AnalyticsModel.InstanceOf(AnalyticsModel.EventNames.PageViewEvent, AnalyticsModel.ParameterNames.PageName, "startupcarousel"));
        }

        public async Task ExecureGoCommand(object p)
        {
            MessagingCenter.Send<StartupScreenCardsViewModel>(this, "GotoApp");
        }

        public async void ExecureSaveDataCoammnd(object p)
        {
            await Database.SaveLastState(new ApplicationStateModel() { SkipIntro = true });
        }

        public async Task ExecureCurrentItemCommand(object p)
        {
            if (p is StartupCardModel value)
            {

            }
        }
    }
}
