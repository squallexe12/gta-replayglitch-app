param(
    [string]$OutputDirectory = "."
)

$ErrorActionPreference = "Stop"

function Get-CscPath {
    $candidates = @(
        "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe",
        "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
    )

    foreach ($candidate in $candidates) {
        if (Test-Path $candidate) {
            return $candidate
        }
    }

    throw "Could not find csc.exe. Install .NET Framework 4.x Developer Pack or build on a machine with the compiler available."
}

$csc = Get-CscPath
$outputPath = Join-Path $OutputDirectory "ReplayFirewallTool.exe"

if (-not [string]::IsNullOrWhiteSpace($OutputDirectory) -and -not (Test-Path $OutputDirectory)) {
    New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
}

& $csc `
    /nologo `
    /target:winexe `
    /out:$outputPath `
    /win32manifest:ReplayFirewallTool.manifest `
    /win32icon:ReplayFirewallTool.ico `
    /reference:System.Windows.Forms.dll `
    /reference:System.Drawing.dll `
    /reference:Microsoft.CSharp.dll `
    ReplayFirewallTool.cs

if ($LASTEXITCODE -ne 0) {
    throw "Build failed with exit code $LASTEXITCODE."
}

Write-Host ""
Write-Host "Build completed:" -ForegroundColor Green
Write-Host $outputPath
