namespace BookingTracker.Models;

public enum VenueType
{
    Bar,
    HonkyTonk,
    Restaurant,
    Brewery,
    PrivateEvent,
    Festival,
    Corporate,
    SongwriterRound,
    Other
}

public enum Priority
{
    Low,
    Medium,
    High
}

public enum Source
{
    UserResearch,
    UserReferral,
    VenueDiscovery,
    PreviousGig,
    Inbound,
    Other
}

public enum ResearchConfidence
{
    Low,
    Medium,
    High
}

public enum VenueStatus
{
    ResearchCandidate,
    NeedsUserReview,
    ApprovedTarget,
    DraftNeeded,
    ReadyToSend,
    Contacted,
    FollowUpDue,
    InConversation,
    Booked,
    NotNow,
    BadFit,
    DeadNoResponse,
    ExistingRelationship
}

public enum HouseSound
{
    Yes,
    No,
    Unknown
}

public enum Channel
{
    Email,
    Instagram,
    Facebook,
    Phone,
    InPerson,
    Form,
    Referral
}

public enum Direction
{
    Outbound,
    Inbound
}

public enum OutreachResult
{
    NoReply,
    Interested,
    Passed,
    Booked,
    NeedsFollowUp,
    BadFit
}
