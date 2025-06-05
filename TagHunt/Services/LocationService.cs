using TagHunt.Models;

namespace TagHunt.Services;

public class LocationService : ILocationService
{
    public event EventHandler<LocationGps> LocationChanged;

    public async Task<LocationGps> GetCurrentLocationAsync()
    {
        // TODO: Implement using MAUI's Geolocation
        throw new NotImplementedException();
    }

    public async Task<bool> RequestLocationPermissionAsync()
    {
        // TODO: Implement using MAUI's Permissions
        throw new NotImplementedException();
    }

    public async Task StartLocationUpdatesAsync()
    {
        // TODO: Implement using MAUI's Geolocation
        throw new NotImplementedException();
    }

    public async Task StopLocationUpdatesAsync()
    {
        // TODO: Implement using MAUI's Geolocation
        throw new NotImplementedException();
    }

    protected virtual void OnLocationChanged(LocationGps location)
    {
        LocationChanged?.Invoke(this, location);
    }
} 