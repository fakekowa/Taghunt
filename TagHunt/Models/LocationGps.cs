namespace TagHunt.Models;

/// <summary>
/// Represents GPS location data with coordinates and metadata
/// </summary>
public class LocationGps
{
    /// <summary>
    /// Gets or sets the latitude coordinate in decimal degrees
    /// </summary>
    public double Latitude { get; set; }
    
    /// <summary>
    /// Gets or sets the longitude coordinate in decimal degrees
    /// </summary>
    public double Longitude { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp when this location was recorded
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Gets or sets the accuracy of the GPS reading in meters (optional)
    /// </summary>
    public double? Accuracy { get; set; }
} 