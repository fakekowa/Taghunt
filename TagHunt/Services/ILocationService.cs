using TagHunt.Models;

namespace TagHunt.Services;

public interface ILocationService
{
    Task<LocationGps> GetCurrentLocationAsync();
    Task<bool> RequestLocationPermissionAsync();
    Task StartLocationUpdatesAsync();
    Task StopLocationUpdatesAsync();
    event EventHandler<LocationGps> LocationChanged;
} 