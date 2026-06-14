# Getting Started with Booking Tracker

## Quick Start

### Option 1: Run with Docker (Recommended)

1. Build and start the application:
   ```bash
   docker-compose up -d
   ```

2. Access the application:
   - Web Interface: http://localhost:5000
   - API: http://localhost:5000/api

3. Stop the application:
   ```bash
   docker-compose down
   ```

### Option 2: Run Locally for Development

1. Ensure you have .NET 9.0 SDK installed

2. Create the data directory:
   ```bash
   mkdir data
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Access the application at https://localhost:5001 or http://localhost:5000

## First Steps

1. **Register an Account**
   - Navigate to http://localhost:5000
   - Click on "Register" in the top right
   - Create your user account

2. **Add Your First Venue**
   - Click on "Manage Venues" from the dashboard
   - Click "Add New Venue"
   - Fill in the venue details

   Alternatively, use the API:
   ```bash
   curl -X POST http://localhost:5000/api/venues \
     -H "Content-Type: application/json" \
     -d '{
       "name": "The Bluebird Cafe",
       "city": "Nashville",
       "state": "TN",
       "venueType": 4,
       "status": 1,
       "priority": 2,
       "source": 0,
       "researchConfidence": 2
     }'
   ```

3. **View Venues**
   - Web: http://localhost:5000/Venues
   - API: http://localhost:5000/api/venues

4. **Use Pre-configured Views**
   - New venues needing review: http://localhost:5000/api/views/new-venues-needing-review
   - Approved targets: http://localhost:5000/api/views/approved-targets
   - Follow-ups due this week: http://localhost:5000/api/views/followups-due-this-week
   - High-priority Nashville: http://localhost:5000/api/views/high-priority-nashville
   - And more (see README.md)

5. **Set Up Reminders for Bots**
   - Today's reminders: http://localhost:5000/api/reminders/due
   - Overdue reminders: http://localhost:5000/api/reminders/overdue

## Project Structure

```
bookingtracker/
├── Controllers/          # API Controllers
│   ├── VenuesController.cs
│   ├── OutreachController.cs
│   ├── ViewsController.cs
│   └── RemindersController.cs
├── Models/              # Data models
│   ├── Venue.cs
│   ├── OutreachLog.cs
│   └── Enums.cs
├── Data/                # Database context
│   └── ApplicationDbContext.cs
├── Pages/               # Razor Pages UI
│   ├── Index.cshtml
│   └── Venues/
├── Migrations/          # EF Core migrations
├── data/                # SQLite database storage (created at runtime)
├── docker-compose.yml   # Docker configuration
├── Dockerfile          # Docker build instructions
└── README.md           # Full documentation
```

## API Endpoints Overview

### Venues
- `GET /api/venues` - List all venues
- `GET /api/venues/{id}` - Get single venue
- `POST /api/venues` - Create venue
- `PUT /api/venues/{id}` - Update venue
- `DELETE /api/venues/{id}` - Delete venue

### Outreach Logs
- `GET /api/outreach` - List all outreach logs
- `POST /api/outreach` - Create outreach log
- And more...

### Views (Filtered Lists)
- 9 pre-configured views for different workflows
- See README.md for full list

### Reminders
- `GET /api/reminders/due` - Today's reminders
- `GET /api/reminders/overdue` - Overdue reminders

## Data Backup

Your data is stored in the `./data` directory:

1. To backup: Simply copy the `./data` folder to your backup location
2. To restore: Copy your backup back to `./data` and restart the application

## Enum Reference

When creating venues via API, use these enum values:

### VenueType
- 0 = Bar
- 1 = HonkyTonk
- 2 = Restaurant
- 3 = Brewery
- 4 = PrivateEvent
- 5 = Festival
- 6 = Corporate
- 7 = SongwriterRound
- 8 = Other

### Priority
- 0 = Low
- 1 = Medium
- 2 = High

### VenueStatus
- 0 = ResearchCandidate
- 1 = NeedsUserReview
- 2 = ApprovedTarget
- 3 = DraftNeeded
- 4 = ReadyToSend
- 5 = Contacted
- 6 = FollowUpDue
- 7 = InConversation
- 8 = Booked
- 9 = NotNow
- 10 = BadFit
- 11 = DeadNoResponse
- 12 = ExistingRelationship

### Source
- 0 = UserResearch
- 1 = UserReferral
- 2 = VenueDiscovery
- 3 = PreviousGig
- 4 = Inbound
- 5 = Other

### ResearchConfidence
- 0 = Low
- 1 = Medium
- 2 = High

## Next Steps

1. Add more venues through the API or web interface
2. Log outreach communications
3. Use views to track your booking pipeline
4. Set up a bot to check reminders daily
5. Customize the UI pages as needed

For full documentation, see README.md
