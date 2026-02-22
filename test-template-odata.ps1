$base = "http://localhost:5025"
$body = '{"email":"adminuser@example.com","password":"testUser123!"}'
$login = Invoke-RestMethod -Uri "$base/identity/login" -Method POST -Body $body -ContentType "application/json"
$token = $login.accessToken
$headers = @{ Authorization = "Bearer $token" }

# OData endpoint
$r = Invoke-RestMethod -Uri "$base/odata/EmailTemplate?`$count=true" -Headers $headers
Write-Host "OData templates: $($r.value.Count) (count: $($r.'@odata.count'))"
if ($r.value.Count -gt 0) {
    Write-Host "  First: $($r.value[0].Name) ($($r.value[0].Category))"
}

# API endpoint (raw)
$r2 = Invoke-RestMethod -Uri "$base/api/emailtemplate" -Headers $headers
Write-Host "API templates: $($r2.Count)"
