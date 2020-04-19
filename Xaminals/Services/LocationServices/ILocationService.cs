namespace Loffers.Services.LocationServices
{
    public interface ILocationService
    {
        bool IsLocationAvailable();
        void OpenSettings();
    }
}
