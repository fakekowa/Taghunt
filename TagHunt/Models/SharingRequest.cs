namespace TagHunt.Models;

public class SharingRequest
{
    public required string Id { get; set; }
    public required string RequesterId { get; set; }
    public required string RecipientId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsAccepted { get; set; }
    public required LocationGps LastLocation { get; set; }
} 