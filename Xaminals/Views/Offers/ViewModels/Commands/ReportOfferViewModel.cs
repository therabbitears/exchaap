using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Common.Alerts;

namespace Xaminals.Views.Offers.ViewModels
{
    public partial class ReportOfferViewModel
    {
        public ICommand ReportCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            ReportCommand = new Command(async () => await ExecuteReportCommand());
        }

        private async Task ExecuteReportCommand()
        {
            IsBusy = true;

            try
            {
                var result = await new RestService().Report<HttpResult<object>>(new
                {
                    this.Offer.Id,
                    this.Comment,
                    Options = this.Options.Where(c => c.Selected).Select(d => new { d.Text })
                });

                if (!result.IsError)
                {
                    await ExecuteCancelPopupCommand();
                    await RaiseSuccess("This offer has been reported and we will review it soon.");
                }
                else
                    await RaiseError(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while reporting offer.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
