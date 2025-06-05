using System;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;
using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services;

public class LocationService : ILocationService
{
    private readonly IGeolocation _geolocation;
    private readonly ITimer _timer;
    private bool _isTracking;

    public event EventHandler<PlayerLocation> LocationChanged = delegate { };

    public LocationService(IGeolocation geolocation, ITimer timer)
    {
        _geolocation = geolocation;
        _timer = timer;
    }

    public async Task<bool> RequestPermissionsAsync()
    {
        try
        {
            var status = await _geolocation.RequestPermissionAsync();
            return status == PermissionStatus.Granted;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<PlayerLocation?> GetCurrentLocationAsync()
    {
        try
        {
            var location = await _geolocation.GetLocationAsync();
            if (location == null) return null;

            return new PlayerLocation
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task StartTrackingAsync()
    {
        if (_isTracking) return;

        _isTracking = true;
        _timer.Interval = TimeSpan.FromSeconds(10);
        _timer.Elapsed += async (s, e) => await UpdateLocationAsync();
        _timer.Start();
    }

    public async Task StopTrackingAsync()
    {
        if (!_isTracking) return;

        _isTracking = false;
        _timer.Stop();
    }

    private async Task UpdateLocationAsync()
    {
        var location = await GetCurrentLocationAsync();
        if (location != null)
        {
            LocationChanged?.Invoke(this, location);
        }
    }
} 