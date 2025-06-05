using TagHunt.Models;

namespace TagHunt.Services
{
    public interface IConfigurationService
    {
        FirebaseConfig GetFirebaseConfig();
    }

    public class ConfigurationService : IConfigurationService
    {
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
                ApiKey = "AIzaSyDa4Gp42u1jQhj5BkIlZjbdIoemISc_kRM",
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
                ApiKey = "AIzaSyDa4Gp42u1jQhj5BkIlZjbdIoemISc_kRM",
                AuthDomain = "taghunt-2507b.firebaseapp.com",
                ProjectId = "taghunt-2507b"
            };
#endif
        }
    }
} 