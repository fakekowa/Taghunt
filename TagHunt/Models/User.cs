using System;

namespace TagHunt.Models;

/// <summary>
/// Represents a user in the TagHunt application
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Gets or sets the user's username
    /// </summary>
    public required string Username { get; set; }
    
    /// <summary>
    /// Gets or sets the user's email address
    /// </summary>
    public required string Email { get; set; }
    
    /// <summary>
    /// Gets or sets the user's display name
    /// </summary>
    public required string DisplayName { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the user account was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time of the user's last login
    /// </summary>
    public DateTime LastLoginAt { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the user is currently online
    /// </summary>
    public bool IsOnline { get; set; }
    
    /// <summary>
    /// Gets or sets the location sharing permissions for different users or groups
    /// </summary>
    public Dictionary<string, bool> LocationSharingPermissions { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the User class with default timestamps
    /// </summary>
    public User()
    {
        CreatedAt = DateTime.UtcNow;
        LastLoginAt = DateTime.UtcNow;
    }
} 