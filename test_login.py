import requests
from bs4 import BeautifulSoup

# Create a session to persist cookies
session = requests.Session()

# Get the login page and extract the anti-forgery token
login_url = "http://localhost:5000/Identity/Account/Login"
response = session.get(login_url)
soup = BeautifulSoup(response.text, 'html.parser')
token = soup.find('input', {'name': '__RequestVerificationToken'})['value']

print(f"Anti-forgery token: {token[:50]}...")

# Perform login
login_data = {
    'Input.Email': 'admin@bookingtracker.local',
    'Input.Password': 'ChangeMe123!',
    'Input.RememberMe': 'false',
    '__RequestVerificationToken': token
}

response = session.post(login_url, data=login_data, allow_redirects=True)
print(f"Login response status: {response.status_code}")
print(f"Final URL after login: {response.url}")

# Check if we can access the dashboard
dashboard_response = session.get("http://localhost:5000/")
print(f"Dashboard response status: {dashboard_response.status_code}")
print(f"Dashboard URL: {dashboard_response.url}")

# Check if dashboard contains venue data
if "Dashboard" in dashboard_response.text:
    print("[OK] Dashboard page loaded")
else:
    print("[FAIL] Dashboard page NOT loaded")

if "venue" in dashboard_response.text.lower():
    print("[OK] Dashboard contains 'venue' text")
else:
    print("[FAIL] Dashboard does NOT contain 'venue' text")

# Check for specific elements
if '/api/venues' in dashboard_response.text:
    print("[OK] Dashboard has /api/venues endpoint reference")
else:
    print("[FAIL] Dashboard missing /api/venues reference")

# Save the cookies for later use
print(f"\nCookies: {list(session.cookies.keys())}")

# Save session for next test
import pickle
with open('/tmp/session.pkl', 'wb') as f:
    pickle.dump(session.cookies, f)
