using Firebase.Database;
using Firebase.Database.Query;
using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services;

/// <summary>
/// Firebase implementation of the database service for game and player data management
/// </summary>
public class FirebaseDbService : IFirebaseDbService
{
    #region Fields

    private readonly FirebaseClient _database;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the FirebaseDbService
    /// </summary>
    /// <param name="databaseUrl">The Firebase database URL</param>
    public FirebaseDbService(string databaseUrl)
    {
        _database = new FirebaseClient(databaseUrl);
    }

    #endregion

    #region Game Management

    /// <summary>
    /// Creates a new game in the database
    /// </summary>
    /// <param name="game">The game to create</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> CreateGameAsync(Game game)
    {
        try
        {
            await _database
                .Child("games")
                .Child(game.Id)
                .PutAsync(game);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Updates an existing game in the database
    /// </summary>
    /// <param name="game">The game to update</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> UpdateGameAsync(Game game)
    {
        try
        {
            await _database
                .Child("games")
                .Child(game.Id)
                .PutAsync(game);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Retrieves a game by its ID
    /// </summary>
    /// <param name="gameId">The ID of the game to retrieve</param>
    /// <returns>The game if found, null otherwise</returns>
    public async Task<Game?> GetGameAsync(string gameId)
    {
        try
        {
            return await _database
                .Child("games")
                .Child(gameId)
                .OnceSingleAsync<Game>();
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Retrieves all active games from the database
    /// </summary>
    /// <returns>List of active games</returns>
    public async Task<List<Game>> GetActiveGamesAsync()
    {
        try
        {
            var games = await _database
                .Child("games")
                .OrderBy("status")
                .EqualTo("active")
                .OnceAsync<Game>();

            return games.Select(g => g.Object).ToList();
        }
        catch (Exception)
        {
            return new List<Game>();
        }
    }

    /// <summary>
    /// Deletes a game from the database
    /// </summary>
    /// <param name="gameId">The ID of the game to delete</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> DeleteGameAsync(string gameId)
    {
        try
        {
            await _database
                .Child("games")
                .Child(gameId)
                .DeleteAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion

    #region Player Management

    /// <summary>
    /// Adds a player to a game
    /// </summary>
    /// <param name="gameId">The ID of the game to join</param>
    /// <param name="userId">The ID of the user joining the game</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> JoinGameAsync(string gameId, string userId)
    {
        try
        {
            var game = await GetGameAsync(gameId);
            if (game == null)
            {
                return false;
            }

            game.Players.Add(userId);
            return await UpdateGameAsync(game);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Removes a player from a game
    /// </summary>
    /// <param name="gameId">The ID of the game to leave</param>
    /// <param name="userId">The ID of the user leaving the game</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> LeaveGameAsync(string gameId, string userId)
    {
        try
        {
            var game = await GetGameAsync(gameId);
            if (game == null)
            {
                return false;
            }

            game.Players.Remove(userId);
            return await UpdateGameAsync(game);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Updates a player's location in a game
    /// </summary>
    /// <param name="gameId">The ID of the game</param>
    /// <param name="userId">The ID of the user</param>
    /// <param name="location">The new location data</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> UpdatePlayerLocationAsync(string gameId, string userId, PlayerLocation location)
    {
        try
        {
            await _database
                .Child("games")
                .Child(gameId)
                .Child("playerLocations")
                .Child(userId)
                .PutAsync(location);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Retrieves all player locations for a game
    /// </summary>
    /// <param name="gameId">The ID of the game</param>
    /// <returns>List of player locations</returns>
    public async Task<List<PlayerLocation>> GetPlayerLocationsAsync(string gameId)
    {
        try
        {
            var locations = await _database
                .Child("games")
                .Child(gameId)
                .Child("playerLocations")
                .OnceAsync<PlayerLocation>();

            return locations.Select(l => l.Object).ToList();
        }
        catch (Exception)
        {
            return new List<PlayerLocation>();
        }
    }

    #endregion

    #region Generic Database Operations

    /// <summary>
    /// Retrieves data from a specific path in the database
    /// </summary>
    /// <typeparam name="T">The type of data to retrieve</typeparam>
    /// <param name="path">The database path</param>
    /// <returns>The data if found, null otherwise</returns>
    public async Task<T?> GetAsync<T>(string path) where T : class
    {
        try
        {
            return await _database
                .Child(path)
                .OnceSingleAsync<T>();
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Sets data at a specific path in the database
    /// </summary>
    /// <typeparam name="T">The type of data to set</typeparam>
    /// <param name="path">The database path</param>
    /// <param name="data">The data to set</param>
    /// <returns>Task representing the async operation</returns>
    public async Task SetAsync<T>(string path, T data) where T : class
    {
        await _database
            .Child(path)
            .PutAsync(data);
    }

    /// <summary>
    /// Updates data at a specific path in the database
    /// </summary>
    /// <typeparam name="T">The type of data to update</typeparam>
    /// <param name="path">The database path</param>
    /// <param name="data">The data to update</param>
    /// <returns>Task representing the async operation</returns>
    public async Task UpdateAsync<T>(string path, T data) where T : class
    {
        await _database
            .Child(path)
            .PatchAsync(data);
    }

    /// <summary>
    /// Deletes data at a specific path in the database
    /// </summary>
    /// <param name="path">The database path</param>
    /// <returns>Task representing the async operation</returns>
    public async Task DeleteAsync(string path)
    {
        await _database
            .Child(path)
            .DeleteAsync();
    }

    /// <summary>
    /// Queries data from the database with optional ordering and filtering
    /// </summary>
    /// <typeparam name="T">The type of data to query</typeparam>
    /// <param name="path">The database path</param>
    /// <param name="orderBy">Optional field to order by</param>
    /// <param name="equalTo">Optional value to filter by</param>
    /// <returns>List of matching data</returns>
    public async Task<List<T>> QueryAsync<T>(string path, string? orderBy = null, string? equalTo = null) where T : class
    {
        try
        {
            var query = _database.Child(path);
            
            if (!string.IsNullOrEmpty(orderBy))
            {
                var orderedQuery = query.OrderBy(orderBy);
                if (!string.IsNullOrEmpty(equalTo))
                {
                    var result = await orderedQuery.EqualTo(equalTo).OnceAsync<T>();
                    return result.Select(r => r.Object).ToList();
                }
                var orderedResult = await orderedQuery.OnceAsync<T>();
                return orderedResult.Select(r => r.Object).ToList();
            }

            var baseResult = await query.OnceAsync<T>();
            return baseResult.Select(r => r.Object).ToList();
        }
        catch (Exception)
        {
            return new List<T>();
        }
    }

    #endregion
} 