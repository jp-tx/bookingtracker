using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTracker.Data;
using BookingTracker.Models;
using BookingTracker.Attributes;

namespace BookingTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiKeyAuthorize]
public class VenuesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public VenuesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/venues
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Venue>>> GetVenues()
    {
        return await _context.Venues
            .Include(v => v.OutreachLogs)
            .OrderByDescending(v => v.UpdatedAt)
            .ToListAsync();
    }

    // GET: api/venues/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Venue>> GetVenue(int id)
    {
        var venue = await _context.Venues
            .Include(v => v.OutreachLogs)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (venue == null)
        {
            return NotFound();
        }

        return venue;
    }

    // POST: api/venues
    [HttpPost]
    public async Task<ActionResult<Venue>> PostVenue(Venue venue)
    {
        venue.CreatedAt = DateTime.UtcNow;
        venue.UpdatedAt = DateTime.UtcNow;

        _context.Venues.Add(venue);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVenue), new { id = venue.Id }, venue);
    }

    // PUT: api/venues/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVenue(int id, Venue venue)
    {
        if (id != venue.Id)
        {
            return BadRequest();
        }

        venue.UpdatedAt = DateTime.UtcNow;
        _context.Entry(venue).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VenueExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/venues/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVenue(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null)
        {
            return NotFound();
        }

        _context.Venues.Remove(venue);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool VenueExists(int id)
    {
        return _context.Venues.Any(e => e.Id == id);
    }
}
