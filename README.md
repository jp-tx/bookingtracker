Probably dont use this, its ai slop i use internally to source data from openclaw, im not gonna maintain it and theres probably bugs everywhere


# Booking Tracker

A lightweight booking pipeline tracker for musicians to manage venue research, curation, and outreach.

## Features

- **Venue Management**: Track venues with detailed information including contact details, capacity, music format fit, and more
- **Outreach Logging**: Record all communication attempts and responses
- **Pipeline Statuses**: Track venues through various stages from research to booking
- **Curation Workflow**: Review and approve venues before outreach
- **Follow-up Reminders**: Automated reminders for follow-up communications
- **Multiple Views**: Pre-configured views for different workflows (new venues, approved targets, follow-ups due, etc.)
- **API-First Design**: All data accessible via REST API for integration with bots and other tools

## Users

1. **Curator**: Decision-maker, sender of booking communications
2. **Researcher**: Venue-fit analyst, organizer, draft writer
3. **Secondary Communicator**: Additional outreach support

## Technology Stack

- **Backend**: ASP.NET Core 9.0 with Razor Pages
- **Database**: SQLite with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Deployment**: Docker with Docker Compose
- **Data Storage**: Volume-mounted data directory for easy backup

## Quick Start

### Running with Docker Compose

1. Build and run the application:
   ```bash
   docker-compose up -d
   ```

2. Access the application:
   - Web UI: http://localhost:5000
   - API: http://localhost:5000/api

3. Data is stored in the `./data` directory on the host for easy backup.

### Running Locally for Development

1. Restore dependencies:
   ```bash
   dotnet restore
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Access the application at https://localhost:5001 or http://localhost:5000

## API Endpoints

### Venues
- `GET /api/venues` - List all venues
- `GET /api/venues/{id}` - Get single venue
- `POST /api/venues` - Create venue
- `PUT /api/venues/{id}` - Update venue
- `DELETE /api/venues/{id}` - Delete venue

### Outreach Logs
- `GET /api/outreach` - List all outreach logs
- `GET /api/outreach/{id}` - Get single outreach log
- `POST /api/outreach` - Create outreach log
- `PUT /api/outreach/{id}` - Update outreach log
- `DELETE /api/outreach/{id}` - Delete outreach log

### Views (Filtered Lists)
- `GET /api/views/new-venues-needing-review` - Venues awaiting curation
- `GET /api/views/approved-targets` - Approved venues ready for outreach
- `GET /api/views/followups-due-this-week` - Follow-ups due in the next 7 days
- `GET /api/views/high-priority-nashville` - High-priority Nashville venues
- `GET /api/views/out-of-town-targets` - Non-Nashville venues
- `GET /api/views/no-response` - Venues with no response after 14+ days
- `GET /api/views/booked-venues` - Successfully booked venues
- `GET /api/views/strong-rebook-candidates` - High fit score booked venues
- `GET /api/views/bad-fit-archive` - Rejected/dead venues

### Reminders (for bots)
- `GET /api/reminders/due` - Get today's follow-up reminders
- `GET /api/reminders/overdue` - Get overdue follow-ups

## Venue Pipeline Statuses

1. **Research Candidate**: Initial venue discovery
2. **Needs User Review**: Ready for curator approval
3. **Approved Target**: Curator approved
4. **Draft Needed**: Needs outreach message draft
5. **Ready to Send**: Draft approved, ready to contact
6. **Contacted**: Initial outreach sent
7. **Follow-up Due**: Waiting period passed, needs follow-up
8. **In Conversation**: Venue responded
9. **Booked**: Date secured
10. **Not Now**: Possible later
11. **Bad Fit**: Do not pursue
12. **Dead/No Response**: No reply after final follow-up
13. **Existing Relationship**: Venue already knows/books artist

## Follow-up Rules

- **First follow-up**: 5-7 days after initial outreach
- **Second follow-up**: 10-14 days after first follow-up
- **Auto-archive**: After two follow-ups with no response, moved to "Dead/no response"

## Data Directory

All data is stored in a configurable data directory:
- **Development**: `./data`
- **Docker**: `/data` (mounted from host `./data`)

This allows for easy backup by simply copying the data directory.

## Database Migrations

To create a new migration:
```bash
dotnet ef migrations add MigrationName
```

To apply migrations:
```bash
dotnet ef database update
```

## Architecture

The application follows an API-first architecture:
- All data operations are available via REST API
- Web interface is modular and consumes the same APIs
- Bot integration is supported through dedicated reminder endpoints
- Authentication is handled via ASP.NET Core Identity

## Backup

To backup your data:
1. Stop the application: `docker-compose down`
2. Copy the `./data` directory to your backup location
3. Restart the application: `docker-compose up -d`

## License

This project is for personal use.
