using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingTracker.Pages.Venues;

public class EditModel : PageModel
{
    public int Id { get; set; }

    public void OnGet(int id)
    {
        Id = id;
    }
}
