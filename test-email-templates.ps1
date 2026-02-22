$ErrorActionPreference = "Stop"
$base = "http://localhost:5025"

# Login
$body = '{"email":"adminuser@example.com","password":"testUser123!"}'
$login = Invoke-RestMethod -Uri "$base/identity/login" -Method POST -Body $body -ContentType "application/json"
$token = $login.accessToken
$headers = @{ Authorization = "Bearer $token" }
Write-Host "Login: OK (token length: $($token.Length))"

# Test Email Status
$status = Invoke-RestMethod -Uri "$base/api/email/status" -Headers $headers
Write-Host "Email configured: $($status.isConfigured)"

# Test Email Templates
$templates = Invoke-RestMethod -Uri "$base/api/emailtemplate" -Headers $headers
Write-Host "Email templates: $($templates.value.Count) records"
if ($templates.value.Count -gt 0) {
    $t = $templates.value[0]
    Write-Host "  First: Name=$($t.name), Category=$($t.category), Subject=$($t.subject)"
}

# Test get single template
$tmpl = Invoke-RestMethod -Uri "$base/api/emailtemplate/1" -Headers $headers
Write-Host "Template 1: $($tmpl.name) ($($tmpl.category))"

Write-Host "`nAll email template tests completed!"
