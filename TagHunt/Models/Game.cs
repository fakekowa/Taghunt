using System;
using System.Collections.Generic;

namespace TagHunt.Models
{
    /// <summary>
    /// Represents a game session in the TagHunt application
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Gets or sets the unique identifier for the game
        /// </summary>
        public string Id { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the name of the game
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the ID of the user who created the game
        /// </summary>
        public string CreatorId { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the current status of the game (waiting, active, finished)
        /// </summary>
        public string Status { get; set; } = "waiting";
        
        /// <summary>
        /// Gets or sets the date and time when the game was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Gets or sets the date and time when the game was started
        /// </summary>
        public DateTime? StartedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the date and time when the game ended
        /// </summary>
        public DateTime? EndedAt { get; set; }
        
        /// <summary>
        /// Gets or sets the list of player IDs participating in the game
        /// </summary>
        public List<string> Players { get; set; } = new();
        
        /// <summary>
        /// Gets or sets the current locations of all players in the game
        /// </summary>
        public Dictionary<string, PlayerLocation> PlayerLocations { get; set; } = new();
        
        /// <summary>
        /// Gets or sets the game configuration settings
        /// </summary>
        public GameSettings Settings { get; set; } = new();
    }

    /// <summary>
    /// Represents the configuration settings for a game
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// Gets or sets the maximum number of players allowed in the game
        /// </summary>
        public int MaxPlayers { get; set; } = 10;
        
        /// <summary>
        /// Gets or sets the duration of the game in minutes
        /// </summary>
        public int DurationMinutes { get; set; } = 30;
        
        /// <summary>
        /// Gets or sets the radius of the play area in meters
        /// </summary>
        public double PlayAreaRadius { get; set; } = 1000;
        
        /// <summary>
        /// Gets or sets the center location of the play area
        /// </summary>
        public PlayerLocation CenterLocation { get; set; } = new();
    }

    /// <summary>
    /// Represents a player's location at a specific point in time
    /// </summary>
    public class PlayerLocation
    {
        /// <summary>
        /// Gets or sets the latitude coordinate
        /// </summary>
        public double Latitude { get; set; }
        
        /// <summary>
        /// Gets or sets the longitude coordinate
        /// </summary>
        public double Longitude { get; set; }
        
        /// <summary>
        /// Gets or sets the timestamp when this location was recorded
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
} 