namespace TagHunt.Models;

/// <summary>
/// Represents a location sharing request between users
/// </summary>
public class SharingRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the sharing request
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the user who made the sharing request
    /// </summary>
    public required string RequesterId { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the user who will receive the sharing request
    /// </summary>
    public required string RecipientId { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the request was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the request expires (optional)
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the request has been accepted
    /// </summary>
    public bool IsAccepted { get; set; }
    
    /// <summary>
    /// Gets or sets the last known location of the requester
    /// </summary>
    public required LocationGps LastLocation { get; set; }
} 