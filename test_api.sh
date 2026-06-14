#!/bin/bash

# Booking Tracker API Test Suite
BASE_URL="http://localhost:5000"
RESULTS_FILE="test_results.txt"

echo "=== Booking Tracker API Test Suite ===" > $RESULTS_FILE
echo "Test Date: $(date)" >> $RESULTS_FILE
echo "" >> $RESULTS_FILE

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

PASS_COUNT=0
FAIL_COUNT=0

# Test function
test_endpoint() {
    local method=$1
    local endpoint=$2
    local description=$3
    local data=$4
    local expected_status=$5

    echo -n "Testing: $description... "

    if [ -z "$data" ]; then
        response=$(curl -s -w "\n%{http_code}" -X $method "$BASE_URL$endpoint")
    else
        response=$(curl -s -w "\n%{http_code}" -X $method "$BASE_URL$endpoint" \
            -H "Content-Type: application/json" \
            -d "$data")
    fi

    http_code=$(echo "$response" | tail -n1)
    body=$(echo "$response" | head -n-1)

    if [ "$http_code" == "$expected_status" ]; then
        echo -e "${GREEN}PASS${NC} (HTTP $http_code)"
        echo "✓ PASS: $description (HTTP $http_code)" >> $RESULTS_FILE
        ((PASS_COUNT++))
        echo "$body"
        return 0
    else
        echo -e "${RED}FAIL${NC} (Expected $expected_status, got $http_code)"
        echo "✗ FAIL: $description (Expected $expected_status, got $http_code)" >> $RESULTS_FILE
        echo "  Response: $body" >> $RESULTS_FILE
        ((FAIL_COUNT++))
        return 1
    fi
}

echo ""
echo "=== Testing HTML Endpoints ==="
echo "" >> $RESULTS_FILE
echo "=== HTML Endpoints ===" >> $RESULTS_FILE

test_endpoint "GET" "/" "Dashboard page" "" "200"
test_endpoint "GET" "/Venues" "Venues list page" "" "200"
test_endpoint "GET" "/Identity/Account/Register" "Register page" "" "200"
test_endpoint "GET" "/Identity/Account/Login" "Login page" "" "200"

echo ""
echo "=== Testing API Endpoints ==="
echo "" >> $RESULTS_FILE
echo "=== API Endpoints ===" >> $RESULTS_FILE

# Test Venues API
echo ""
echo "--- Venues API ---"
test_endpoint "GET" "/api/venues" "Get all venues (empty)" "" "200"

# Create a venue
echo ""
echo "Creating test venue..."
create_response=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/venues" \
    -H "Content-Type: application/json" \
    -d '{
        "name": "Test Venue",
        "city": "Nashville",
        "state": "TN",
        "venueType": 0,
        "status": 1,
        "priority": 2,
        "source": 0,
        "researchConfidence": 2,
        "fitScore": 4
    }')

http_code=$(echo "$create_response" | tail -n1)
body=$(echo "$create_response" | head -n-1)

if [ "$http_code" == "201" ]; then
    echo -e "${GREEN}PASS${NC} - Venue created (HTTP $http_code)"
    echo "✓ PASS: Create venue (HTTP $http_code)" >> $RESULTS_FILE
    ((PASS_COUNT++))

    # Extract venue ID
    venue_id=$(echo "$body" | grep -o '"id":[0-9]*' | head -1 | grep -o '[0-9]*')
    echo "Created venue ID: $venue_id"

    # Test GET single venue
    test_endpoint "GET" "/api/venues/$venue_id" "Get venue by ID" "" "200"

    # Test UPDATE venue
    test_endpoint "PUT" "/api/venues/$venue_id" "Update venue" \
        "{\"id\":$venue_id,\"name\":\"Updated Test Venue\",\"city\":\"Nashville\",\"state\":\"TN\",\"venueType\":0,\"status\":2,\"priority\":2,\"source\":0,\"researchConfidence\":2,\"fitScore\":5}" \
        "204"

    # Test GET all venues (should have 1)
    test_endpoint "GET" "/api/venues" "Get all venues (with data)" "" "200"

else
    echo -e "${RED}FAIL${NC} - Failed to create venue (HTTP $http_code)"
    echo "✗ FAIL: Create venue (Expected 201, got $http_code)" >> $RESULTS_FILE
    echo "Response: $body" >> $RESULTS_FILE
    ((FAIL_COUNT++))
    venue_id=""
fi

# Test Outreach API
echo ""
echo "--- Outreach API ---"
test_endpoint "GET" "/api/outreach" "Get all outreach logs (empty)" "" "200"

if [ -n "$venue_id" ]; then
    echo ""
    echo "Creating test outreach log..."
    outreach_response=$(curl -s -w "\n%{http_code}" -X POST "$BASE_URL/api/outreach" \
        -H "Content-Type: application/json" \
        -d "{
            \"venueId\": $venue_id,
            \"date\": \"2026-06-14T12:00:00Z\",
            \"channel\": 0,
            \"direction\": 0,
            \"senderContact\": \"Test User\",
            \"summary\": \"Initial contact\"
        }")

    http_code=$(echo "$outreach_response" | tail -n1)
    body=$(echo "$outreach_response" | head -n-1)

    if [ "$http_code" == "201" ]; then
        echo -e "${GREEN}PASS${NC} - Outreach log created (HTTP $http_code)"
        echo "✓ PASS: Create outreach log (HTTP $http_code)" >> $RESULTS_FILE
        ((PASS_COUNT++))

        outreach_id=$(echo "$body" | grep -o '"id":[0-9]*' | head -1 | grep -o '[0-9]*')
        echo "Created outreach log ID: $outreach_id"

        test_endpoint "GET" "/api/outreach/$outreach_id" "Get outreach log by ID" "" "200"
    else
        echo -e "${RED}FAIL${NC} - Failed to create outreach log (HTTP $http_code)"
        echo "✗ FAIL: Create outreach log (Expected 201, got $http_code)" >> $RESULTS_FILE
        ((FAIL_COUNT++))
    fi
fi

# Test Views API
echo ""
echo "--- Views API ---"
test_endpoint "GET" "/api/views/new-venues-needing-review" "New venues needing review" "" "200"
test_endpoint "GET" "/api/views/approved-targets" "Approved targets" "" "200"
test_endpoint "GET" "/api/views/followups-due-this-week" "Follow-ups due this week" "" "200"
test_endpoint "GET" "/api/views/high-priority-nashville" "High priority Nashville" "" "200"
test_endpoint "GET" "/api/views/out-of-town-targets" "Out of town targets" "" "200"
test_endpoint "GET" "/api/views/no-response" "No response" "" "200"
test_endpoint "GET" "/api/views/booked-venues" "Booked venues" "" "200"
test_endpoint "GET" "/api/views/strong-rebook-candidates" "Strong rebook candidates" "" "200"
test_endpoint "GET" "/api/views/bad-fit-archive" "Bad fit archive" "" "200"

# Test Reminders API
echo ""
echo "--- Reminders API ---"
test_endpoint "GET" "/api/reminders/due" "Reminders due" "" "200"
test_endpoint "GET" "/api/reminders/overdue" "Overdue reminders" "" "200"

# Cleanup - Delete test venue
if [ -n "$venue_id" ]; then
    echo ""
    echo "Cleaning up test data..."
    test_endpoint "DELETE" "/api/venues/$venue_id" "Delete test venue" "" "204"
fi

# Summary
echo ""
echo "=== Test Summary ===" | tee -a $RESULTS_FILE
echo "Total Passed: $PASS_COUNT" | tee -a $RESULTS_FILE
echo "Total Failed: $FAIL_COUNT" | tee -a $RESULTS_FILE
echo "" | tee -a $RESULTS_FILE

if [ $FAIL_COUNT -eq 0 ]; then
    echo -e "${GREEN}All tests passed!${NC}" | tee -a $RESULTS_FILE
    exit 0
else
    echo -e "${RED}Some tests failed. See $RESULTS_FILE for details.${NC}" | tee -a $RESULTS_FILE
    exit 1
fi
