using System.ComponentModel.DataAnnotations;

namespace BookingTracker.Models;

public class Venue
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(50)]
    public string? State { get; set; }

    [MaxLength(100)]
    public string? Neighborhood { get; set; }

    public VenueType VenueType { get; set; }

    [MaxLength(500)]
    public string? Website { get; set; }

    [MaxLength(200)]
    public string? Instagram { get; set; }

    [MaxLength(200)]
    public string? Facebook { get; set; }

    [MaxLength(200)]
    public string? BookingContactName { get; set; }

    [MaxLength(200)]
    public string? BookingContactEmail { get; set; }

    [MaxLength(50)]
    public string? BookingContactPhone { get; set; }

    [MaxLength(500)]
    public string? BookingFormUrl { get; set; }

    [MaxLength(1000)]
    public string? OtherContactDetails { get; set; }

    [MaxLength(100)]
    public string? PreferredContactMethod { get; set; }

    [MaxLength(100)]
    public string? CapacityRoomSize { get; set; }

    [MaxLength(500)]
    public string? MusicFormatFit { get; set; }

    [MaxLength(200)]
    public string? TypicalMusicDaysTimes { get; set; }

    [MaxLength(500)]
    public string? PayNotes { get; set; }

    public HouseSound HouseSound { get; set; } = HouseSound.Unknown;

    [MaxLength(100)]
    public string? TravelDistanceFromNashville { get; set; }

    [Range(1, 5)]
    public int? FitScore { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public Source Source { get; set; }

    public ResearchConfidence ResearchConfidence { get; set; } = ResearchConfidence.Medium;

    public VenueStatus Status { get; set; } = VenueStatus.ResearchCandidate;

    [MaxLength(500)]
    public string? NextAction { get; set; }

    public DateTime? NextFollowUpDate { get; set; }

    public DateTime? LastContactedDate { get; set; }

    public DateTime? LastResponseDate { get; set; }

    [MaxLength(100)]
    public string? Owner { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public ICollection<OutreachLog> OutreachLogs { get; set; } = new List<OutreachLog>();
}
