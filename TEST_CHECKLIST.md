# Booking Tracker Test Checklist

## API Endpoint Tests

### Venues API
- [ ] GET /api/venues - Returns 200 and empty array initially
- [ ] POST /api/venues - Creates a new venue
- [ ] GET /api/venues/{id} - Returns the created venue
- [ ] PUT /api/venues/{id} - Updates a venue
- [ ] DELETE /api/venues/{id} - Deletes a venue
- [ ] POST /api/venues - Validates required fields
- [ ] GET /api/venues - Returns created venues

### Outreach API
- [ ] GET /api/outreach - Returns 200 and empty array initially
- [ ] POST /api/outreach - Creates a new outreach log
- [ ] GET /api/outreach/{id} - Returns the created outreach log
- [ ] PUT /api/outreach/{id} - Updates an outreach log
- [ ] DELETE /api/outreach/{id} - Deletes an outreach log

### Views API
- [ ] GET /api/views/new-venues-needing-review - Returns 200
- [ ] GET /api/views/approved-targets - Returns 200
- [ ] GET /api/views/followups-due-this-week - Returns 200
- [ ] GET /api/views/high-priority-nashville - Returns 200
- [ ] GET /api/views/out-of-town-targets - Returns 200
- [ ] GET /api/views/no-response - Returns 200
- [ ] GET /api/views/booked-venues - Returns 200
- [ ] GET /api/views/strong-rebook-candidates - Returns 200
- [ ] GET /api/views/bad-fit-archive - Returns 200

### Reminders API
- [ ] GET /api/reminders/due - Returns 200 and proper structure
- [ ] GET /api/reminders/overdue - Returns 200 and proper structure

## HTML Endpoint Tests

### Pages
- [ ] GET / - Dashboard loads
- [ ] GET /Venues - Venues list page loads
- [ ] GET /Venues/Create - Create venue form loads (if exists)
- [ ] GET /Venues/Edit/{id} - Edit venue form loads (if exists)
- [ ] GET /Venues/Details/{id} - Venue details loads (if exists)
- [ ] GET /Identity/Account/Register - Registration page loads
- [ ] GET /Identity/Account/Login - Login page loads

## Feature Tests

### Venue Management
- [ ] Can create venue with all required fields
- [ ] Can create venue with optional fields
- [ ] Can update venue
- [ ] Can delete venue
- [ ] Venues appear in appropriate views based on status
- [ ] Fit score validation (1-5)
- [ ] Enum values validate correctly

### Outreach Logging
- [ ] Can create outreach log linked to venue
- [ ] Outreach logs appear in venue details
- [ ] Can track multiple outreach attempts per venue

### Filtering/Views
- [ ] Venues filter correctly by status
- [ ] Priority filtering works
- [ ] Date-based filtering works (follow-ups)
- [ ] Location-based filtering works (Nashville vs out-of-town)

### Data Persistence
- [ ] Data persists in SQLite database
- [ ] Database file created in data directory
- [ ] Migrations apply correctly

## Test Results
*Will be filled in during testing*
