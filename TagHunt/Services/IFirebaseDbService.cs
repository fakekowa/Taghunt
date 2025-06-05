using TagHunt.Models;

namespace TagHunt.Services.Interfaces;

/// <summary>
/// Interface for Firebase database services that handle game and player data operations
/// </summary>
public interface IFirebaseDbService
{
    /// <summary>
    /// Creates a new game in the database
    /// </summary>
    /// <param name="game">The game to create</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> CreateGameAsync(Game game);
    
    /// <summary>
    /// Updates an existing game in the database
    /// </summary>
    /// <param name="game">The game to update</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> UpdateGameAsync(Game game);
    
    /// <summary>
    /// Retrieves a game by its ID
    /// </summary>
    /// <param name="gameId">The ID of the game to retrieve</param>
    /// <returns>The game if found, null otherwise</returns>
    Task<Game?> GetGameAsync(string gameId);
    
    /// <summary>
    /// Retrieves all active games from the database
    /// </summary>
    /// <returns>List of active games</returns>
    Task<List<Game>> GetActiveGamesAsync();
    
    /// <summary>
    /// Deletes a game from the database
    /// </summary>
    /// <param name="gameId">The ID of the game to delete</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> DeleteGameAsync(string gameId);
    
    /// <summary>
    /// Adds a player to a game
    /// </summary>
    /// <param name="gameId">The ID of the game to join</param>
    /// <param name="userId">The ID of the user joining the game</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> JoinGameAsync(string gameId, string userId);
    
    /// <summary>
    /// Removes a player from a game
    /// </summary>
    /// <param name="gameId">The ID of the game to leave</param>
    /// <param name="userId">The ID of the user leaving the game</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> LeaveGameAsync(string gameId, string userId);
    
    /// <summary>
    /// Updates a player's location in a game
    /// </summary>
    /// <param name="gameId">The ID of the game</param>
    /// <param name="userId">The ID of the user</param>
    /// <param name="location">The new location data</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> UpdatePlayerLocationAsync(string gameId, string userId, PlayerLocation location);
    
    /// <summary>
    /// Retrieves all player locations for a game
    /// </summary>
    /// <param name="gameId">The ID of the game</param>
    /// <returns>List of player locations</returns>
    Task<List<PlayerLocation>> GetPlayerLocationsAsync(string gameId);
} 