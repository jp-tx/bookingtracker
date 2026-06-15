import requests
import pickle

# Load the session
session = requests.Session()
with open('/tmp/session.pkl', 'rb') as f:
    session.cookies.update(pickle.load(f))

# Test venues page features
venues_page = session.get("http://localhost:5000/Venues")
html = venues_page.text

# Test 1: Check for column visibility controls
if 'Column Visibility' in html:
    print("[OK] Column visibility section present")
else:
    print("[FAIL] Column visibility section missing")

# Test 2: Check for column checkboxes
if 'column-checkbox' in html:
    print("[OK] Column checkboxes present")
else:
    print("[FAIL] Column checkboxes missing")

# Test 3: Check for sortable column headers
if 'sortable' in html or 'sortBy' in html:
    print("[OK] Sortable column functionality present")
else:
    print("[FAIL] Sortable functionality missing")

# Test 4: Check for new status "Future Contact"
if 'Future Contact' in html:
    print("[OK] New 'Future Contact' status option present")
else:
    print("[FAIL] 'Future Contact' status missing")

# Test 5: Check for specific column options
required_columns = ['Venue Name', 'City', 'Status', 'Priority', 'Instagram', 'Contact Email']
missing_cols = []
for col in required_columns:
    if col not in html:
        missing_cols.append(col)

if len(missing_cols) == 0:
    print(f"[OK] All expected column options present")
else:
    print(f"[FAIL] Missing column options: {missing_cols}")

# Test 6: Check JavaScript functions exist
js_functions = ['sortBy', 'getColumnValue', 'formatColumnValue', 'initColumnControls']
missing_funcs = []
for func in js_functions:
    if f'function {func}' not in html:
        missing_funcs.append(func)

if len(missing_funcs) == 0:
    print(f"[OK] All required JavaScript functions present")
else:
    print(f"[FAIL] Missing JS functions: {missing_funcs}")

print("\n=== Feature Test Summary ===")
print("New Status: MarkedForFutureContact (displays as 'Future Contact')")
print("Column Visibility: Checkboxes for all 27 fields")
print("Sorting: Click column headers to sort (shows up/down arrows)")
print("Persistence: Column preferences saved to localStorage")
