using System;
using System.Collections.Generic;

namespace TagHunt.Models
{
    public class Game
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
        public string Status { get; set; } = "waiting";  // waiting, active, finished
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public List<string> Players { get; set; } = new();
        public Dictionary<string, PlayerLocation> PlayerLocations { get; set; } = new();
        public GameSettings Settings { get; set; } = new();
    }

    public class GameSettings
    {
        public int MaxPlayers { get; set; } = 10;
        public int DurationMinutes { get; set; } = 30;
        public double PlayAreaRadius { get; set; } = 1000;  // meters
        public PlayerLocation CenterLocation { get; set; } = new();
    }

    public class PlayerLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
} 