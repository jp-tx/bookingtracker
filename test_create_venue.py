import requests
import pickle
import json

# Load the session
session = requests.Session()
with open('/tmp/session.pkl', 'rb') as f:
    session.cookies.update(pickle.load(f))

# Create a test venue
venue_data = {
    "name": "Test Venue",
    "city": "Nashville",
    "state": "TN",
    "neighborhood": "East Nashville",
    "venueType": 0,  # Bar
    "priority": 1,  # Medium
    "source": 0,  # User Research
    "researchConfidence": 1,  # Medium
    "status": 0,  # Research Candidate
    "fitScore": 3,
    "notes": "This is a test venue created by automated testing"
}

print("Creating test venue...")
create_response = session.post(
    "http://localhost:5000/api/venues",
    json=venue_data,
    headers={"Content-Type": "application/json"}
)

print(f"Create response status: {create_response.status_code}")

if create_response.status_code == 200 or create_response.status_code == 201:
    created_venue = create_response.json()
    print(f"[OK] Venue created successfully!")
    print(f"     Venue ID: {created_venue.get('id')}")
    print(f"     Venue Name: {created_venue.get('name')}")

    # Verify we can fetch the venue
    venue_id = created_venue.get('id')
    fetch_response = session.get(f"http://localhost:5000/api/venues/{venue_id}")
    if fetch_response.status_code == 200:
        print(f"[OK] Can fetch created venue")
    else:
        print(f"[FAIL] Cannot fetch created venue: {fetch_response.status_code}")

    # Verify it appears in the list
    list_response = session.get("http://localhost:5000/api/venues")
    if list_response.status_code == 200:
        venues = list_response.json()
        print(f"[OK] Total venues in system: {len(venues)}")
        if any(v.get('id') == venue_id for v in venues):
            print(f"[OK] New venue appears in list")
        else:
            print(f"[FAIL] New venue NOT in list")
else:
    print(f"[FAIL] Venue creation failed: {create_response.status_code}")
    print(f"Response: {create_response.text[:500]}")
