$base = "http://localhost:5025"
$body = '{"email":"adminuser@example.com","password":"testUser123!"}'
$login = Invoke-RestMethod -Uri "$base/identity/login" -Method POST -Body $body -ContentType "application/json"
$token = $login.accessToken
$headers = @{ Authorization = "Bearer $token" }

$contacts = Invoke-RestMethod -Uri "$base/api/contact" -Headers $headers
Write-Host "Contacts: $($contacts.value.Count)"

$templates = Invoke-RestMethod -Uri "$base/api/emailtemplate?`$count=true" -Headers $headers
Write-Host "Templates count: $($templates.'@odata.count')"
Write-Host "Templates value count: $($templates.value.Count)"
