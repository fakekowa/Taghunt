using System;

namespace TagHunt.Models;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
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