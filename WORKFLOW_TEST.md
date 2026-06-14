# Booking Tracker - Complete Workflow Test

## Application URL
**http://localhost:5035**

## Current Database State

### Venues in Database:
1. **Venue ID 2**: "Test Venue 2" - Status: 1 (Needs User Review)
   - Fit Score: 4/5
   - Priority: High
   - City: Nashville, TN

2. **Venue ID 4**: "Test" - Status: 2 (Approved Target)
   - Priority: High
   - City: Nashville, TN

3. **Venue ID 6**: "New Test Venue for Curation" - Status: 1 (Needs User Review)
   - Fit Score: 4/5
   - Priority: High
   - City: Nashville, TN (Downtown)
   - Contact: John Smith (john@testvenue.com)
   - Notes: "Great venue for acoustic sets, intimate setting, good sound system"

**Expected "Needs Review" Count: 2 venues** (IDs 2 and 6)

---

## Complete Workflow Test Steps

### 1. Dashboard Test
**URL:** http://localhost:5035

**Expected Results:**
- ✅ Total Venues: 3
- ✅ Needs Review: 2
- ✅ Approved: 1
- ✅ Booked: 0
- ✅ Quick action buttons visible
- ✅ Recent venues list shows latest venues
- ✅ Follow-ups section loads

**Test:**
1. Visit dashboard
2. Verify statistics match expected values
3. Click "Review Venues" button → Should go to Curation page

---

### 2. Curation Workflow Test
**URL:** http://localhost:5035/Venues/Curation

**Expected Results:**
- ✅ Shows 2 venues needing review
- ✅ Venue cards display: Name, Location, Type, Priority, Fit Score
- ✅ Each card has "Approve for Outreach" and "Mark as Bad Fit" buttons
- ✅ "View Full Details" link on each card

**Test Approve Workflow:**
1. Visit curation page
2. Find "New Test Venue for Curation" card
3. Click "Approve for Outreach" button
4. Verify alert "Venue approved!"
5. Verify venue disappears from list
6. Return to Dashboard
7. Verify "Needs Review" count decreased to 1
8. Verify "Approved" count increased to 2

**Test Reject Workflow:**
1. Visit curation page
2. Find "Test Venue 2" card
3. Click "Mark as Bad Fit" button
4. Verify alert "Venue marked as bad fit"
5. Verify venue disappears from list
6. Verify "No venues awaiting review!" message appears

---

### 3. Venues List Test
**URL:** http://localhost:5035/Venues

**Expected Results:**
- ✅ Shows all 3 venues in table
- ✅ Search box works (type "Test" → filters venues)
- ✅ Status filter dropdown works
- ✅ View and Edit buttons work for each venue

**Test:**
1. Visit Venues page
2. Test search: Type "New Test" → Should show only venue 6
3. Clear search
4. Test status filter: Select "Approved" → Should show only venue 4
5. Clear filter
6. Click "View" on any venue → Should go to Details page
7. Click "Edit" on any venue → Should go to Edit page

---

### 4. Create Venue Test
**URL:** http://localhost:5035/Venues/Create

**Test:**
1. Visit Create page
2. Fill in form:
   - Name: "The Listening Room"
   - City: "Nashville"
   - State: "TN"
   - Neighborhood: "Music Row"
   - Venue Type: "Bar"
   - Contact Name: "Sarah Johnson"
   - Contact Email: "booking@listeningroom.com"
   - Fit Score: 5
   - Priority: High
   - Status: "Needs User Review"
   - Notes: "Perfect for singer-songwriter format"
3. Click "Create Venue"
4. Verify success alert
5. Verify redirected to Venues list
6. Verify new venue appears in list
7. Go to Dashboard → Verify "Needs Review" increased to 2

---

### 5. Edit Venue Test
**URL:** http://localhost:5035/Venues/Edit/6

**Test:**
1. Visit Edit page for venue 6
2. Change Fit Score from 4 to 5
3. Add to Notes: " - UPDATED"
4. Click "Save Changes"
5. Verify success alert
6. Go to Details page → Verify changes saved

---

### 6. Venue Details Test
**URL:** http://localhost:5035/Venues/Details/6

**Expected Display:**
- ✅ Status, Priority, and Fit Score cards at top
- ✅ Basic Information card with location
- ✅ Contact Information card with clickable email/phone links
- ✅ Venue Details card
- ✅ Research Info card
- ✅ Notes card
- ✅ Outreach History section (empty for now)

**Test:**
1. Visit Details page
2. Verify all information displays correctly
3. Click email link → Should open email client
4. Click "Edit Venue" → Should go to Edit page
5. Click "Back to List" → Should go to Venues page

---

### 7. Outreach Log Test
**URL:** http://localhost:5035/Outreach

**Current State:** 1 outreach log exists for Venue 4

**Test:**
1. Visit Outreach page
2. Verify table shows at least 1 log entry
3. Test search: Type "Test" → Should filter by venue name
4. Test channel filter: Select "Email"
5. Test result filter
6. Click venue name link → Should go to venue details

---

## API Endpoints Test

All endpoints can be tested via:
- Dashboard API Documentation (expand section)
- Or use curl/Postman

### Key Endpoints:
```bash
# Get all venues
curl http://localhost:5035/api/venues

# Get venues needing review
curl http://localhost:5035/api/views/new-venues-needing-review

# Get approved targets
curl http://localhost:5035/api/views/approved-targets

# Get follow-ups due this week
curl http://localhost:5035/api/views/followups-due-this-week
```

---

## Status Values Reference

- **0** = Research Candidate
- **1** = Needs User Review
- **2** = Approved Target
- **3** = Draft Needed
- **4** = Ready to Send
- **5** = Contacted
- **6** = Follow-up Due
- **7** = In Conversation
- **8** = Booked
- **9** = Not Now
- **10** = Bad Fit
- **11** = Dead/No Response
- **12** = Existing Relationship

---

## Known Issues Fixed

1. ✅ **Curation page template literal bug** - Fixed on line 77 (was using single quotes instead of backticks)
2. ✅ **All emojis removed** - Replaced with text labels throughout the app
3. ✅ **Dark mode styling** - Applied consistently across all pages

---

## Next Steps for Full Production

1. **Add authentication** - Currently no login required
2. **Add outreach log creation UI** - Currently only accessible via API
3. **Add data export** - CSV/Excel export for venues and outreach logs
4. **Add email integration** - Track sent emails automatically
5. **Add calendar integration** - Show follow-ups on calendar
6. **Add analytics dashboard** - Conversion rates, response times, etc.
