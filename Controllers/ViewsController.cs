using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTracker.Data;
using BookingTracker.Models;
using BookingTracker.Attributes;

namespace BookingTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiKeyAuthorize]
public class ViewsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ViewsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/views/new-venues-needing-review
    [HttpGet("new-venues-needing-review")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetNewVenuesNeedingReview()
    {
        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.Status == VenueStatus.NeedsUserReview || v.Status == VenueStatus.ResearchCandidate)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }

    // GET: api/views/approved-targets
    [HttpGet("approved-targets")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetApprovedTargets()
    {
        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.Status == VenueStatus.ApprovedTarget ||
                       v.Status == VenueStatus.DraftNeeded ||
                       v.Status == VenueStatus.ReadyToSend)
            .OrderByDescending(v => v.Priority)
            .ThenBy(v => v.Name)
            .ToListAsync();
    }

    // GET: api/views/followups-due-this-week
    [HttpGet("followups-due-this-week")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetFollowupsDueThisWeek()
    {
        var startOfWeek = DateTime.UtcNow.Date;
        var endOfWeek = startOfWeek.AddDays(7);

        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.Status == VenueStatus.FollowUpDue &&
                       v.NextFollowUpDate.HasValue &&
                       v.NextFollowUpDate.Value >= startOfWeek &&
                       v.NextFollowUpDate.Value <= endOfWeek)
            .OrderBy(v => v.NextFollowUpDate)
            .ToListAsync();
    }

    // GET: api/views/high-priority-nashville
    [HttpGet("high-priority-nashville")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetHighPriorityNashville()
    {
        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.Priority == Priority.High &&
                       v.City != null &&
                       v.City.ToLower().Contains("nashville"))
            .OrderByDescending(v => v.FitScore)
            .ToListAsync();
    }

    // GET: api/views/out-of-town-targets
    [HttpGet("out-of-town-targets")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetOutOfTownTargets()
    {
        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.City != null &&
                       !v.City.ToLower().Contains("nashville") &&
                       (v.Status == VenueStatus.ApprovedTarget ||
                        v.Status == VenueStatus.InConversation ||
                        v.Status == VenueStatus.Contacted))
            .OrderBy(v => v.State)
            .ThenBy(v => v.City)
            .ToListAsync();
    }

    // GET: api/views/no-response
    [HttpGet("no-response")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetNoResponse()
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-14);

        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.Status == VenueStatus.Contacted &&
                       v.LastContactedDate.HasValue &&
                       v.LastContactedDate.Value < cutoffDate &&
                       !v.LastResponseDate.HasValue)
            .OrderBy(v => v.LastContactedDate)
            .ToListAsync();
    }

    // GET: api/views/booked-venues
    [HttpGet("booked-venues")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetBookedVenues()
    {
        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.Status == VenueStatus.Booked)
            .OrderByDescending(v => v.UpdatedAt)
            .ToListAsync();
    }

    // GET: api/views/strong-rebook-candidates
    [HttpGet("strong-rebook-candidates")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetStrongRebookCandidates()
    {
        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.Status == VenueStatus.Booked &&
                       v.FitScore >= 4)
            .OrderByDescending(v => v.FitScore)
            .ToListAsync();
    }

    // GET: api/views/bad-fit-archive
    [HttpGet("bad-fit-archive")]
    public async Task<ActionResult<IEnumerable<Venue>>> GetBadFitArchive()
    {
        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.Status == VenueStatus.BadFit ||
                       v.Status == VenueStatus.DeadNoResponse ||
                       v.Status == VenueStatus.NotNow)
            .OrderBy(v => v.Status)
            .ThenBy(v => v.Name)
            .ToListAsync();
    }
}
