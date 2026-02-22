$base = "http://localhost:5025"
$body = '{"email":"adminuser@example.com","password":"testUser123!"}'
$login = Invoke-RestMethod -Uri "$base/identity/login" -Method POST -Body $body -ContentType "application/json"
$token = $login.accessToken
$headers = @{ Authorization = "Bearer $token"; "Content-Type" = "application/json" }

$template = @{
    Name = "Test Template"
    Subject = "Test Subject"
    Body = "Test Body"
    Category = "Sales"
    Description = "A test template"
    IsActive = $true
} | ConvertTo-Json

$result = Invoke-RestMethod -Uri "$base/api/emailtemplate" -Method POST -Body $template -Headers $headers
Write-Host "Created template: Id=$($result.id), Name=$($result.name)"

$templates = Invoke-RestMethod -Uri "$base/api/emailtemplate" -Headers @{ Authorization = "Bearer $token" }
Write-Host "Total templates: $($templates.value.Count)"
