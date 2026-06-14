using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingTracker.Pages.Venues;

public class DetailsModel : PageModel
{
    public int Id { get; set; }

    public void OnGet(int id)
    {
        Id = id;
    }
}
