$json = Get-Content "c:\repos\MudBlazorCrmApp\MudBlazorCrmApp\EmailTemplate.Data.json" -Raw
$data = $json | ConvertFrom-Json
Write-Host "Parsed $($data.Count) templates"
Write-Host "First: $($data[0].Name)"
