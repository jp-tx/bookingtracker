using System.ComponentModel.DataAnnotations;

namespace BookingTracker.Models;

public class OutreachLog
{
    public int Id { get; set; }

    public int VenueId { get; set; }

    public Venue? Venue { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public Channel Channel { get; set; }

    public Direction Direction { get; set; }

    [MaxLength(200)]
    public string? SenderContact { get; set; }

    [MaxLength(2000)]
    public string? Summary { get; set; }

    public OutreachResult? Result { get; set; }

    public DateTime? FollowUpDate { get; set; }

    [MaxLength(1000)]
    public string? MessageDraftLink { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
