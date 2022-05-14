[CmdletBinding()]
Param(
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)

Write-Output "PowerShell $($PSVersionTable.PSEdition) version $($PSVersionTable.PSVersion)"

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { Write-Error $_ -ErrorAction Continue; exit 1 }
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

###########################################################################
# CONFIGURATION
###########################################################################

$BuildProjectFile = "$PSScriptRoot\Build\_build.csproj"
$TempDirectory = "$PSScriptRoot\.nuke\temp"

$DotNetGlobalFile = "$PSScriptRoot\global.json"
$DotNetInstallUrl = "https://dot.net/v1/dotnet-install.ps1"
$DotNetChannel = "Current"

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = 1
$env:DOTNET_CLI_TELEMETRY_OPTOUT = 1
$env:DOTNET_MULTILEVEL_LOOKUP = 0
$env:DOTNET_ROLL_FORWARD = "Major"
$env:NUKE_TELEMETRY_OPTOUT = 1

###########################################################################
# EXECUTION
###########################################################################

function ExecSafe([scriptblock] $cmd) {
    & $cmd
    if ($LASTEXITCODE) { exit $LASTEXITCODE }
}

$DotNetDirectory = "$TempDirectory\dotnet-win"

# Download install script
$DotNetInstallFile = "$TempDirectory\dotnet-install.ps1"
New-Item -ItemType Directory -Path $TempDirectory -Force | Out-Null
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
(New-Object System.Net.WebClient).DownloadFile($DotNetInstallUrl, $DotNetInstallFile)

# If dotnet CLI is installed globally and it matches requested version, use for execution
if ($null -ne (Get-Command "dotnet" -ErrorAction SilentlyContinue) -and `
     $(dotnet --version) -and $LASTEXITCODE -eq 0) {
    $env:DOTNET_EXE = (Get-Command "dotnet").Path

    if ($env:GITHUB_ACTIONS) {
        . $DotNetInstallFile -Version 3.1.419 -NoPath -InstallDir (Split-Path($env:DOTNET_EXE))
        . $DotNetInstallFile -Version 2.1.818 -NoPath -InstallDir (Split-Path($env:DOTNET_EXE))
    }
}
else {

    # If global.json exists, load expected version
    if (Test-Path $DotNetGlobalFile) {
        $DotNetGlobal = $(Get-Content $DotNetGlobalFile | Out-String | ConvertFrom-Json)
        if ($DotNetGlobal.PSObject.Properties["sdk"] -and $DotNetGlobal.sdk.PSObject.Properties["version"]) {
            $DotNetVersion = $DotNetGlobal.sdk.version
        }
    }

    # Install by channel or version
    if (!(Test-Path variable:DotNetVersion)) {
        ExecSafe { & powershell $DotNetInstallFile -InstallDir $DotNetDirectory -Channel $DotNetChannel -NoPath }
    } else {
        ExecSafe { & powershell $DotNetInstallFile -InstallDir $DotNetDirectory -Version $DotNetVersion -NoPath }
    }

if ($env:GITHUB_ACTIONS) {
    ExecSafe { & powershell $DotNetInstallFile -InstallDir $DotNetDirectory -Version 2.1.818 -NoPath }
    ExecSafe { & powershell $DotNetInstallFile -InstallDir $DotNetDirectory -Version 3.1.419 -NoPath }
}
    
    $env:DOTNET_EXE = "$DotNetDirectory\dotnet.exe"
}

if ($env:GITHUB_ACTIONS) {
    . $env:DOTNET_EXE --info
}

Write-Output "Microsoft (R) .NET SDK version $(& $env:DOTNET_EXE --version) (location: $env:DOTNET_EXE)"

ExecSafe { & $env:DOTNET_EXE build $BuildProjectFile /nodeReuse:false /p:UseSharedCompilation=false -nologo -clp:NoSummary --verbosity quiet }
ExecSafe { & $env:DOTNET_EXE run --project $BuildProjectFile --no-build -- $BuildArguments }