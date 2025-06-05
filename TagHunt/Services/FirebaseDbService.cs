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

    public async Task<T> GetAsync<T>(string path) where T : class
    {
        // TODO: Implement Firebase Realtime Database operations
        throw new NotImplementedException();
    }

    public async Task SetAsync<T>(string path, T data) where T : class
    {
        // TODO: Implement Firebase Realtime Database operations
        throw new NotImplementedException();
    }

    public async Task UpdateAsync<T>(string path, T data) where T : class
    {
        // TODO: Implement Firebase Realtime Database operations
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string path)
    {
        // TODO: Implement Firebase Realtime Database operations
        throw new NotImplementedException();
    }

    public async Task<List<T>> QueryAsync<T>(string path, string orderBy = null, string equalTo = null) where T : class
    {
        // TODO: Implement Firebase Realtime Database operations
        throw new NotImplementedException();
    }
} 