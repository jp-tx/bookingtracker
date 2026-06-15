import requests
import pickle

# Load the session
session = requests.Session()
with open('/tmp/session.pkl', 'rb') as f:
    session.cookies.update(pickle.load(f))

# Test venues list page
venues_page = session.get("http://localhost:5000/Venues")
print(f"Venues page status: {venues_page.status_code}")
print(f"Venues page URL: {venues_page.url}")

if venues_page.status_code == 200:
    html = venues_page.text

    if 'Venues' in html or 'venues' in html:
        print("[OK] Venues page loaded")
    else:
        print("[FAIL] Venues page did not load correctly")

    # Check for venue table or list
    if 'table' in html.lower() or '<tbody' in html:
        print("[OK] Venues page has table structure")
    else:
        print("[FAIL] Venues page missing table")

    # Check for create button
    if 'Create' in html or 'Add' in html:
        print("[OK] Venues page has create/add option")
    else:
        print("[FAIL] Venues page missing create button")
else:
    print(f"[FAIL] Venues page returned {venues_page.status_code}")
