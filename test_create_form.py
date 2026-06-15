import requests
import pickle

# Load the session
session = requests.Session()
with open('/tmp/session.pkl', 'rb') as f:
    session.cookies.update(pickle.load(f))

# Test venue create form
create_page = session.get("http://localhost:5000/Venues/Create")
print(f"Create page status: {create_page.status_code}")
print(f"Create page URL: {create_page.url}")

if create_page.status_code == 200:
    html = create_page.text

    # Check for form
    if '<form' in html:
        print("[OK] Create page has form")
    else:
        print("[FAIL] Create page missing form")

    # Check for required fields
    required_fields = ['name', 'city', 'state', 'venueType', 'priority', 'status']
    missing_fields = []
    for field in required_fields:
        if f'id="{field}"' in html or f"id='{field}'" in html:
            pass
        else:
            missing_fields.append(field)

    if len(missing_fields) == 0:
        print(f"[OK] All required fields present")
    else:
        print(f"[FAIL] Missing fields: {missing_fields}")

    # Check for new fields that were added
    new_fields = ['bookingFormUrl', 'preferredContactMethod', 'payNotes', 'owner', 'nextAction', 'nextFollowUpDate']
    present_new_fields = []
    for field in new_fields:
        if f'id="{field}"' in html or f"id='{field}'" in html:
            present_new_fields.append(field)

    if len(present_new_fields) == len(new_fields):
        print(f"[OK] All new fields present: {present_new_fields}")
    else:
        missing = [f for f in new_fields if f not in present_new_fields]
        print(f"[WARN] Some new fields missing: {missing}")
else:
    print(f"[FAIL] Create page returned {create_page.status_code}")
