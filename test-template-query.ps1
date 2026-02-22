$base = "http://localhost:5025"
$body = '{"email":"adminuser@example.com","password":"testUser123!"}'
$login = Invoke-RestMethod -Uri "$base/identity/login" -Method POST -Body $body -ContentType "application/json"
$token = $login.accessToken
$headers = @{ Authorization = "Bearer $token" }

# No filter
$r1 = Invoke-RestMethod -Uri "$base/api/emailtemplate" -Headers $headers
Write-Host "No filter: $($r1.value.Count)"

# With OData count
$r2 = Invoke-WebRequest -Uri "$base/api/emailtemplate?`$count=true" -Headers $headers
Write-Host "Raw response: $($r2.Content.Substring(0, [Math]::Min(500, $r2.Content.Length)))"
