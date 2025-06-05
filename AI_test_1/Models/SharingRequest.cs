namespace AI_test_1.Models;

public class SharingRequest
{
    public string Id { get; set; }
    public string RequesterId { get; set; }
    public string RecipientId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsAccepted { get; set; }
    public LocationGps LastLocation { get; set; }
} 