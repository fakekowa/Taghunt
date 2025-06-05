using TagHunt.Models;

namespace TagHunt.Services.Interfaces;

public interface IFirebaseDbService
{
    Task<bool> CreateGameAsync(Game game);
    Task<bool> UpdateGameAsync(Game game);
    Task<Game?> GetGameAsync(string gameId);
    Task<List<Game>> GetActiveGamesAsync();
    Task<bool> DeleteGameAsync(string gameId);
    Task<bool> JoinGameAsync(string gameId, string userId);
    Task<bool> LeaveGameAsync(string gameId, string userId);
    Task<bool> UpdatePlayerLocationAsync(string gameId, string userId, PlayerLocation location);
    Task<List<PlayerLocation>> GetPlayerLocationsAsync(string gameId);
} 