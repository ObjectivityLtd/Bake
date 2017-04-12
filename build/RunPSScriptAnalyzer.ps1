Get-PackageProvider -Name NuGet -ForceBootstrap

try {
    if (Get-Module PSScriptAnalyzer) {
        Import-Module -Name PSScriptAnalyzer -ErrorAction Stop
    }
    else {
        Install-Module PSScriptAnalyzer -Force
    }
}
catch {
    Write-Error -Message $_
    exit 1
}

try {
    $rules = Get-ScriptAnalyzerRule -Severity Warning,Error -ErrorAction Stop
    $results = Invoke-ScriptAnalyzer -Path "..\src" -IncludeRule $rules.RuleName -Recurse -ErrorAction Stop
    $results
}
catch {
    Write-Error -Message $_
    exit 1
}
if ($results.Count -gt 0) {
    Write-Host "Analysis of your code threw $($results.Count) warnings or errors. Please go back and check your code."
    exit 1
}
else {
    Write-Host 'Awesome code! No issues found!' -Foregroundcolor green
}