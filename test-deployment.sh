#!/bin/bash

# Test script to verify deployment readiness
echo "=================================="
echo "Deployment Readiness Test"
echo "=================================="
echo ""

PASSED=0
FAILED=0

# Test 1: Check if deploy.sh exists and is executable
echo "[TEST 1] Checking deploy.sh..."
if [ -x deploy.sh ]; then
    echo "✓ PASS: deploy.sh exists and is executable"
    ((PASSED++))
else
    echo "✗ FAIL: deploy.sh is missing or not executable"
    ((FAILED++))
fi

# Test 2: Check if app.zip exists
echo "[TEST 2] Checking app.zip..."
if [ -f app.zip ]; then
    SIZE=$(du -h app.zip | cut -f1)
    echo "✓ PASS: app.zip exists (Size: $SIZE)"
    ((PASSED++))
else
    echo "✗ FAIL: app.zip is missing"
    ((FAILED++))
fi

# Test 3: Check infrastructure files
echo "[TEST 3] Checking infrastructure files..."
if [ -f infrastructure/app-service.bicep ] && [ -f infrastructure/genai-resources.bicep ]; then
    echo "✓ PASS: All infrastructure files present"
    ((PASSED++))
else
    echo "✗ FAIL: Missing infrastructure files"
    ((FAILED++))
fi

# Test 4: Check application builds
echo "[TEST 4] Testing application build..."
cd ExpenseApp
if dotnet build -c Release > /dev/null 2>&1; then
    echo "✓ PASS: Application builds successfully"
    ((PASSED++))
else
    echo "✗ FAIL: Application build failed"
    ((FAILED++))
fi
cd ..

# Test 5: Check documentation
echo "[TEST 5] Checking documentation..."
if [ -f README.md ] && [ -f ARCHITECTURE.md ] && [ -f DEPLOYMENT_CHECKLIST.md ]; then
    echo "✓ PASS: All documentation files present"
    ((PASSED++))
else
    echo "✗ FAIL: Missing documentation files"
    ((FAILED++))
fi

# Test 6: Check for hardcoded secrets
echo "[TEST 6] Scanning for hardcoded secrets..."
if ! grep -r "password.*=.*\".*\"" --include="*.cs" --include="*.json" ExpenseApp/ > /dev/null 2>&1; then
    echo "✓ PASS: No obvious hardcoded secrets found"
    ((PASSED++))
else
    echo "✗ FAIL: Possible hardcoded secrets detected"
    ((FAILED++))
fi

# Test 7: Check Git status
echo "[TEST 7] Checking git status..."
if [ -z "$(git status --porcelain)" ]; then
    echo "✓ PASS: Working directory clean"
    ((PASSED++))
else
    echo "⚠ WARNING: Uncommitted changes detected"
fi

# Summary
echo ""
echo "=================================="
echo "Test Summary"
echo "=================================="
echo "Passed: $PASSED"
echo "Failed: $FAILED"
echo ""

if [ $FAILED -eq 0 ]; then
    echo "✓ All tests passed! Ready for deployment."
    exit 0
else
    echo "✗ Some tests failed. Please fix issues before deployment."
    exit 1
fi
