using System;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Maui.Devices.Sensors;
using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services;

/// <summary>
/// Service that provides GPS location tracking and management functionality
/// </summary>
public class LocationService : ILocationService
{
    #region Fields

    private readonly IGeolocation _geolocation;
    private readonly System.Timers.Timer _timer;
    private bool _isTracking;

    #endregion

    #region Events

    /// <summary>
    /// Event that fires when the device location changes during tracking
    /// </summary>
    public event EventHandler<LocationGps> LocationChanged = delegate { };

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the LocationService
    /// </summary>
    /// <param name="geolocation">The geolocation service to use for GPS operations</param>
    public LocationService(IGeolocation geolocation)
    {
        _geolocation = geolocation;
        _timer = new System.Timers.Timer(10000); // Update every 10 seconds
        _timer.Elapsed += async (s, e) => await UpdateLocationAsync();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Requests location permission from the user
    /// </summary>
    /// <returns>True if permission was granted, false otherwise</returns>
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

    /// <summary>
    /// Gets the current GPS location of the device
    /// </summary>
    /// <returns>The current location as LocationGps</returns>
    /// <exception cref="InvalidOperationException">Thrown when location cannot be obtained</exception>
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

    /// <summary>
    /// Starts continuous location updates using a timer
    /// </summary>
    /// <returns>Task representing the async operation</returns>
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

    /// <summary>
    /// Stops continuous location updates
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    public async Task StopLocationUpdatesAsync()
    {
        if (!_isTracking)
        {
            return;
        }

        _isTracking = false;
        _timer.Stop();
        await Task.CompletedTask;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the current location and fires the LocationChanged event
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    private async Task UpdateLocationAsync()
    {
        try
        {
            var location = await GetCurrentLocationAsync();
            LocationChanged?.Invoke(this, location);
        }
        catch
        {
            // Handle or log error as needed - silently fail to avoid disrupting tracking
        }
    }

    #endregion
} 