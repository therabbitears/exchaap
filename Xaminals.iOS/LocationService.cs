using System;
using CoreLocation;
using Loffers.Services.LocationServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xaminals.iOS.LocationService))]
namespace Xaminals.iOS
{
    public class LocationService : ILocationService
    {
        public bool IsLocationAvailable()
        {
            return CLLocationManager.LocationServicesEnabled;
        }

        public void OpenSettings()
        {
            UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(UIApplication.OpenSettingsUrlString));
        }
    }
}