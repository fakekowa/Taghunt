using TagHunt.Models;

namespace TagHunt.Services.Interfaces
{
    public interface ILocationService
    {
        event EventHandler<LocationGps> LocationChanged;
        Task<bool> RequestLocationPermissionAsync();
        Task<LocationGps> GetCurrentLocationAsync();
        Task StartLocationUpdatesAsync();
        Task StopLocationUpdatesAsync();
    }
} 