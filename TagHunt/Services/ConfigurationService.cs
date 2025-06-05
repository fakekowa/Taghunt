using TagHunt.Models;

namespace TagHunt.Services
{
    /// <summary>
    /// Interface for configuration services that provide application settings
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the Firebase configuration for the application
        /// </summary>
        /// <returns>Firebase configuration object</returns>
        FirebaseConfig GetFirebaseConfig();
    }

    /// <summary>
    /// Service that provides configuration settings for the application
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        /// <summary>
        /// Gets the Firebase configuration, prioritizing environment variables over platform defaults
        /// </summary>
        /// <returns>Firebase configuration with API keys and domain settings</returns>
        public FirebaseConfig GetFirebaseConfig()
        {
            // Try to get from environment variables first (most secure)
            var apiKey = Environment.GetEnvironmentVariable("FIREBASE_API_KEY");
            var authDomain = Environment.GetEnvironmentVariable("FIREBASE_AUTH_DOMAIN");
            var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");

            if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(authDomain))
            {
                return new FirebaseConfig
                {
                    ApiKey = apiKey,
                    AuthDomain = authDomain,
                    ProjectId = projectId ?? "taghunt-2507b"
                };
            }

            // Fallback to platform-specific defaults
#if IOS
            return new FirebaseConfig
            {
                ApiKey = "AIzaSyDKAfhQ9rYxk-_AYZeYoEjyiN4CuibRhYo",
                AuthDomain = "taghunt-2507b.firebaseapp.com",
                ProjectId = "taghunt-2507b"
            };
#elif ANDROID
            return new FirebaseConfig
            {
                ApiKey = "AIzaSyDKAfhQ9rYxk-_AYZeYoEjyiN4CuibRhYo",
                AuthDomain = "taghunt-2507b.firebaseapp.com",
                ProjectId = "taghunt-2507b"
            };
#else
            return new FirebaseConfig
            {
                ApiKey = "AIzaSyDKAfhQ9rYxk-_AYZeYoEjyiN4CuibRhYo",
                AuthDomain = "taghunt-2507b.firebaseapp.com",
                ProjectId = "taghunt-2507b"
            };
#endif
        }
    }
} 