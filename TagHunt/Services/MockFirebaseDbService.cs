using TagHunt.Models;
using TagHunt.Services.Interfaces;

namespace TagHunt.Services
{
    /// <summary>
    /// Mock Firebase database service for testing purposes
    /// </summary>
    public class MockFirebaseDbService : IFirebaseDbService
    {
        private readonly List<Game> _games = new();

        /// <summary>
        /// Mock create game - always succeeds
        /// </summary>
        public async Task<bool> CreateGameAsync(Game game)
        {
            await Task.Delay(200); // Simulate network delay
            _games.Add(game);
            return true;
        }

        /// <summary>
        /// Mock update game - always succeeds
        /// </summary>
        public async Task<bool> UpdateGameAsync(Game game)
        {
            await Task.Delay(200);
            var existingGame = _games.FirstOrDefault(g => g.Id == game.Id);
            if (existingGame != null)
            {
                var index = _games.IndexOf(existingGame);
                _games[index] = game;
            }
            return true;
        }

        /// <summary>
        /// Mock get game
        /// </summary>
        public async Task<Game?> GetGameAsync(string gameId)
        {
            await Task.Delay(100);
            return _games.FirstOrDefault(g => g.Id == gameId);
        }

        /// <summary>
        /// Mock get active games - returns empty list
        /// </summary>
        public async Task<List<Game>> GetActiveGamesAsync()
        {
            await Task.Delay(300);
            return _games.Where(g => g.Status == "active").ToList();
        }

        /// <summary>
        /// Mock delete game - always succeeds
        /// </summary>
        public async Task<bool> DeleteGameAsync(string gameId)
        {
            await Task.Delay(200);
            var game = _games.FirstOrDefault(g => g.Id == gameId);
            if (game != null)
            {
                _games.Remove(game);
            }
            return true;
        }

        /// <summary>
        /// Mock join game - always succeeds
        /// </summary>
        public async Task<bool> JoinGameAsync(string gameId, string userId)
        {
            await Task.Delay(250);
            var game = _games.FirstOrDefault(g => g.Id == gameId);
            if (game != null && !game.Players.Contains(userId))
            {
                game.Players.Add(userId);
            }
            return true;
        }

        /// <summary>
        /// Mock leave game - always succeeds
        /// </summary>
        public async Task<bool> LeaveGameAsync(string gameId, string userId)
        {
            await Task.Delay(250);
            var game = _games.FirstOrDefault(g => g.Id == gameId);
            if (game != null)
            {
                game.Players.Remove(userId);
            }
            return true;
        }

        /// <summary>
        /// Mock update player location - always succeeds
        /// </summary>
        public async Task<bool> UpdatePlayerLocationAsync(string gameId, string userId, PlayerLocation location)
        {
            await Task.Delay(150);
            
            var game = _games.FirstOrDefault(g => g.Id == gameId);
            if (game != null)
            {
                game.PlayerLocations[userId] = location;
            }
            
            return true;
        }

        /// <summary>
        /// Mock get player locations - returns empty list or mock data
        /// </summary>
        public async Task<List<PlayerLocation>> GetPlayerLocationsAsync(string gameId)
        {
            await Task.Delay(200);
            var game = _games.FirstOrDefault(g => g.Id == gameId);
            return game?.PlayerLocations.Values.ToList() ?? new List<PlayerLocation>();
        }
    }
} 