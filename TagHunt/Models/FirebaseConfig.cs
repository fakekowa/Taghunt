namespace TagHunt.Models
{
    public class FirebaseConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        public string AuthDomain { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
    }

    public class FirebaseSettings
    {
        public FirebaseConfig iOS { get; set; } = new();
        public FirebaseConfig Android { get; set; } = new();
    }
} 