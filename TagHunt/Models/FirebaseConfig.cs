namespace TagHunt.Models
{
    /// <summary>
    /// Configuration settings for Firebase services
    /// </summary>
    public class FirebaseConfig
    {
        /// <summary>
        /// Gets or sets the Firebase API key
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the Firebase authentication domain
        /// </summary>
        public string AuthDomain { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the Firebase project ID
        /// </summary>
        public string ProjectId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Platform-specific Firebase configuration settings
    /// </summary>
    public class FirebaseSettings
    {
        /// <summary>
        /// Gets or sets the Firebase configuration for iOS
        /// </summary>
        public FirebaseConfig iOS { get; set; } = new();
        
        /// <summary>
        /// Gets or sets the Firebase configuration for Android
        /// </summary>
        public FirebaseConfig Android { get; set; } = new();
    }
} 