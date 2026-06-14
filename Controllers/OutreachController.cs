using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTracker.Data;
using BookingTracker.Models;
using BookingTracker.Attributes;

namespace BookingTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiKeyAuthorize]
public class OutreachController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OutreachController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/outreach
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OutreachLog>>> GetOutreachLogs()
    {
        return await _context.OutreachLogs
            .Include(o => o.Venue)
            .OrderByDescending(o => o.Date)
            .ToListAsync();
    }

    // GET: api/outreach/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OutreachLog>> GetOutreachLog(int id)
    {
        var outreachLog = await _context.OutreachLogs
            .Include(o => o.Venue)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (outreachLog == null)
        {
            return NotFound();
        }

        return outreachLog;
    }

    // POST: api/outreach
    [HttpPost]
    public async Task<ActionResult<OutreachLog>> PostOutreachLog(OutreachLog outreachLog)
    {
        outreachLog.CreatedAt = DateTime.UtcNow;

        _context.OutreachLogs.Add(outreachLog);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOutreachLog), new { id = outreachLog.Id }, outreachLog);
    }

    // PUT: api/outreach/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOutreachLog(int id, OutreachLog outreachLog)
    {
        if (id != outreachLog.Id)
        {
            return BadRequest();
        }

        _context.Entry(outreachLog).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OutreachLogExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/outreach/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOutreachLog(int id)
    {
        var outreachLog = await _context.OutreachLogs.FindAsync(id);
        if (outreachLog == null)
        {
            return NotFound();
        }

        _context.OutreachLogs.Remove(outreachLog);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OutreachLogExists(int id)
    {
        return _context.OutreachLogs.Any(e => e.Id == id);
    }
}
