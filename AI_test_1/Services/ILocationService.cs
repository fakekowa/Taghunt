using AI_test_1.Models;

namespace AI_test_1.Services;

public interface ILocationService
{
    Task<LocationGps> GetCurrentLocationAsync();
    Task<bool> RequestLocationPermissionAsync();
    Task StartLocationUpdatesAsync();
    Task StopLocationUpdatesAsync();
    event EventHandler<LocationGps> LocationChanged;
} 