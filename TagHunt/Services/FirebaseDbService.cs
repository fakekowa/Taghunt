using Firebase.Database;
using Firebase.Database.Query;
using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services;

public class FirebaseDbService : IFirebaseDbService
{
    private readonly FirebaseClient _database;

    public FirebaseDbService(string databaseUrl)
    {
        _database = new FirebaseClient(databaseUrl);
    }

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

    public async Task<bool> JoinGameAsync(string gameId, string userId)
    {
        try
        {
            var game = await GetGameAsync(gameId);
            if (game == null) return false;

            game.Players.Add(userId);
            return await UpdateGameAsync(game);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> LeaveGameAsync(string gameId, string userId)
    {
        try
        {
            var game = await GetGameAsync(gameId);
            if (game == null) return false;

            game.Players.Remove(userId);
            return await UpdateGameAsync(game);
        }
        catch (Exception)
        {
            return false;
        }
    }

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

    public async Task SetAsync<T>(string path, T data) where T : class
    {
        await _database
            .Child(path)
            .PutAsync(data);
    }

    public async Task UpdateAsync<T>(string path, T data) where T : class
    {
        await _database
            .Child(path)
            .PatchAsync(data);
    }

    public async Task DeleteAsync(string path)
    {
        await _database
            .Child(path)
            .DeleteAsync();
    }

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
} 