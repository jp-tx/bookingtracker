import requests
import pickle

# Load the session
session = requests.Session()
with open('/tmp/session.pkl', 'rb') as f:
    session.cookies.update(pickle.load(f))

# Test if we can fetch venues via API with the session cookies
api_response = session.get("http://localhost:5000/api/venues")
print(f"API /api/venues status: {api_response.status_code}")

if api_response.status_code == 200:
    venues = api_response.json()
    print(f"[OK] API returned {len(venues)} venues")
    if len(venues) > 0:
        print(f"[OK] First venue: {venues[0].get('name', 'No name')}")
    else:
        print("[INFO] No venues in database yet")
else:
    print(f"[FAIL] API returned status {api_response.status_code}")
    print(f"Response: {api_response.text[:200]}")

# Get the dashboard HTML and check if JavaScript is fetching data
dashboard = session.get("http://localhost:5000/")
html = dashboard.text

# Check for the JavaScript that fetches venue data
if 'fetch(\'/api/venues\')' in html or 'fetch("/api/venues")' in html:
    print("[OK] Dashboard JavaScript calls /api/venues")
else:
    print("[FAIL] Dashboard JavaScript does NOT call /api/venues")

# Check for venue display elements
if 'id="venuesTableBody"' in html or 'id="venuesList"' in html:
    print("[OK] Dashboard has venue display container")
else:
    print("[FAIL] Dashboard missing venue display container")

# Check for error messages in dashboard
if 'error' in html.lower() and 'failed' in html.lower():
    print("[WARNING] Dashboard may have errors")
    # Find and print error messages
    import re
    errors = re.findall(r'error[^<]*', html.lower())
    for err in errors[:3]:
        print(f"  Found: {err}")
