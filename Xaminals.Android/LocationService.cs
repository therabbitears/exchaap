using Android.Content;
using Android.Locations;
using Loffers.Services.LocationServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xaminals.Droid.LocationService))]
namespace Xaminals.Droid
{
    public class LocationService : ILocationService
    {
        public void OpenSettings()
        {
            var locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                Forms.Context.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
            }
            else
            {
                //this is handled in the PCL
            }
        }

        public bool IsLocationAvailable() 
        {
            var locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }
    }
}