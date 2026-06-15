import requests
import pickle

# Load the session
session = requests.Session()
with open('/tmp/session.pkl', 'rb') as f:
    session.cookies.update(pickle.load(f))

# Test Edit page
edit_page = session.get("http://localhost:5000/Venues/Edit/1")
html = edit_page.text

print("=== Testing Future Contact Modal Feature ===\n")

# Test 1: Check for "Marked for Future Contact" status option
if 'Marked for Future Contact' in html:
    print("[OK] 'Marked for Future Contact' status option present in Edit page")
else:
    print("[FAIL] 'Marked for Future Contact' status missing from Edit page")

# Test 2: Check for modal
if 'futureContactModal' in html:
    print("[OK] Future Contact modal present")
else:
    print("[FAIL] Future Contact modal missing")

# Test 3: Check for modal title
if 'Reason for Future Contact' in html:
    print("[OK] Modal has correct title")
else:
    print("[FAIL] Modal title missing")

# Test 4: Check for reason textarea
if 'futureContactReason' in html:
    print("[OK] Reason textarea present")
else:
    print("[FAIL] Reason textarea missing")

# Test 5: Check for JavaScript functions
if 'handleStatusChange' in html:
    print("[OK] Status change handler present")
else:
    print("[FAIL] Status change handler missing")

if 'confirmFutureContact' in html:
    print("[OK] Confirmation button present")
else:
    print("[FAIL] Confirmation button missing")

# Test Create page
create_page = session.get("http://localhost:5000/Venues/Create")
create_html = create_page.text

print("\n=== Testing Create Page ===\n")

if 'Marked for Future Contact' in create_html:
    print("[OK] 'Marked for Future Contact' option present in Create page")
else:
    print("[FAIL] 'Marked for Future Contact' missing from Create page")

if 'futureContactModal' in create_html:
    print("[OK] Future Contact modal present in Create page")
else:
    print("[FAIL] Future Contact modal missing from Create page")

print("\n=== Feature Summary ===")
print("When 'Marked for Future Contact' is selected:")
print("1. Modal popup appears requiring a reason")
print("2. Reason is mandatory (cannot submit without it)")
print("3. Reason is appended to notes with timestamp")
print("4. If modal is cancelled, status reverts to previous value")
