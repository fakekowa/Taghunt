using TagHunt.Models;

namespace TagHunt.Services;

/// <summary>
/// Interface for location services that handle GPS location tracking and permissions
/// </summary>
public interface ILocationService
{
    /// <summary>
    /// Gets the current GPS location of the device
    /// </summary>
    /// <returns>The current location as LocationGps</returns>
    Task<LocationGps> GetCurrentLocationAsync();
    
    /// <summary>
    /// Requests location permission from the user
    /// </summary>
    /// <returns>True if permission was granted, false otherwise</returns>
    Task<bool> RequestLocationPermissionAsync();
    
    /// <summary>
    /// Starts continuous location updates
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    Task StartLocationUpdatesAsync();
    
    /// <summary>
    /// Stops continuous location updates
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    Task StopLocationUpdatesAsync();
    
    /// <summary>
    /// Event that fires when the device location changes
    /// </summary>
    event EventHandler<LocationGps> LocationChanged;
} 