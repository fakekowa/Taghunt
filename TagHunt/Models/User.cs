using System;

namespace TagHunt.Models;

public class User
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public bool IsOnline { get; set; }
    public Dictionary<string, bool> LocationSharingPermissions { get; set; } = new();

    public User()
    {
        CreatedAt = DateTime.UtcNow;
        LastLoginAt = DateTime.UtcNow;
    }
} 