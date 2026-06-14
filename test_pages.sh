#!/bin/bash

BASE_URL="http://localhost:5000"
RESULTS_FILE="page_test_results.txt"

echo "=== Booking Tracker Page Link Test ===" > $RESULTS_FILE
echo "Test Date: $(date)" >> $RESULTS_FILE
echo "" >> $RESULTS_FILE

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

PASS_COUNT=0
FAIL_COUNT=0

# Arrays to store discovered links
declare -A tested_urls

test_page() {
    local url=$1
    local description=$2

    # Skip if already tested
    if [[ -n "${tested_urls[$url]}" ]]; then
        return
    fi

    tested_urls[$url]=1

    echo -n "Testing: $description ($url)... "

    response=$(curl -s -w "\n%{http_code}" "$url" 2>&1)
    http_code=$(echo "$response" | tail -n1)

    if [ "$http_code" == "200" ]; then
        echo -e "${GREEN}PASS${NC} (HTTP $http_code)"
        echo "✓ PASS: $description - $url (HTTP $http_code)" >> $RESULTS_FILE
        ((PASS_COUNT++))
        echo "$response" | head -n-1
        return 0
    elif [ "$http_code" == "302" ] || [ "$http_code" == "301" ]; then
        echo -e "${YELLOW}REDIRECT${NC} (HTTP $http_code)"
        echo "→ REDIRECT: $description - $url (HTTP $http_code)" >> $RESULTS_FILE
        ((PASS_COUNT++))
        return 0
    else
        echo -e "${RED}FAIL${NC} (HTTP $http_code)"
        echo "✗ FAIL: $description - $url (HTTP $http_code)" >> $RESULTS_FILE
        ((FAIL_COUNT++))
        return 1
    fi
}

extract_links() {
    local html=$1
    # Extract href values from HTML
    echo "$html" | grep -oP 'href="[^"]*"' | sed 's/href="//g' | sed 's/"//g' | sort -u
}

echo ""
echo "=== Phase 1: Testing Main Pages ==="
echo "" >> $RESULTS_FILE
echo "=== Main Pages ===" >> $RESULTS_FILE

# Test main pages and extract links
echo "Fetching Dashboard..."
dashboard_html=$(test_page "$BASE_URL/" "Dashboard")

echo ""
echo "Fetching Venues page..."
venues_html=$(test_page "$BASE_URL/Venues" "Venues List")

echo ""
echo "Fetching Register page..."
register_html=$(test_page "$BASE_URL/Identity/Account/Register" "Register")

echo ""
echo "Fetching Login page..."
login_html=$(test_page "$BASE_URL/Identity/Account/Login" "Login")

echo ""
echo "Fetching Privacy page..."
privacy_html=$(test_page "$BASE_URL/Privacy" "Privacy")

echo ""
echo "=== Phase 2: Extracting All Links ==="
echo "" >> $RESULTS_FILE
echo "=== Discovered Links ===" >> $RESULTS_FILE

# Extract all unique links from all pages
all_links=$(
    (echo "$dashboard_html"; echo "$venues_html"; echo "$register_html"; echo "$login_html"; echo "$privacy_html") | \
    extract_links | \
    grep -v '^#' | \
    grep -v '^javascript:' | \
    grep -v 'https://learn.microsoft.com' | \
    grep -v 'https://aka.ms'
)

echo "Found links:"
echo "$all_links" | while read link; do
    echo "  - $link"
    echo "  - $link" >> $RESULTS_FILE
done

echo ""
echo "=== Phase 3: Testing All Discovered Links ==="
echo "" >> $RESULTS_FILE
echo "=== Link Test Results ===" >> $RESULTS_FILE

# Test each discovered link
echo "$all_links" | while read link; do
    if [[ "$link" == /* ]]; then
        # Relative link starting with /
        full_url="$BASE_URL$link"
        test_page "$full_url" "Link: $link"
    elif [[ "$link" == http* ]]; then
        # Absolute link
        test_page "$link" "External: $link"
    else
        # Relative link without /
        full_url="$BASE_URL/$link"
        test_page "$full_url" "Link: $link"
    fi
done

echo ""
echo "=== Phase 4: Testing Specific Features ==="
echo "" >> $RESULTS_FILE
echo "=== Feature Tests ===" >> $RESULTS_FILE

# Test venue-specific pages that might not be linked
echo ""
echo "Testing Venue CRUD pages..."

# These pages might exist but not be linked yet
test_page "$BASE_URL/Venues/Create" "Venues Create Page"
test_page "$BASE_URL/Venues/Details/1" "Venues Details Page"
test_page "$BASE_URL/Venues/Edit/1" "Venues Edit Page"
test_page "$BASE_URL/Venues/Delete/1" "Venues Delete Page"

# Test outreach pages
echo ""
echo "Testing Outreach pages..."
test_page "$BASE_URL/Outreach" "Outreach List Page"
test_page "$BASE_URL/Outreach/Create" "Outreach Create Page"

# Test curation workflow
echo ""
echo "Testing Curation workflow..."
test_page "$BASE_URL/Venues/Curation" "Curation Workflow Page"

# Test Identity pages
echo ""
echo "Testing Identity pages..."
test_page "$BASE_URL/Identity/Account/Logout" "Logout Page"
test_page "$BASE_URL/Identity/Account/Manage" "Manage Account"
test_page "$BASE_URL/Identity/Account/ForgotPassword" "Forgot Password"

# Summary
echo ""
echo "=== Test Summary ===" | tee -a $RESULTS_FILE
echo "Total Passed: $PASS_COUNT" | tee -a $RESULTS_FILE
echo "Total Failed: $FAIL_COUNT" | tee -a $RESULTS_FILE
echo "" | tee -a $RESULTS_FILE

if [ $FAIL_COUNT -eq 0 ]; then
    echo -e "${GREEN}All page tests passed!${NC}" | tee -a $RESULTS_FILE
    exit 0
else
    echo -e "${RED}Some pages failed. See $RESULTS_FILE for details.${NC}" | tee -a $RESULTS_FILE
    exit 1
fi
