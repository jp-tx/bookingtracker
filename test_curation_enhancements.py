import requests
import pickle

# Load the session
session = requests.Session()
with open('/tmp/session.pkl', 'rb') as f:
    session.cookies.update(pickle.load(f))

print("=== Testing Curation Workflow Enhancements ===\n")

# Test Curation page
curation_page = session.get("http://localhost:5000/Venues/Curation")
html = curation_page.text

# Test 1: Check for "Mark for Future Contact" button
if 'Mark for Future Contact' in html:
    print("[OK] 'Mark for Future Contact' button present in Curation workflow")
else:
    print("[FAIL] 'Mark for Future Contact' button missing from Curation")

# Test 2: Check for modal
if 'futureContactModal' in html:
    print("[OK] Future Contact modal present in Curation")
else:
    print("[FAIL] Future Contact modal missing from Curation")

# Test 3: Check for markForFutureContact function
if 'markForFutureContact' in html:
    print("[OK] markForFutureContact function present")
else:
    print("[FAIL] markForFutureContact function missing")

# Test 4: Check for confirmFutureContact handler
if 'confirmFutureContact' in html:
    print("[OK] Confirmation handler present")
else:
    print("[FAIL] Confirmation handler missing")

# Test Details page
details_page = session.get("http://localhost:5000/Venues/Details/1")
details_html = details_page.text

print("\n=== Testing Details Page Curation Actions ===\n")

# Test 5: Check for curation actions section
if 'curationActions' in details_html:
    print("[OK] Curation actions section present in Details page")
else:
    print("[FAIL] Curation actions section missing from Details")

# Test 6: Check for buttons
if 'Approve for Outreach' in details_html:
    print("[OK] 'Approve for Outreach' button present")
else:
    print("[FAIL] 'Approve for Outreach' button missing")

if 'Mark for Future Contact' in details_html:
    print("[OK] 'Mark for Future Contact' button present in Details")
else:
    print("[FAIL] 'Mark for Future Contact' button missing from Details")

if 'Mark as Bad Fit' in details_html:
    print("[OK] 'Mark as Bad Fit' button present")
else:
    print("[FAIL] 'Mark as Bad Fit' button missing")

# Test 7: Check for modal in Details
if 'futureContactModal' in details_html:
    print("[OK] Future Contact modal present in Details page")
else:
    print("[FAIL] Future Contact modal missing from Details")

# Test 8: Check for button handlers
if 'approveBtn' in details_html and 'futureContactBtn' in details_html and 'badFitBtn' in details_html:
    print("[OK] All curation button handlers present in Details")
else:
    print("[FAIL] Some button handlers missing from Details")

print("\n=== Feature Summary ===")
print("Curation Workflow:")
print("- 3 actions: Approve, Mark for Future Contact, Mark as Bad Fit")
print("- Modal popup requires reason for Future Contact")
print("- Reason appended to notes with timestamp")
print("\nDetails Page:")
print("- Quick curation actions shown for venues needing review (status 0 or 1)")
print("- Same 3-button workflow available")
print("- Same modal validation as other pages")
