using TagHunt.Models;

namespace TagHunt.Services.Interfaces
{
    public interface ILocationService
    {
        event EventHandler<PlayerLocation> LocationChanged;
        Task<bool> RequestPermissionsAsync();
        Task<PlayerLocation?> GetCurrentLocationAsync();
        Task StartTrackingAsync();
        Task StopTrackingAsync();
    }
} 