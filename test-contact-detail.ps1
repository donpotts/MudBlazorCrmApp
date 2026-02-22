$base = "http://localhost:5025"
$body = '{"email":"adminuser@example.com","password":"testUser123!"}'
$login = Invoke-RestMethod -Uri "$base/identity/login" -Method POST -Body $body -ContentType "application/json"
$token = $login.accessToken
$headers = @{ Authorization = "Bearer $token" }

$contact = Invoke-RestMethod -Uri "$base/api/contact/801" -Headers $headers
$contact | ConvertTo-Json -Depth 3
