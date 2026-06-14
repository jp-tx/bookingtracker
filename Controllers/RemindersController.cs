using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTracker.Data;
using BookingTracker.Models;
using BookingTracker.Attributes;

namespace BookingTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiKeyAuthorize]
public class RemindersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RemindersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/reminders/due
    [HttpGet("due")]
    public async Task<ActionResult<object>> GetRemindiersDue()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var followUpsDue = await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.NextFollowUpDate.HasValue &&
                       v.NextFollowUpDate.Value >= today &&
                       v.NextFollowUpDate.Value < tomorrow)
            .OrderBy(v => v.NextFollowUpDate)
            .Select(v => new
            {
                v.Id,
                v.Name,
                v.City,
                v.State,
                v.Status,
                v.NextFollowUpDate,
                v.NextAction,
                v.LastContactedDate,
                v.Priority,
                v.Owner
            })
            .ToListAsync();

        return new
        {
            date = today,
            count = followUpsDue.Count,
            reminders = followUpsDue
        };
    }

    // GET: api/reminders/overdue
    [HttpGet("overdue")]
    public async Task<ActionResult<object>> GetRemindersOverdue()
    {
        var today = DateTime.UtcNow.Date;

        var overdueFollowUps = await _context.Venues
            .Include(v => v.OutreachLogs)
            .Where(v => v.NextFollowUpDate.HasValue &&
                       v.NextFollowUpDate.Value < today)
            .OrderBy(v => v.NextFollowUpDate)
            .Select(v => new
            {
                v.Id,
                v.Name,
                v.City,
                v.State,
                v.Status,
                v.NextFollowUpDate,
                v.NextAction,
                v.LastContactedDate,
                v.Priority,
                v.Owner,
                daysOverdue = v.NextFollowUpDate.HasValue ? (today - v.NextFollowUpDate.Value).Days : 0
            })
            .ToListAsync();

        return new
        {
            date = today,
            count = overdueFollowUps.Count,
            reminders = overdueFollowUps
        };
    }
}
