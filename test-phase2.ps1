$ErrorActionPreference = "Stop"
$base = "http://localhost:5025"

# Login
$body = '{"email":"adminuser@example.com","password":"testUser123!"}'
$login = Invoke-RestMethod -Uri "$base/identity/login" -Method POST -Body $body -ContentType "application/json"
$token = $login.accessToken
$headers = @{ Authorization = "Bearer $token" }
Write-Host "Login: OK (token length: $($token.Length))"

# Test Contacts with enriched fields
$contacts = Invoke-RestMethod -Uri "$base/api/contact" -Headers $headers
Write-Host "Contacts: $($contacts.value.Count) records"
if ($contacts.value.Count -gt 0) {
    $c = $contacts.value[0]
    Write-Host "  First: Company=$($c.company), Title=$($c.title), Status=$($c.status), Pref=$($c.preferredContactMethod)"
}

# Test Notifications - generate
$genCount = Invoke-RestMethod -Uri "$base/api/notification/generate" -Method POST -Headers $headers
Write-Host "Notifications generated: $genCount"

# Test Notifications - unread count
$unread = Invoke-RestMethod -Uri "$base/api/notification/unread-count" -Headers $headers
Write-Host "Unread notifications: $unread"

# Test Attachments endpoint
$atts = Invoke-RestMethod -Uri "$base/api/attachment/entity/Customer/301" -Headers $headers
Write-Host "Attachments for Customer 301: $($atts.Count)"

# Test Activities
$activities = Invoke-RestMethod -Uri "$base/api/activity" -Headers $headers
Write-Host "Activities: $($activities.value.Count) records"

# Test Tasks
$tasks = Invoke-RestMethod -Uri "$base/api/todotask" -Headers $headers
Write-Host "Tasks: $($tasks.value.Count) records"

# Test new Notification endpoint
$notifList = Invoke-RestMethod -Uri "$base/api/notification" -Headers $headers
Write-Host "Notification list: $($notifList.value.Count) items"

Write-Host "`nAll Phase 2 API tests completed!"
