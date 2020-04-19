using Loffers.Services.LocationServices;
using System;
using System.Diagnostics;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xaminals.UWP.LocationService))]
namespace Xaminals.UWP
{
    public class LocationService : ILocationService
    {
        public bool IsLocationAvailable()
        {
            return true;
        }

        public void OpenSettings()
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
        }
    }
}
