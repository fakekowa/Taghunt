using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Maui.Devices.Sensors;
using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services;

public class LocationService : ILocationService
{
    private readonly IGeolocation _geolocation;
    private readonly System.Timers.Timer _timer;
    private bool _isTracking;

    public event EventHandler<LocationGps> LocationChanged = delegate { };

    public LocationService(IGeolocation geolocation)
    {
        _geolocation = geolocation;
        _timer = new System.Timers.Timer(10000); // 10 seconds
        _timer.Elapsed += async (s, e) => await UpdateLocationAsync();
    }

    public async Task<bool> RequestLocationPermissionAsync()
    {
        try
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status == PermissionStatus.Granted;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<LocationGps> GetCurrentLocationAsync()
    {
        try
        {
            var location = await _geolocation.GetLocationAsync();
            if (location == null)
            {
                throw new InvalidOperationException("Could not get current location");
            }

            return new LocationGps
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Accuracy = location.Accuracy,
                Timestamp = location.Timestamp.DateTime
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Could not get current location", ex);
        }
    }

    public async Task StartLocationUpdatesAsync()
    {
        if (_isTracking)
        {
            return;
        }

        _isTracking = true;
        _timer.Start();
        await Task.CompletedTask;
    }

    public async Task StopLocationUpdatesAsync()
    {
        if (!_isTracking) return;

        _isTracking = false;
        _timer.Stop();
        await Task.CompletedTask;
    }

    private async Task UpdateLocationAsync()
    {
        try
        {
            var location = await GetCurrentLocationAsync();
            LocationChanged?.Invoke(this, location);
        }
        catch
        {
            // Handle or log error as needed
        }
    }
} 