using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookingTracker.Models;

namespace BookingTracker.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Venue> Venues { get; set; }
    public DbSet<OutreachLog> OutreachLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Venue>()
            .HasMany(v => v.OutreachLogs)
            .WithOne(o => o.Venue)
            .HasForeignKey(o => o.VenueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
