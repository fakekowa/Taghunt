using TagHunt.Models;

namespace TagHunt.Services;

public class FirebaseDbService : IFirebaseDbService
{
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