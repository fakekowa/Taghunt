using AI_test_1.Models;

namespace AI_test_1.Services;

public interface IFirebaseDbService
{
    Task<T> GetAsync<T>(string path) where T : class;
    Task SetAsync<T>(string path, T data) where T : class;
    Task UpdateAsync<T>(string path, T data) where T : class;
    Task DeleteAsync(string path);
    Task<List<T>> QueryAsync<T>(string path, string orderBy = null, string equalTo = null) where T : class;
} 