#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

<#
.SYNOPSIS
    Installs dotnet cli
.DESCRIPTION
    Installs dotnet cli. If dotnet installation already exists in the given directory
    it will update it only if the requested version differs from the one already installed.
.PARAMETER Channel
    Default: LTS
    Download from the Channel specified. Possible values:
    - Current - most current release
    - LTS - most current supported release
    - 2-part version in a format A.B - represents a specific release
          examples: 2.0, 1.0
    - 3-part version in a format A.B.Cxx - represents a specific SDK release
          examples: 5.0.1xx, 5.0.2xx
          Supported since 5.0 release
    Note: The version parameter overrides the channel parameter when any version other than 'latest' is used.
.PARAMETER Quality
    Download the latest build of specified quality in the channel. The possible values are: daily, signed, validated, preview, GA.
    Works only in combination with channel. Not applicable for current and LTS channels and will be ignored if those channels are used. 
    For SDK use channel in A.B.Cxx format: using quality together with channel in A.B format is not supported.
    Supported since 5.0 release.
    Note: The version parameter overrides the channel parameter when any version other than 'latest' is used, and therefore overrides the quality.     
.PARAMETER Version
    Default: latest
    Represents a build version on specific channel. Possible values:
    - latest - most latest build on specific channel
    - 3-part version in a format A.B.C - represents specific version of build
          examples: 2.0.0-preview2-006120, 1.1.0
.PARAMETER Internal
    Download internal builds. Requires providing credentials via -FeedCredential parameter.
.PARAMETER FeedCredential
    Token to access Azure feed. Used as a query string to append to the Azure feed.
    This parameter typically is not specified.
.PARAMETER InstallDir
    Default: %LocalAppData%\Microsoft\dotnet
    Path to where to install dotnet. Note that binaries will be placed directly in a given directory.
.PARAMETER Architecture
    Default: <auto> - this value represents currently running OS architecture
    Architecture of dotnet binaries to be installed.
    Possible values are: <auto>, amd64, x64, x86, arm64, arm
.PARAMETER SharedRuntime
    This parameter is obsolete and may be removed in a future version of this script.
    The recommended alternative is '-Runtime dotnet'.
    Installs just the shared runtime bits, not the entire SDK.
.PARAMETER Runtime
    Installs just a shared runtime, not the entire SDK.
    Possible values:
        - dotnet     - the Microsoft.NETCore.App shared runtime
        - aspnetcore - the Microsoft.AspNetCore.App shared runtime
        - windowsdesktop - the Microsoft.WindowsDesktop.App shared runtime
.PARAMETER DryRun
    If set it will not perform installation but instead display what command line to use to consistently install
    currently requested version of dotnet cli. In example if you specify version 'latest' it will display a link
    with specific version so that this command can be used deterministicly in a build script.
    It also displays binaries location if you prefer to install or download it yourself.
.PARAMETER NoPath
    By default this script will set environment variable PATH for the current process to the binaries folder inside installation folder.
    If set it will display binaries location but not set any environment variable.
.PARAMETER Verbose
    Displays diagnostics information.
.PARAMETER AzureFeed
    Default: https://dotnetcli.azureedge.net/dotnet
    For internal use only.
    Allows using a different storage to download SDK archives from.
    This parameter is only used if $NoCdn is false.
.PARAMETER UncachedFeed
    For internal use only.
    Allows using a different storage to download SDK archives from.
    This parameter is only used if $NoCdn is true.
.PARAMETER ProxyAddress
    If set, the installer will use the proxy when making web requests
.PARAMETER ProxyUseDefaultCredentials
    Default: false
    Use default credentials, when using proxy address.
.PARAMETER ProxyBypassList
    If set with ProxyAddress, will provide the list of comma separated urls that will bypass the proxy
.PARAMETER SkipNonVersionedFiles
    Default: false
    Skips installing non-versioned files if they already exist, such as dotnet.exe.
.PARAMETER NoCdn
    Disable downloading from the Azure CDN, and use the uncached feed directly.
.PARAMETER JSonFile
    Determines the SDK version from a user specified global.json file
    Note: global.json must have a value for 'SDK:Version'
.PARAMETER DownloadTimeout
    Determines timeout duration in seconds for dowloading of the SDK file
    Default: 1200 seconds (20 minutes)
#>
[cmdletbinding()]
param(
   [string]$Channel="LTS",
   [string]$Quality,
   [string]$Version="Latest",
   [switch]$Internal,
   [string]$JSonFile,
   [Alias('i')][string]$InstallDir="<auto>",
   [string]$Architecture="<auto>",
   [string]$Runtime,
   [Obsolete("This parameter may be removed in a future version of this script. The recommended alternative is '-Runtime dotnet'.")]
   [switch]$SharedRuntime,
   [switch]$DryRun,
   [switch]$NoPath,
   [string]$AzureFeed,
   [string]$UncachedFeed,
   [string]$FeedCredential,
   [string]$ProxyAddress,
   [switch]$ProxyUseDefaultCredentials,
   [string[]]$ProxyBypassList=@(),
   [switch]$SkipNonVersionedFiles,
   [switch]$NoCdn,
   [int]$DownloadTimeout=1200
)

Set-StrictMode -Version Latest
$ErrorActionPreference="Stop"
$ProgressPreference="SilentlyContinue"

function Say($str) {
    try {
        Write-Host "dotnet-install: $str"
    }
    catch {
        # Some platforms cannot utilize Write-Host (Azure Functions, for instance). Fall back to Write-Output
        Write-Output "dotnet-install: $str"
    }
}

function Say-Warning($str) {
    try {
        Write-Warning "dotnet-install: $str"
    }
    catch {
        # Some platforms cannot utilize Write-Warning (Azure Functions, for instance). Fall back to Write-Output
        Write-Output "dotnet-install: Warning: $str"
    }
}

# Writes a line with error style settings.
# Use this function to show a human-readable comment along with an exception.
function Say-Error($str) {
    try {
        # Write-Error is quite oververbose for the purpose of the function, let's write one line with error style settings.
        $Host.UI.WriteErrorLine("dotnet-install: $str")
    }
    catch {
        Write-Output "dotnet-install: Error: $str"
    }
}

function Say-Verbose($str) {
    try {
        Write-Verbose "dotnet-install: $str"
    }
    catch {
        # Some platforms cannot utilize Write-Verbose (Azure Functions, for instance). Fall back to Write-Output
        Write-Output "dotnet-install: $str"
    }
}

function Say-Invocation($Invocation) {
    $command = $Invocation.MyCommand;
    $args = (($Invocation.BoundParameters.Keys | foreach { "-$_ `"$($Invocation.BoundParameters[$_])`"" }) -join " ")
    Say-Verbose "$command $args"
}

function Invoke-With-Retry([ScriptBlock]$ScriptBlock, [System.Threading.CancellationToken]$cancellationToken = [System.Threading.CancellationToken]::None, [int]$MaxAttempts = 3, [int]$SecondsBetweenAttempts = 1) {
    $Attempts = 0

    while ($true) {
        try {
            return & $ScriptBlock
        }
        catch {
            $Attempts++
            if (($Attempts -lt $MaxAttempts) -and -not $cancellationToken.IsCancellationRequested) {
                Start-Sleep $SecondsBetweenAttempts
            }
            else {
                throw
            }
        }
    }
}

function Get-Machine-Architecture() {
    Say-Invocation $MyInvocation

    # On PS x86, PROCESSOR_ARCHITECTURE reports x86 even on x64 systems.
    # To get the correct architecture, we need to use PROCESSOR_ARCHITEW6432.
    # PS x64 doesn't define this, so we fall back to PROCESSOR_ARCHITECTURE.
    # Possible values: amd64, x64, x86, arm64, arm
    if( $ENV:PROCESSOR_ARCHITEW6432 -ne $null ) {
        return $ENV:PROCESSOR_ARCHITEW6432
    }

    return $ENV:PROCESSOR_ARCHITECTURE
}

function Get-CLIArchitecture-From-Architecture([string]$Architecture) {
    Say-Invocation $MyInvocation

    if ($Architecture -eq "<auto>") {
        $Architecture = Get-Machine-Architecture
    }

    switch ($Architecture.ToLowerInvariant()) {
        { ($_ -eq "amd64") -or ($_ -eq "x64") } { return "x64" }
        { $_ -eq "x86" } { return "x86" }
        { $_ -eq "arm" } { return "arm" }
        { $_ -eq "arm64" } { return "arm64" }
        default { throw "Architecture '$Architecture' not supported. If you think this is a bug, report it at https://github.com/dotnet/install-scripts/issues" }
    }
}

function ValidateFeedCredential([string] $FeedCredential)
{
    if ($Internal -and [string]::IsNullOrWhitespace($FeedCredential)) {
        $message = "Provide credentials via -FeedCredential parameter."
        if ($DryRun) {
            Say-Warning "$message"
        } else {
            throw "$message"
        }
    }
    
    #FeedCredential should start with "?", for it to be added to the end of the link.
    #adding "?" at the beginning of the FeedCredential if needed.
    if ((![string]::IsNullOrWhitespace($FeedCredential)) -and ($FeedCredential[0] -ne '?')) {
        $FeedCredential = "?" + $FeedCredential
    }

    return $FeedCredential
}
function Get-NormalizedQuality([string]$Quality) {
    Say-Invocation $MyInvocation

    if ([string]::IsNullOrEmpty($Quality)) {
        return ""
    }

    switch ($Quality) {
        { @("daily", "signed", "validated", "preview") -contains $_ } { return $Quality.ToLowerInvariant() }
        #ga quality is available without specifying quality, so normalizing it to empty
        { $_ -eq "ga" } { return "" }
        default { throw "'$Quality' is not a supported value for -Quality option. Supported values are: daily, signed, validated, preview, ga. If you think this is a bug, report it at https://github.com/dotnet/install-scripts/issues." }
    }
}

function Get-NormalizedChannel([string]$Channel) {
    Say-Invocation $MyInvocation

    if ([string]::IsNullOrEmpty($Channel)) {
        return ""
    }

    if ($Channel.StartsWith('release/')) {
        Say-Warning 'Using branch name with -Channel option is no longer supported with newer releases. Use -Quality option with a channel in X.Y format instead, such as "-Channel 5.0 -Quality Daily."'
    }

    switch ($Channel) {
        { $_ -eq "lts" } { return "LTS" }
        { $_ -eq "current" } { return "current" }
        default { return $Channel.ToLowerInvariant() }
    }
}

function Get-NormalizedProduct([string]$Runtime) {
    Say-Invocation $MyInvocation

    switch ($Runtime) {
        { $_ -eq "dotnet" } { return "dotnet-runtime" }
        { $_ -eq "aspnetcore" } { return "aspnetcore-runtime" }
        { $_ -eq "windowsdesktop" } { return "windowsdesktop-runtime" }
        { [string]::IsNullOrEmpty($_) } { return "dotnet-sdk" }
        default { throw "'$Runtime' is not a supported value for -Runtime option, supported values are: dotnet, aspnetcore, windowsdesktop. If you think this is a bug, report it at https://github.com/dotnet/install-scripts/issues." }
    }
}


# The version text returned from the feeds is a 1-line or 2-line string:
# For the SDK and the dotnet runtime (2 lines):
# Line 1: # commit_hash
# Line 2: # 4-part version
# For the aspnetcore runtime (1 line):
# Line 1: # 4-part version
function Get-Version-From-LatestVersion-File-Content([string]$VersionText) {
    Say-Invocation $MyInvocation

    $Data = -split $VersionText

    $VersionInfo = @{
        CommitHash = $(if ($Data.Count -gt 1) { $Data[0] })
        Version = $Data[-1] # last line is always the version number.
    }
    return $VersionInfo
}

function Load-Assembly([string] $Assembly) {
    try {
        Add-Type -Assembly $Assembly | Out-Null
    }
    catch {
        # On Nano Server, Powershell Core Edition is used.  Add-Type is unable to resolve base class assemblies because they are not GAC'd.
        # Loading the base class assemblies is not unnecessary as the types will automatically get resolved.
    }
}

function GetHTTPResponse([Uri] $Uri, [bool]$HeaderOnly, [bool]$DisableRedirect, [bool]$DisableFeedCredential)
{
    $cts = New-Object System.Threading.CancellationTokenSource

    $downloadScript = {

        $HttpClient = $null

        try {
            # HttpClient is used vs Invoke-WebRequest in order to support Nano Server which doesn't support the Invoke-WebRequest cmdlet.
            Load-Assembly -Assembly System.Net.Http

            if(-not $ProxyAddress) {
                try {
                    # Despite no proxy being explicitly specified, we may still be behind a default proxy
                    $DefaultProxy = [System.Net.WebRequest]::DefaultWebProxy;
                    if($DefaultProxy -and (-not $DefaultProxy.IsBypassed($Uri))) {
                        if ($null -ne $DefaultProxy.GetProxy($Uri)) {
                            $ProxyAddress = $DefaultProxy.GetProxy($Uri).OriginalString
                        } else {
                            $ProxyAddress = $null
                        }
                        $ProxyUseDefaultCredentials = $true
                    }
                } catch {
                    # Eat the exception and move forward as the above code is an attempt
                    #    at resolving the DefaultProxy that may not have been a problem.
                    $ProxyAddress = $null
                    Say-Verbose("Exception ignored: $_.Exception.Message - moving forward...")
                }
            }

            $HttpClientHandler = New-Object System.Net.Http.HttpClientHandler
            if($ProxyAddress) {
                $HttpClientHandler.Proxy =  New-Object System.Net.WebProxy -Property @{
                    Address=$ProxyAddress;
                    UseDefaultCredentials=$ProxyUseDefaultCredentials;
                    BypassList = $ProxyBypassList;
                }
            }       
            if ($DisableRedirect)
            {
                $HttpClientHandler.AllowAutoRedirect = $false
            }
            $HttpClient = New-Object System.Net.Http.HttpClient -ArgumentList $HttpClientHandler

            # Default timeout for HttpClient is 100s.  For a 50 MB download this assumes 500 KB/s average, any less will time out
            # Defaulting to 20 minutes allows it to work over much slower connections.
            $HttpClient.Timeout = New-TimeSpan -Seconds $DownloadTimeout

            if ($HeaderOnly){
                $completionOption = [System.Net.Http.HttpCompletionOption]::ResponseHeadersRead
            }
            else {
                $completionOption = [System.Net.Http.HttpCompletionOption]::ResponseContentRead
            }

            if ($DisableFeedCredential) {
                $UriWithCredential = $Uri
            }
            else {
                $UriWithCredential = "${Uri}${FeedCredential}"
            }

            $Task = $HttpClient.GetAsync("$UriWithCredential", $completionOption).ConfigureAwait("false");
            $Response = $Task.GetAwaiter().GetResult();

            if (($null -eq $Response) -or ((-not $HeaderOnly) -and (-not ($Response.IsSuccessStatusCode)))) {
                # The feed credential is potentially sensitive info. Do not log FeedCredential to console output.
                $DownloadException = [System.Exception] "Unable to download $Uri."

                if ($null -ne $Response) {
                    $DownloadException.Data["StatusCode"] = [int] $Response.StatusCode
                    $DownloadException.Data["ErrorMessage"] = "Unable to download $Uri. Returned HTTP status code: " + $DownloadException.Data["StatusCode"]

                    if (404 -eq [int] $Response.StatusCode)
                    {
                        $cts.Cancel()
                    }
                }

                throw $DownloadException
            }

            return $Response
        }
        catch [System.Net.Http.HttpRequestException] {
            $DownloadException = [System.Exception] "Unable to download $Uri."

            # Pick up the exception message and inner exceptions' messages if they exist
            $CurrentException = $PSItem.Exception
            $ErrorMsg = $CurrentException.Message + "`r`n"
            while ($CurrentException.InnerException) {
              $CurrentException = $CurrentException.InnerException
              $ErrorMsg += $CurrentException.Message + "`r`n"
            }

            # Check if there is an issue concerning TLS.
            if ($ErrorMsg -like "*SSL/TLS*") {
                $ErrorMsg += "Ensure that TLS 1.2 or higher is enabled to use this script.`r`n"
            }

            $DownloadException.Data["ErrorMessage"] = $ErrorMsg
            throw $DownloadException
        }
        finally {
             if ($null -ne $HttpClient) {
                $HttpClient.Dispose()
            }
        }
    }

    try {
        return Invoke-With-Retry $downloadScript $cts.Token
    }
    finally
    {
        if ($null -ne $cts)
        {
            $cts.Dispose()
        }
    }
}

function Get-Version-From-LatestVersion-File([string]$AzureFeed, [string]$Channel) {
    Say-Invocation $MyInvocation

    $VersionFileUrl = $null
    if ($Runtime -eq "dotnet") {
        $VersionFileUrl = "$AzureFeed/Runtime/$Channel/latest.version"
    }
    elseif ($Runtime -eq "aspnetcore") {
        $VersionFileUrl = "$AzureFeed/aspnetcore/Runtime/$Channel/latest.version"
    }
    elseif ($Runtime -eq "windowsdesktop") {
        $VersionFileUrl = "$AzureFeed/WindowsDesktop/$Channel/latest.version"
    }
    elseif (-not $Runtime) {
        $VersionFileUrl = "$AzureFeed/Sdk/$Channel/latest.version"
    }
    else {
        throw "Invalid value for `$Runtime"
    }

    Say-Verbose "Constructed latest.version URL: $VersionFileUrl"

    try {
        $Response = GetHTTPResponse -Uri $VersionFileUrl
    }
    catch {
        Say-Verbose "Failed to download latest.version file."
        throw
    }
    $StringContent = $Response.Content.ReadAsStringAsync().Result

    switch ($Response.Content.Headers.ContentType) {
        { ($_ -eq "application/octet-stream") } { $VersionText = $StringContent }
        { ($_ -eq "text/plain") } { $VersionText = $StringContent }
        { ($_ -eq "text/plain; charset=UTF-8") } { $VersionText = $StringContent }
        default { throw "``$Response.Content.Headers.ContentType`` is an unknown .version file content type." }
    }

    $VersionInfo = Get-Version-From-LatestVersion-File-Content $VersionText

    return $VersionInfo
}

function Parse-Jsonfile-For-Version([string]$JSonFile) {
    Say-Invocation $MyInvocation

    If (-Not (Test-Path $JSonFile)) {
        throw "Unable to find '$JSonFile'"
    }
    try {
        $JSonContent = Get-Content($JSonFile) -Raw | ConvertFrom-Json | Select-Object -expand "sdk" -ErrorAction SilentlyContinue
    }
    catch {
        Say-Error "Json file unreadable: '$JSonFile'"
        throw
    }
    if ($JSonContent) {
        try {
            $JSonContent.PSObject.Properties | ForEach-Object {
                $PropertyName = $_.Name
                if ($PropertyName -eq "version") {
                    $Version = $_.Value
                    Say-Verbose "Version = $Version"
                }
            }
        }
        catch {
            Say-Error "Unable to parse the SDK node in '$JSonFile'"
            throw
        }
    }
    else {
        throw "Unable to find the SDK node in '$JSonFile'"
    }
    If ($Version -eq $null) {
        throw "Unable to find the SDK:version node in '$JSonFile'"
    }
    return $Version
}

function Get-Specific-Version-From-Version([string]$AzureFeed, [string]$Channel, [string]$Version, [string]$JSonFile) {
    Say-Invocation $MyInvocation

    if (-not $JSonFile) {
        if ($Version.ToLowerInvariant() -eq "latest") {
            $LatestVersionInfo = Get-Version-From-LatestVersion-File -AzureFeed $AzureFeed -Channel $Channel
            return $LatestVersionInfo.Version
        }
        else {
            return $Version 
        }
    }
    else {
        return Parse-Jsonfile-For-Version $JSonFile
    }
}

function Get-Download-Link([string]$AzureFeed, [string]$SpecificVersion, [string]$CLIArchitecture) {
    Say-Invocation $MyInvocation

    # If anything fails in this lookup it will default to $SpecificVersion
    $SpecificProductVersion = Get-Product-Version -AzureFeed $AzureFeed -SpecificVersion $SpecificVersion

    if ($Runtime -eq "dotnet") {
        $PayloadURL = "$AzureFeed/Runtime/$SpecificVersion/dotnet-runtime-$SpecificProductVersion-win-$CLIArchitecture.zip"
    }
    elseif ($Runtime -eq "aspnetcore") {
        $PayloadURL = "$AzureFeed/aspnetcore/Runtime/$SpecificVersion/aspnetcore-runtime-$SpecificProductVersion-win-$CLIArchitecture.zip"
    }
    elseif ($Runtime -eq "windowsdesktop") {
        # The windows desktop runtime is part of the core runtime layout prior to 5.0
        $PayloadURL = "$AzureFeed/Runtime/$SpecificVersion/windowsdesktop-runtime-$SpecificProductVersion-win-$CLIArchitecture.zip"
        if ($SpecificVersion -match '^(\d+)\.(.*)$')
        {
            $majorVersion = [int]$Matches[1]
            if ($majorVersion -ge 5)
            {
                $PayloadURL = "$AzureFeed/WindowsDesktop/$SpecificVersion/windowsdesktop-runtime-$SpecificProductVersion-win-$CLIArchitecture.zip"
            }
        }
    }
    elseif (-not $Runtime) {
        $PayloadURL = "$AzureFeed/Sdk/$SpecificVersion/dotnet-sdk-$SpecificProductVersion-win-$CLIArchitecture.zip"
    }
    else {
        throw "Invalid value for `$Runtime"
    }

    Say-Verbose "Constructed primary named payload URL: $PayloadURL"

    return $PayloadURL, $SpecificProductVersion
}

function Get-LegacyDownload-Link([string]$AzureFeed, [string]$SpecificVersion, [string]$CLIArchitecture) {
    Say-Invocation $MyInvocation

    if (-not $Runtime) {
        $PayloadURL = "$AzureFeed/Sdk/$SpecificVersion/dotnet-dev-win-$CLIArchitecture.$SpecificVersion.zip"
    }
    elseif ($Runtime -eq "dotnet") {
        $PayloadURL = "$AzureFeed/Runtime/$SpecificVersion/dotnet-win-$CLIArchitecture.$SpecificVersion.zip"
    }
    else {
        return $null
    }

    Say-Verbose "Constructed legacy named payload URL: $PayloadURL"

    return $PayloadURL
}

function Get-Product-Version([string]$AzureFeed, [string]$SpecificVersion, [string]$PackageDownloadLink) {
    Say-Invocation $MyInvocation

    # Try to get the version number, using the productVersion.txt file located next to the installer file.
    $ProductVersionTxtURLs = (Get-Product-Version-Url $AzureFeed $SpecificVersion $PackageDownloadLink -Flattened $true),
                             (Get-Product-Version-Url $AzureFeed $SpecificVersion $PackageDownloadLink -Flattened $false)
    
    Foreach ($ProductVersionTxtURL in $ProductVersionTxtURLs) {
        Say-Verbose "Checking for the existence of $ProductVersionTxtURL"

        try {
            $productVersionResponse = GetHTTPResponse($productVersionTxtUrl)

            if ($productVersionResponse.StatusCode -eq 200) {
                $productVersion = $productVersionResponse.Content.ReadAsStringAsync().Result.Trim()
                if ($productVersion -ne $SpecificVersion)
                {
                    Say "Using alternate version $productVersion found in $ProductVersionTxtURL"
                }
                return $productVersion
            }
            else {
                Say-Verbose "Got StatusCode $($productVersionResponse.StatusCode) when trying to get productVersion.txt at $productVersionTxtUrl."
            }
        } 
        catch {
            Say-Verbose "Could not read productVersion.txt at $productVersionTxtUrl (Exception: '$($_.Exception.Message)'. )"
        }
    }

    # Getting the version number with productVersion.txt has failed. Try parsing the download link for a version number.
    if ([string]::IsNullOrEmpty($PackageDownloadLink))
    {
        Say-Verbose "Using the default value '$SpecificVersion' as the product version."
        return $SpecificVersion
    }

    $productVersion = Get-ProductVersionFromDownloadLink $PackageDownloadLink $SpecificVersion
    return $productVersion
}

function Get-Product-Version-Url([string]$AzureFeed, [string]$SpecificVersion, [string]$PackageDownloadLink, [bool]$Flattened) {
    Say-Invocation $MyInvocation

    $majorVersion=$null
    if ($SpecificVersion -match '^(\d+)\.(.*)') {
        $majorVersion = $Matches[1] -as[int]
    }

    $pvFileName='productVersion.txt'
    if($Flattened) {
        if(-not $Runtime) {
            $pvFileName='sdk-productVersion.txt'
        }
        elseif($Runtime -eq "dotnet") {
            $pvFileName='runtime-productVersion.txt'
        }
        else {
            $pvFileName="$Runtime-productVersion.txt"
        }
    }

    if ([string]::IsNullOrEmpty($PackageDownloadLink)) {
        if ($Runtime -eq "dotnet") {
            $ProductVersionTxtURL = "$AzureFeed/Runtime/$SpecificVersion/$pvFileName"
        }
        elseif ($Runtime -eq "aspnetcore") {
            $ProductVersionTxtURL = "$AzureFeed/aspnetcore/Runtime/$SpecificVersion/$pvFileName"
        }
        elseif ($Runtime -eq "windowsdesktop") {
            # The windows desktop runtime is part of the core runtime layout prior to 5.0
            $ProductVersionTxtURL = "$AzureFeed/Runtime/$SpecificVersion/$pvFileName"
            if ($majorVersion -ne $null -and $majorVersion -ge 5) {
                $ProductVersionTxtURL = "$AzureFeed/WindowsDesktop/$SpecificVersion/$pvFileName"
            }
        }
        elseif (-not $Runtime) {
            $ProductVersionTxtURL = "$AzureFeed/Sdk/$SpecificVersion/$pvFileName"
        }
        else {
            throw "Invalid value '$Runtime' specified for `$Runtime"
        }
    }
    else {
        $ProductVersionTxtURL = $PackageDownloadLink.Substring(0, $PackageDownloadLink.LastIndexOf("/"))  + "/$pvFileName"
    }

    Say-Verbose "Constructed productVersion link: $ProductVersionTxtURL"

    return $ProductVersionTxtURL
}

function Get-ProductVersionFromDownloadLink([string]$PackageDownloadLink, [string]$SpecificVersion)
{
    Say-Invocation $MyInvocation

    #product specific version follows the product name
    #for filename 'dotnet-sdk-3.1.404-win-x64.zip': the product version is 3.1.400
    $filename = $PackageDownloadLink.Substring($PackageDownloadLink.LastIndexOf("/") + 1)
    $filenameParts = $filename.Split('-')
    if ($filenameParts.Length -gt 2)
    {
        $productVersion = $filenameParts[2]
        Say-Verbose "Extracted product version '$productVersion' from download link '$PackageDownloadLink'."
    }
    else {
        Say-Verbose "Using the default value '$SpecificVersion' as the product version."
        $productVersion = $SpecificVersion
    }
    return $productVersion 
}

function Get-User-Share-Path() {
    Say-Invocation $MyInvocation

    $InstallRoot = $env:DOTNET_INSTALL_DIR
    if (!$InstallRoot) {
        $InstallRoot = "$env:LocalAppData\Microsoft\dotnet"
    }
    return $InstallRoot
}

function Resolve-Installation-Path([string]$InstallDir) {
    Say-Invocation $MyInvocation

    if ($InstallDir -eq "<auto>") {
        return Get-User-Share-Path
    }
    return $InstallDir
}

function Is-Dotnet-Package-Installed([string]$InstallRoot, [string]$RelativePathToPackage, [string]$SpecificVersion) {
    Say-Invocation $MyInvocation

    $DotnetPackagePath = Join-Path -Path $InstallRoot -ChildPath $RelativePathToPackage | Join-Path -ChildPath $SpecificVersion
    Say-Verbose "Is-Dotnet-Package-Installed: DotnetPackagePath=$DotnetPackagePath"
    return Test-Path $DotnetPackagePath -PathType Container
}

function Get-Absolute-Path([string]$RelativeOrAbsolutePath) {
    # Too much spam
    # Say-Invocation $MyInvocation

    return $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($RelativeOrAbsolutePath)
}

function Get-Path-Prefix-With-Version($path) {
    # example path with regex: shared/1.0.0-beta-12345/somepath
    $match = [regex]::match($path, "/\d+\.\d+[^/]+/")
    if ($match.Success) {
        return $entry.FullName.Substring(0, $match.Index + $match.Length)
    }

    return $null
}

function Get-List-Of-Directories-And-Versions-To-Unpack-From-Dotnet-Package([System.IO.Compression.ZipArchive]$Zip, [string]$OutPath) {
    Say-Invocation $MyInvocation

    $ret = @()
    foreach ($entry in $Zip.Entries) {
        $dir = Get-Path-Prefix-With-Version $entry.FullName
        if ($null -ne $dir) {
            $path = Get-Absolute-Path $(Join-Path -Path $OutPath -ChildPath $dir)
            if (-Not (Test-Path $path -PathType Container)) {
                $ret += $dir
            }
        }
    }

    $ret = $ret | Sort-Object | Get-Unique

    $values = ($ret | foreach { "$_" }) -join ";"
    Say-Verbose "Directories to unpack: $values"

    return $ret
}

# Example zip content and extraction algorithm:
# Rule: files if extracted are always being extracted to the same relative path locally
# .\
#       a.exe   # file does not exist locally, extract
#       b.dll   # file exists locally, override only if $OverrideFiles set
#       aaa\    # same rules as for files
#           ...
#       abc\1.0.0\  # directory contains version and exists locally
#           ...     # do not extract content under versioned part
#       abc\asd\    # same rules as for files
#            ...
#       def\ghi\1.0.1\  # directory contains version and does not exist locally
#           ...         # extract content
function Extract-Dotnet-Package([string]$ZipPath, [string]$OutPath) {
    Say-Invocation $MyInvocation

    Load-Assembly -Assembly System.IO.Compression.FileSystem
    Set-Variable -Name Zip
    try {
        $Zip = [System.IO.Compression.ZipFile]::OpenRead($ZipPath)

        $DirectoriesToUnpack = Get-List-Of-Directories-And-Versions-To-Unpack-From-Dotnet-Package -Zip $Zip -OutPath $OutPath

        foreach ($entry in $Zip.Entries) {
            $PathWithVersion = Get-Path-Prefix-With-Version $entry.FullName
            if (($null -eq $PathWithVersion) -Or ($DirectoriesToUnpack -contains $PathWithVersion)) {
                $DestinationPath = Get-Absolute-Path $(Join-Path -Path $OutPath -ChildPath $entry.FullName)
                $DestinationDir = Split-Path -Parent $DestinationPath
                $OverrideFiles=$OverrideNonVersionedFiles -Or (-Not (Test-Path $DestinationPath))
                if ((-Not $DestinationPath.EndsWith("\")) -And $OverrideFiles) {
                    New-Item -ItemType Directory -Force -Path $DestinationDir | Out-Null
                    [System.IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $DestinationPath, $OverrideNonVersionedFiles)
                }
            }
        }
    }
    finally {
        if ($null -ne $Zip) {
            $Zip.Dispose()
        }
    }
}

function DownloadFile($Source, [string]$OutPath) {
    if ($Source -notlike "http*") {
        #  Using System.IO.Path.GetFullPath to get the current directory
        #    does not work in this context - $pwd gives the current directory
        if (![System.IO.Path]::IsPathRooted($Source)) {
            $Source = $(Join-Path -Path $pwd -ChildPath $Source)
        }
        $Source = Get-Absolute-Path $Source
        Say "Copying file from $Source to $OutPath"
        Copy-Item $Source $OutPath
        return
    }

    $Stream = $null

    try {
        $Response = GetHTTPResponse -Uri $Source
        $Stream = $Response.Content.ReadAsStreamAsync().Result
        $File = [System.IO.File]::Create($OutPath)
        $Stream.CopyTo($File)
        $File.Close()
    }
    finally {
        if ($null -ne $Stream) {
            $Stream.Dispose()
        }
    }
}

function SafeRemoveFile($Path) {
    try {
        if (Test-Path $Path) {
            Remove-Item $Path
            Say-Verbose "The temporary file `"$Path`" was removed."
        }
        else
        {
            Say-Verbose "The temporary file `"$Path`" does not exist, therefore is not removed."
        }
    }
    catch
    {
        Say-Warning "Failed to remove the temporary file: `"$Path`", remove it manually."
    }
}

function Prepend-Sdk-InstallRoot-To-Path([string]$InstallRoot) {
    $BinPath = Get-Absolute-Path $(Join-Path -Path $InstallRoot -ChildPath "")
    if (-Not $NoPath) {
        $SuffixedBinPath = "$BinPath;"
        if (-Not $env:path.Contains($SuffixedBinPath)) {
            Say "Adding to current process PATH: `"$BinPath`". Note: This change will not be visible if PowerShell was run as a child process."
            $env:path = $SuffixedBinPath + $env:path
        } else {
            Say-Verbose "Current process PATH already contains `"$BinPath`""
        }
    }
    else {
        Say "Binaries of dotnet can be found in $BinPath"
    }
}

function PrintDryRunOutput($Invocation, $DownloadLinks)
{
    Say "Payload URLs:"
    
    for ($linkIndex=0; $linkIndex -lt $DownloadLinks.count; $linkIndex++) {
        Say "URL #$linkIndex - $($DownloadLinks[$linkIndex].type): $($DownloadLinks[$linkIndex].downloadLink)"
    }
    $RepeatableCommand = ".\$ScriptName -Version `"$SpecificVersion`" -InstallDir `"$InstallRoot`" -Architecture `"$CLIArchitecture`""
    if ($Runtime -eq "dotnet") {
       $RepeatableCommand+=" -Runtime `"dotnet`""
    }
    elseif ($Runtime -eq "aspnetcore") {
       $RepeatableCommand+=" -Runtime `"aspnetcore`""
    }

    foreach ($key in $Invocation.BoundParameters.Keys) {
        if (-not (@("Architecture","Channel","DryRun","InstallDir","Runtime","SharedRuntime","Version","Quality","FeedCredential") -contains $key)) {
            $RepeatableCommand+=" -$key `"$($Invocation.BoundParameters[$key])`""
        }
    }
    if ($Invocation.BoundParameters.Keys -contains "FeedCredential") {
        $RepeatableCommand+=" -FeedCredential `"<feedCredential>`""
    }
    Say "Repeatable invocation: $RepeatableCommand"
    if ($SpecificVersion -ne $EffectiveVersion)
    {
        Say "NOTE: Due to finding a version manifest with this runtime, it would actually install with version '$EffectiveVersion'"
    }
}

function Get-AkaMSDownloadLink([string]$Channel, [string]$Quality, [bool]$Internal, [string]$Product, [string]$Architecture) {
    Say-Invocation $MyInvocation 

    #quality is not supported for LTS or current channel
    if (![string]::IsNullOrEmpty($Quality) -and (@("LTS", "current") -contains $Channel)) {
        $Quality = ""
        Say-Warning "Specifying quality for current or LTS channel is not supported, the quality will be ignored."
    }
    Say-Verbose "Retrieving primary payload URL from aka.ms link for channel: '$Channel', quality: '$Quality' product: '$Product', os: 'win', architecture: '$Architecture'." 
   
    #construct aka.ms link
    $akaMsLink = "https://aka.ms/dotnet"
    if ($Internal) {
        $akaMsLink += "/internal"
    }
    $akaMsLink += "/$Channel"
    if (-not [string]::IsNullOrEmpty($Quality)) {
        $akaMsLink +="/$Quality"
    }
    $akaMsLink +="/$Product-win-$Architecture.zip"
    Say-Verbose  "Constructed aka.ms link: '$akaMsLink'."
    $akaMsDownloadLink=$null

    for ($maxRedirections = 9; $maxRedirections -ge 0; $maxRedirections--)
    {
        #get HTTP response
        #do not pass credentials as a part of the $akaMsLink and do not apply credentials in the GetHTTPResponse function
        #otherwise the redirect link would have credentials as well
        #it would result in applying credentials twice to the resulting link and thus breaking it, and in echoing credentials to the output as a part of redirect link
        $Response= GetHTTPResponse -Uri $akaMsLink -HeaderOnly $true -DisableRedirect $true -DisableFeedCredential $true
        Say-Verbose "Received response:`n$Response"

        if ([string]::IsNullOrEmpty($Response)) {
            Say-Verbose "The link '$akaMsLink' is not valid: failed to get redirect location. The resource is not available."
            return $null
        }

        #if HTTP code is 301 (Moved Permanently), the redirect link exists
        if  ($Response.StatusCode -eq 301)
        {
            try {
                $akaMsDownloadLink = $Response.Headers.GetValues("Location")[0]

                if ([string]::IsNullOrEmpty($akaMsDownloadLink)) {
                    Say-Verbose "The link '$akaMsLink' is not valid: server returned 301 (Moved Permanently), but the headers do not contain the redirect location."
                    return $null
                }

                Say-Verbose "The redirect location retrieved: '$akaMsDownloadLink'."
                # This may yet be a link to another redirection. Attempt to retrieve the page again.
                $akaMsLink = $akaMsDownloadLink
                continue
            }
            catch {
                Say-Verbose "The link '$akaMsLink' is not valid: failed to get redirect location."
                return $null
            }
        }
        elseif ((($Response.StatusCode -lt 300) -or ($Response.StatusCode -ge 400)) -and (-not [string]::IsNullOrEmpty($akaMsDownloadLink)))
        {
            # Redirections have ended.
            return $akaMsDownloadLink
        }

        Say-Verbose "The link '$akaMsLink' is not valid: failed to retrieve the redirection location."
        return $null
    }

    Say-Verbose "Aka.ms links have redirected more than the maximum allowed redirections. This may be caused by a cyclic redirection of aka.ms links."
    return $null

}

function Get-AkaMsLink-And-Version([string] $NormalizedChannel, [string] $NormalizedQuality, [bool] $Internal, [string] $ProductName, [string] $Architecture) {
    $AkaMsDownloadLink = Get-AkaMSDownloadLink -Channel $NormalizedChannel -Quality $NormalizedQuality -Internal $Internal -Product $ProductName -Architecture $Architecture
   
    if ([string]::IsNullOrEmpty($AkaMsDownloadLink)){
        if (-not [string]::IsNullOrEmpty($NormalizedQuality)) {
            # if quality is specified - exit with error - there is no fallback approach
            Say-Error "Failed to locate the latest version in the channel '$NormalizedChannel' with '$NormalizedQuality' quality for '$ProductName', os: 'win', architecture: '$Architecture'."
            Say-Error "Refer to: https://aka.ms/dotnet-os-lifecycle for information on .NET Core support."
            throw "aka.ms link resolution failure"
        }
        Say-Verbose "Falling back to latest.version file approach."
        return ($null, $null, $null)
    }
    else {
        Say-Verbose "Retrieved primary named payload URL from aka.ms link: '$AkaMsDownloadLink'."
        Say-Verbose  "Downloading using legacy url will not be attempted."

        #get version from the path
        $pathParts = $AkaMsDownloadLink.Split('/')
        if ($pathParts.Length -ge 2) { 
            $SpecificVersion = $pathParts[$pathParts.Length - 2]
            Say-Verbose "Version: '$SpecificVersion'."
        }
        else {
            Say-Error "Failed to extract the version from download link '$AkaMsDownloadLink'."
            return ($null, $null, $null)
        }

        #retrieve effective (product) version
        $EffectiveVersion = Get-Product-Version -SpecificVersion $SpecificVersion -PackageDownloadLink $AkaMsDownloadLink
        Say-Verbose "Product version: '$EffectiveVersion'."

        return ($AkaMsDownloadLink, $SpecificVersion, $EffectiveVersion);
    }
}

function Get-Feeds-To-Use()
{
    $feeds = @(
    "https://dotnetcli.azureedge.net/dotnet",
    "https://dotnetbuilds.azureedge.net/public"
    )

    if (-not [string]::IsNullOrEmpty($AzureFeed)) {
        $feeds = @($AzureFeed)
    }

    if ($NoCdn) {
        $feeds = @(
        "https://dotnetcli.blob.core.windows.net/dotnet",
        "https://dotnetbuilds.blob.core.windows.net/public"
        )

        if (-not [string]::IsNullOrEmpty($UncachedFeed)) {
            $feeds = @($UncachedFeed)
        }
    }

    return $feeds
}

function Resolve-AssetName-And-RelativePath([string] $Runtime) {
    
    if ($Runtime -eq "dotnet") {
        $assetName = ".NET Core Runtime"
        $dotnetPackageRelativePath = "shared\Microsoft.NETCore.App"
    }
    elseif ($Runtime -eq "aspnetcore") {
        $assetName = "ASP.NET Core Runtime"
        $dotnetPackageRelativePath = "shared\Microsoft.AspNetCore.App"
    }
    elseif ($Runtime -eq "windowsdesktop") {
        $assetName = ".NET Core Windows Desktop Runtime"
        $dotnetPackageRelativePath = "shared\Microsoft.WindowsDesktop.App"
    }
    elseif (-not $Runtime) {
        $assetName = ".NET Core SDK"
        $dotnetPackageRelativePath = "sdk"
    }
    else {
        throw "Invalid value for `$Runtime"
    }

    return ($assetName, $dotnetPackageRelativePath)
}

function Prepare-Install-Directory {
    New-Item -ItemType Directory -Force -Path $InstallRoot | Out-Null

    $installDrive = $((Get-Item $InstallRoot -Force).PSDrive.Name);
    $diskInfo = $null
    try{
        $diskInfo = Get-PSDrive -Name $installDrive
    }
    catch{
        Say-Warning "Failed to check the disk space. Installation will continue, but it may fail if you do not have enough disk space."
    }
    
    if ( ($null -ne $diskInfo) -and ($diskInfo.Free / 1MB -le 100)) {
        throw "There is not enough disk space on drive ${installDrive}:"
    }
}

Say "Note that the intended use of this script is for Continuous Integration (CI) scenarios, where:"
Say "- The SDK needs to be installed without user interaction and without admin rights."
Say "- The SDK installation doesn't need to persist across multiple CI runs."
Say "To set up a development environment or to run apps, use installers rather than this script. Visit https://dotnet.microsoft.com/download to get the installer.`r`n"

if ($SharedRuntime -and (-not $Runtime)) {
    $Runtime = "dotnet"
}

$OverrideNonVersionedFiles = !$SkipNonVersionedFiles

$CLIArchitecture = Get-CLIArchitecture-From-Architecture $Architecture
$NormalizedQuality = Get-NormalizedQuality $Quality
Say-Verbose "Normalized quality: '$NormalizedQuality'"
$NormalizedChannel = Get-NormalizedChannel $Channel
Say-Verbose "Normalized channel: '$NormalizedChannel'"
$NormalizedProduct = Get-NormalizedProduct $Runtime
Say-Verbose "Normalized product: '$NormalizedProduct'"
$FeedCredential = ValidateFeedCredential $FeedCredential

$InstallRoot = Resolve-Installation-Path $InstallDir
Say-Verbose "InstallRoot: $InstallRoot"
$ScriptName = $MyInvocation.MyCommand.Name
($assetName, $dotnetPackageRelativePath) = Resolve-AssetName-And-RelativePath -Runtime $Runtime

$feeds = Get-Feeds-To-Use
$DownloadLinks = @()

# aka.ms links can only be used if the user did not request a specific version via the command line or a global.json file.
if ([string]::IsNullOrEmpty($JSonFile) -and ($Version -eq "latest")) {
    ($DownloadLink, $SpecificVersion, $EffectiveVersion) = Get-AkaMsLink-And-Version $NormalizedChannel $NormalizedQuality $Internal $NormalizedProduct $CLIArchitecture
    
    if ($null -ne $DownloadLink) {
        $DownloadLinks += New-Object PSObject -Property @{downloadLink="$DownloadLink";specificVersion="$SpecificVersion";effectiveVersion="$EffectiveVersion";type='aka.ms'}
        Say-Verbose "Generated aka.ms link $DownloadLink with version $EffectiveVersion"
        
        if (-Not $DryRun) {
            Say-Verbose "Checking if the version $EffectiveVersion is already installed"
            if (Is-Dotnet-Package-Installed -InstallRoot $InstallRoot -RelativePathToPackage $dotnetPackageRelativePath -SpecificVersion $EffectiveVersion)
            {
                Say "$assetName with version '$EffectiveVersion' is already installed."
                Prepend-Sdk-InstallRoot-To-Path -InstallRoot $InstallRoot
                return
            }
        }
    }
}

# Primary and legacy links cannot be used if a quality was specified.
# If we already have an aka.ms link, no need to search the blob feeds.
if ([string]::IsNullOrEmpty($NormalizedQuality) -and 0 -eq $DownloadLinks.count)
{
    foreach ($feed in $feeds) {
        try {
            $SpecificVersion = Get-Specific-Version-From-Version -AzureFeed $feed -Channel $Channel -Version $Version -JSonFile $JSonFile
            $DownloadLink, $EffectiveVersion = Get-Download-Link -AzureFeed $feed -SpecificVersion $SpecificVersion -CLIArchitecture $CLIArchitecture
            $LegacyDownloadLink = Get-LegacyDownload-Link -AzureFeed $feed -SpecificVersion $SpecificVersion -CLIArchitecture $CLIArchitecture
            
            $DownloadLinks += New-Object PSObject -Property @{downloadLink="$DownloadLink";specificVersion="$SpecificVersion";effectiveVersion="$EffectiveVersion";type='primary'}
            Say-Verbose "Generated primary link $DownloadLink with version $EffectiveVersion"
    
            if (-not [string]::IsNullOrEmpty($LegacyDownloadLink)) {
                $DownloadLinks += New-Object PSObject -Property @{downloadLink="$LegacyDownloadLink";specificVersion="$SpecificVersion";effectiveVersion="$EffectiveVersion";type='legacy'}
                Say-Verbose "Generated legacy link $LegacyDownloadLink with version $EffectiveVersion"
            }
    
            if (-Not $DryRun) {
                Say-Verbose "Checking if the version $EffectiveVersion is already installed"
                if (Is-Dotnet-Package-Installed -InstallRoot $InstallRoot -RelativePathToPackage $dotnetPackageRelativePath -SpecificVersion $EffectiveVersion)
                {
                    Say "$assetName with version '$EffectiveVersion' is already installed."
                    Prepend-Sdk-InstallRoot-To-Path -InstallRoot $InstallRoot
                    return
                }
            }
        }
        catch
        {
            Say-Verbose "Failed to acquire download links from feed $feed. Exception: $_"
        }
    }
}

if ($DownloadLinks.count -eq 0) {
    throw "Failed to resolve the exact version number."
}

if ($DryRun) {
    PrintDryRunOutput $MyInvocation $DownloadLinks
    return
}

Prepare-Install-Directory

$ZipPath = [System.IO.Path]::combine([System.IO.Path]::GetTempPath(), [System.IO.Path]::GetRandomFileName())
Say-Verbose "Zip path: $ZipPath"

$DownloadSucceeded = $false
$DownloadedLink = $null
$ErrorMessages = @()

foreach ($link in $DownloadLinks)
{
    Say-Verbose "Downloading `"$($link.type)`" link $($link.downloadLink)"

    try {
        DownloadFile -Source $link.downloadLink -OutPath $ZipPath
        Say-Verbose "Download succeeded."
        $DownloadSucceeded = $true
        $DownloadedLink = $link
        break
    }
    catch {
        $StatusCode = $null
        $ErrorMessage = $null

        if ($PSItem.Exception.Data.Contains("StatusCode")) {
            $StatusCode = $PSItem.Exception.Data["StatusCode"]
        }
    
        if ($PSItem.Exception.Data.Contains("ErrorMessage")) {
            $ErrorMessage = $PSItem.Exception.Data["ErrorMessage"]
        } else {
            $ErrorMessage = $PSItem.Exception.Message
        }

        Say-Verbose "Download failed with status code $StatusCode. Error message: $ErrorMessage"
        $ErrorMessages += "Downloading from `"$($link.type)`" link has failed with error:`nUri: $($link.downloadLink)`nStatusCode: $StatusCode`nError: $ErrorMessage"
    }

    # This link failed. Clean up before trying the next one.
    SafeRemoveFile -Path $ZipPath
}

if (-not $DownloadSucceeded) {
    foreach ($ErrorMessage in $ErrorMessages) {
        Say-Error $ErrorMessages
    }

    throw "Could not find `"$assetName`" with version = $($DownloadLinks[0].effectiveVersion)`nRefer to: https://aka.ms/dotnet-os-lifecycle for information on .NET support"
}

Say "Extracting the archive."
Extract-Dotnet-Package -ZipPath $ZipPath -OutPath $InstallRoot

#  Check if the SDK version is installed; if not, fail the installation.
$isAssetInstalled = $false

# if the version contains "RTM" or "servicing"; check if a 'release-type' SDK version is installed.
if ($DownloadedLink.effectiveVersion -Match "rtm" -or $DownloadedLink.effectiveVersion -Match "servicing") {
    $ReleaseVersion = $DownloadedLink.effectiveVersion.Split("-")[0]
    Say-Verbose "Checking installation: version = $ReleaseVersion"
    $isAssetInstalled = Is-Dotnet-Package-Installed -InstallRoot $InstallRoot -RelativePathToPackage $dotnetPackageRelativePath -SpecificVersion $ReleaseVersion
}

#  Check if the SDK version is installed.
if (!$isAssetInstalled) {
    Say-Verbose "Checking installation: version = $($DownloadedLink.effectiveVersion)"
    $isAssetInstalled = Is-Dotnet-Package-Installed -InstallRoot $InstallRoot -RelativePathToPackage $dotnetPackageRelativePath -SpecificVersion $DownloadedLink.effectiveVersion
}

# Version verification failed. More likely something is wrong either with the downloaded content or with the verification algorithm.
if (!$isAssetInstalled) {
    Say-Error "Failed to verify the version of installed `"$assetName`".`nInstallation source: $($DownloadedLink.downloadLink).`nInstallation location: $InstallRoot.`nReport the bug at https://github.com/dotnet/install-scripts/issues."
    throw "`"$assetName`" with version = $($DownloadedLink.effectiveVersion) failed to install with an unknown error."
}

SafeRemoveFile -Path $ZipPath

Prepend-Sdk-InstallRoot-To-Path -InstallRoot $InstallRoot

Say "Note that the script does not resolve dependencies during installation."
Say "To check the list of dependencies, go to https://docs.microsoft.com/dotnet/core/install/windows#dependencies"
Say "Installation finished"

# SIG # Begin signature block
# MIInoQYJKoZIhvcNAQcCoIInkjCCJ44CAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCAuo3sRWvfrJ+Bd
# sIQ2zLeO20Ij33Vb5ljtEhxAYYSEc6CCDYEwggX/MIID56ADAgECAhMzAAACUosz
# qviV8znbAAAAAAJSMA0GCSqGSIb3DQEBCwUAMH4xCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25p
# bmcgUENBIDIwMTEwHhcNMjEwOTAyMTgzMjU5WhcNMjIwOTAxMTgzMjU5WjB0MQsw
# CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9u
# ZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMR4wHAYDVQQDExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB
# AQDQ5M+Ps/X7BNuv5B/0I6uoDwj0NJOo1KrVQqO7ggRXccklyTrWL4xMShjIou2I
# sbYnF67wXzVAq5Om4oe+LfzSDOzjcb6ms00gBo0OQaqwQ1BijyJ7NvDf80I1fW9O
# L76Kt0Wpc2zrGhzcHdb7upPrvxvSNNUvxK3sgw7YTt31410vpEp8yfBEl/hd8ZzA
# v47DCgJ5j1zm295s1RVZHNp6MoiQFVOECm4AwK2l28i+YER1JO4IplTH44uvzX9o
# RnJHaMvWzZEpozPy4jNO2DDqbcNs4zh7AWMhE1PWFVA+CHI/En5nASvCvLmuR/t8
# q4bc8XR8QIZJQSp+2U6m2ldNAgMBAAGjggF+MIIBejAfBgNVHSUEGDAWBgorBgEE
# AYI3TAgBBggrBgEFBQcDAzAdBgNVHQ4EFgQUNZJaEUGL2Guwt7ZOAu4efEYXedEw
# UAYDVR0RBEkwR6RFMEMxKTAnBgNVBAsTIE1pY3Jvc29mdCBPcGVyYXRpb25zIFB1
# ZXJ0byBSaWNvMRYwFAYDVQQFEw0yMzAwMTIrNDY3NTk3MB8GA1UdIwQYMBaAFEhu
# ZOVQBdOCqhc3NyK1bajKdQKVMFQGA1UdHwRNMEswSaBHoEWGQ2h0dHA6Ly93d3cu
# bWljcm9zb2Z0LmNvbS9wa2lvcHMvY3JsL01pY0NvZFNpZ1BDQTIwMTFfMjAxMS0w
# Ny0wOC5jcmwwYQYIKwYBBQUHAQEEVTBTMFEGCCsGAQUFBzAChkVodHRwOi8vd3d3
# Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2NlcnRzL01pY0NvZFNpZ1BDQTIwMTFfMjAx
# MS0wNy0wOC5jcnQwDAYDVR0TAQH/BAIwADANBgkqhkiG9w0BAQsFAAOCAgEAFkk3
# uSxkTEBh1NtAl7BivIEsAWdgX1qZ+EdZMYbQKasY6IhSLXRMxF1B3OKdR9K/kccp
# kvNcGl8D7YyYS4mhCUMBR+VLrg3f8PUj38A9V5aiY2/Jok7WZFOAmjPRNNGnyeg7
# l0lTiThFqE+2aOs6+heegqAdelGgNJKRHLWRuhGKuLIw5lkgx9Ky+QvZrn/Ddi8u
# TIgWKp+MGG8xY6PBvvjgt9jQShlnPrZ3UY8Bvwy6rynhXBaV0V0TTL0gEx7eh/K1
# o8Miaru6s/7FyqOLeUS4vTHh9TgBL5DtxCYurXbSBVtL1Fj44+Od/6cmC9mmvrti
# yG709Y3Rd3YdJj2f3GJq7Y7KdWq0QYhatKhBeg4fxjhg0yut2g6aM1mxjNPrE48z
# 6HWCNGu9gMK5ZudldRw4a45Z06Aoktof0CqOyTErvq0YjoE4Xpa0+87T/PVUXNqf
# 7Y+qSU7+9LtLQuMYR4w3cSPjuNusvLf9gBnch5RqM7kaDtYWDgLyB42EfsxeMqwK
# WwA+TVi0HrWRqfSx2olbE56hJcEkMjOSKz3sRuupFCX3UroyYf52L+2iVTrda8XW
# esPG62Mnn3T8AuLfzeJFuAbfOSERx7IFZO92UPoXE1uEjL5skl1yTZB3MubgOA4F
# 8KoRNhviFAEST+nG8c8uIsbZeb08SeYQMqjVEmkwggd6MIIFYqADAgECAgphDpDS
# AAAAAAADMA0GCSqGSIb3DQEBCwUAMIGIMQswCQYDVQQGEwJVUzETMBEGA1UECBMK
# V2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0
# IENvcnBvcmF0aW9uMTIwMAYDVQQDEylNaWNyb3NvZnQgUm9vdCBDZXJ0aWZpY2F0
# ZSBBdXRob3JpdHkgMjAxMTAeFw0xMTA3MDgyMDU5MDlaFw0yNjA3MDgyMTA5MDla
# MH4xCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdS
# ZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMT
# H01pY3Jvc29mdCBDb2RlIFNpZ25pbmcgUENBIDIwMTEwggIiMA0GCSqGSIb3DQEB
# AQUAA4ICDwAwggIKAoICAQCr8PpyEBwurdhuqoIQTTS68rZYIZ9CGypr6VpQqrgG
# OBoESbp/wwwe3TdrxhLYC/A4wpkGsMg51QEUMULTiQ15ZId+lGAkbK+eSZzpaF7S
# 35tTsgosw6/ZqSuuegmv15ZZymAaBelmdugyUiYSL+erCFDPs0S3XdjELgN1q2jz
# y23zOlyhFvRGuuA4ZKxuZDV4pqBjDy3TQJP4494HDdVceaVJKecNvqATd76UPe/7
# 4ytaEB9NViiienLgEjq3SV7Y7e1DkYPZe7J7hhvZPrGMXeiJT4Qa8qEvWeSQOy2u
# M1jFtz7+MtOzAz2xsq+SOH7SnYAs9U5WkSE1JcM5bmR/U7qcD60ZI4TL9LoDho33
# X/DQUr+MlIe8wCF0JV8YKLbMJyg4JZg5SjbPfLGSrhwjp6lm7GEfauEoSZ1fiOIl
# XdMhSz5SxLVXPyQD8NF6Wy/VI+NwXQ9RRnez+ADhvKwCgl/bwBWzvRvUVUvnOaEP
# 6SNJvBi4RHxF5MHDcnrgcuck379GmcXvwhxX24ON7E1JMKerjt/sW5+v/N2wZuLB
# l4F77dbtS+dJKacTKKanfWeA5opieF+yL4TXV5xcv3coKPHtbcMojyyPQDdPweGF
# RInECUzF1KVDL3SV9274eCBYLBNdYJWaPk8zhNqwiBfenk70lrC8RqBsmNLg1oiM
# CwIDAQABo4IB7TCCAekwEAYJKwYBBAGCNxUBBAMCAQAwHQYDVR0OBBYEFEhuZOVQ
# BdOCqhc3NyK1bajKdQKVMBkGCSsGAQQBgjcUAgQMHgoAUwB1AGIAQwBBMAsGA1Ud
# DwQEAwIBhjAPBgNVHRMBAf8EBTADAQH/MB8GA1UdIwQYMBaAFHItOgIxkEO5FAVO
# 4eqnxzHRI4k0MFoGA1UdHwRTMFEwT6BNoEuGSWh0dHA6Ly9jcmwubWljcm9zb2Z0
# LmNvbS9wa2kvY3JsL3Byb2R1Y3RzL01pY1Jvb0NlckF1dDIwMTFfMjAxMV8wM18y
# Mi5jcmwwXgYIKwYBBQUHAQEEUjBQME4GCCsGAQUFBzAChkJodHRwOi8vd3d3Lm1p
# Y3Jvc29mdC5jb20vcGtpL2NlcnRzL01pY1Jvb0NlckF1dDIwMTFfMjAxMV8wM18y
# Mi5jcnQwgZ8GA1UdIASBlzCBlDCBkQYJKwYBBAGCNy4DMIGDMD8GCCsGAQUFBwIB
# FjNodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2RvY3MvcHJpbWFyeWNw
# cy5odG0wQAYIKwYBBQUHAgIwNB4yIB0ATABlAGcAYQBsAF8AcABvAGwAaQBjAHkA
# XwBzAHQAYQB0AGUAbQBlAG4AdAAuIB0wDQYJKoZIhvcNAQELBQADggIBAGfyhqWY
# 4FR5Gi7T2HRnIpsLlhHhY5KZQpZ90nkMkMFlXy4sPvjDctFtg/6+P+gKyju/R6mj
# 82nbY78iNaWXXWWEkH2LRlBV2AySfNIaSxzzPEKLUtCw/WvjPgcuKZvmPRul1LUd
# d5Q54ulkyUQ9eHoj8xN9ppB0g430yyYCRirCihC7pKkFDJvtaPpoLpWgKj8qa1hJ
# Yx8JaW5amJbkg/TAj/NGK978O9C9Ne9uJa7lryft0N3zDq+ZKJeYTQ49C/IIidYf
# wzIY4vDFLc5bnrRJOQrGCsLGra7lstnbFYhRRVg4MnEnGn+x9Cf43iw6IGmYslmJ
# aG5vp7d0w0AFBqYBKig+gj8TTWYLwLNN9eGPfxxvFX1Fp3blQCplo8NdUmKGwx1j
# NpeG39rz+PIWoZon4c2ll9DuXWNB41sHnIc+BncG0QaxdR8UvmFhtfDcxhsEvt9B
# xw4o7t5lL+yX9qFcltgA1qFGvVnzl6UJS0gQmYAf0AApxbGbpT9Fdx41xtKiop96
# eiL6SJUfq/tHI4D1nvi/a7dLl+LrdXga7Oo3mXkYS//WsyNodeav+vyL6wuA6mk7
# r/ww7QRMjt/fdW1jkT3RnVZOT7+AVyKheBEyIXrvQQqxP/uozKRdwaGIm1dxVk5I
# RcBCyZt2WwqASGv9eZ/BvW1taslScxMNelDNMYIZdjCCGXICAQEwgZUwfjELMAkG
# A1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQx
# HjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEoMCYGA1UEAxMfTWljcm9z
# b2Z0IENvZGUgU2lnbmluZyBQQ0EgMjAxMQITMwAAAlKLM6r4lfM52wAAAAACUjAN
# BglghkgBZQMEAgEFAKCBrjAZBgkqhkiG9w0BCQMxDAYKKwYBBAGCNwIBBDAcBgor
# BgEEAYI3AgELMQ4wDAYKKwYBBAGCNwIBFTAvBgkqhkiG9w0BCQQxIgQgBzhh8wf+
# /kA/CamFNwS1K9Nt0wsjATKS8Y66iHQTKrIwQgYKKwYBBAGCNwIBDDE0MDKgFIAS
# AE0AaQBjAHIAbwBzAG8AZgB0oRqAGGh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbTAN
# BgkqhkiG9w0BAQEFAASCAQAruzFwahFXWsJ9eny7iY/KyWolnqJ9+cvDD1QaJ0qJ
# tHlTLP8pr0GPPtitwuk+3JME7wkx80+AZXl/ocO39tdhy1Ucd6yiyV3M1Ub/Eo4D
# 37L1saazNomWeeMD6oX3FboVe1Ql17MX1SscTW5QRx8ytYgUOUIbrxFybODcdVn9
# YG7mlkgsJr2lhUhTnHtq7jV/jEu6Sk9o+g15Pbu90VncsCoNerg9aEziui6/W3/g
# Wvt4WhfCPanppzWVmsvTUp5MAt5cpdNBr9b8CV/alI3TGnNwC8RuWMCK6CS4963l
# f4AiLZW/fgD8UQYtl+tAyYfSerpX0sDB039jymj1FkqHoYIXADCCFvwGCisGAQQB
# gjcDAwExghbsMIIW6AYJKoZIhvcNAQcCoIIW2TCCFtUCAQMxDzANBglghkgBZQME
# AgEFADCCAVEGCyqGSIb3DQEJEAEEoIIBQASCATwwggE4AgEBBgorBgEEAYRZCgMB
# MDEwDQYJYIZIAWUDBAIBBQAEIEcy5EvHdubbPngOS1BSZ6xt9xSMrt3OLtUyfQaT
# IyRBAgZh+unPYkgYEzIwMjIwMjA4MTAyMjE5LjM1OVowBIACAfSggdCkgc0wgcox
# CzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRt
# b25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJTAjBgNVBAsTHE1p
# Y3Jvc29mdCBBbWVyaWNhIE9wZXJhdGlvbnMxJjAkBgNVBAsTHVRoYWxlcyBUU1Mg
# RVNOOkREOEMtRTMzNy0yRkFFMSUwIwYDVQQDExxNaWNyb3NvZnQgVGltZS1TdGFt
# cCBTZXJ2aWNloIIRVzCCBwwwggT0oAMCAQICEzMAAAGcD6ZNYdKeSygAAQAAAZww
# DQYJKoZIhvcNAQELBQAwfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0
# b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3Jh
# dGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIwMTAwHhcN
# MjExMjAyMTkwNTE5WhcNMjMwMjI4MTkwNTE5WjCByjELMAkGA1UEBhMCVVMxEzAR
# BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1p
# Y3Jvc29mdCBDb3Jwb3JhdGlvbjElMCMGA1UECxMcTWljcm9zb2Z0IEFtZXJpY2Eg
# T3BlcmF0aW9uczEmMCQGA1UECxMdVGhhbGVzIFRTUyBFU046REQ4Qy1FMzM3LTJG
# QUUxJTAjBgNVBAMTHE1pY3Jvc29mdCBUaW1lLVN0YW1wIFNlcnZpY2UwggIiMA0G
# CSqGSIb3DQEBAQUAA4ICDwAwggIKAoICAQDbUioMGV1JFj+s612s02mKu23KPUNs
# 71OjDeJGtxkTF9rSWTiuA8XgYkAAi/5+2Ff7Ck7JcKQ9H/XD1OKwg1/bH3E1qO1z
# 8XRy0PlpGhmyilgE7KsOvW8PIZCf243KdldgOrxrL8HKiQodOwStyT5lLWYpMsuT
# 2fH8k8oihje4TlpWiFPaCKLnFDaAB0Ccy6vIdtHjYB1Ie3iOZPisquL+vNdCx7gO
# hB8iiTmTdsU8OSUpC8tBTeTIYPzmhaxQZd4moNk6qeCJyi7fiW4fyXdHrZ3otmgx
# xa5pXz5pUUr+cEjV+cwIYBMkaY5kHM9c6dEGkgHn0ZDJvdt/54FOdSG61WwHh4+e
# vUhwvXaB4LCMZIdCt5acOfNvtDjV3CHyFOp5AU/qgAwGftHU9brv4EUwcuteEAKH
# 46NufE20l/WjlNUh7gAvt2zKMjO4zXRxCUTh/prBQwXJiUZeFSrEXiOfkuvSlBni
# yAYYZp5kOnaxfCKdGYjvr4QLA93vQJ6p2Ox3IHvOdCPaCr8LsKVcFpyp8MEhhJTM
# +1LwqHJqFDF5O1Z9mjbYvm3R9vPhkG+RDLKoTpr7mTgkaTljd9xvm94Obp8BD9Hk
# 4mPi51mtgLiuN8/6aZVESVZXtvSuNkD5DnIJQerIy5jaRKW/W2rCe9ngNDJadS7R
# 96GGRl7IIE37lwIDAQABo4IBNjCCATIwHQYDVR0OBBYEFLtpCWdTXY5dtddkspy+
# oxjCA/qyMB8GA1UdIwQYMBaAFJ+nFV0AXmJdg/Tl0mWnG1M1GelyMF8GA1UdHwRY
# MFYwVKBSoFCGTmh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9wa2lvcHMvY3JsL01p
# Y3Jvc29mdCUyMFRpbWUtU3RhbXAlMjBQQ0ElMjAyMDEwKDEpLmNybDBsBggrBgEF
# BQcBAQRgMF4wXAYIKwYBBQUHMAKGUGh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9w
# a2lvcHMvY2VydHMvTWljcm9zb2Z0JTIwVGltZS1TdGFtcCUyMFBDQSUyMDIwMTAo
# MSkuY3J0MAwGA1UdEwEB/wQCMAAwEwYDVR0lBAwwCgYIKwYBBQUHAwgwDQYJKoZI
# hvcNAQELBQADggIBAKcAKqYjGEczTWMs9z0m7Yo23sgqVF3LyK6gOMz7TCHAJN+F
# vbvZkQ53VkvrZUd1sE6a9ToGldcJnOmBc6iuhBlpvdN1BLBRO8QSTD1433VTj4XC
# Qd737wND1+eqKG3BdjrzbDksEwfG4v57PgrN/T7s7PkEjUGXfIgFQQkr8TQi+/HZ
# Z9kRlNccgeACqlfb4uGPxn5sdhQPoxdMvmC3qG9DONJ5UsS9KtO+bey+ohUTDa9L
# vEToc4Qzy5fuHj2H1JsmCaKG78nXpfWpwBLBxZYSpfml29onN8jcG7KD8nGSS/76
# PDlb2GMQsvv+Ra0JgL6FtGRGgYmHCpM6zVrf4V/a+SoHcC+tcdGYk2aKU5KOlv+f
# FE3n024V+z54tDAKR9z78rejdCBWqfvy5cBUQ9c5+3unHD08BEp7qP2rgpoD856v
# NDgEwO77n7EWT76nl/IyrbK2kjbHLzUMphFpXKnV1fYWJI2+E/0LHvXFGGqF4OvM
# BRxbrJVn03T2Dy5db6s5TzJzSaQvCrXYqA4HKvstQWkqkpvBHTX8M09+/vyRbVXN
# xrPdeXw6oD2Q4DksykCFfn8N2j2LdixE9wG5iilv69dzsvHIN/g9A9+thkAQCVb9
# DUSOTaMIGgsOqDYFjhT6ze9lkhHHGv/EEIkxj9l6S4hqUQyWerFkaUWDXcnZMIIH
# cTCCBVmgAwIBAgITMwAAABXF52ueAptJmQAAAAAAFTANBgkqhkiG9w0BAQsFADCB
# iDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1Jl
# ZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMp
# TWljcm9zb2Z0IFJvb3QgQ2VydGlmaWNhdGUgQXV0aG9yaXR5IDIwMTAwHhcNMjEw
# OTMwMTgyMjI1WhcNMzAwOTMwMTgzMjI1WjB8MQswCQYDVQQGEwJVUzETMBEGA1UE
# CBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9z
# b2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQ
# Q0EgMjAxMDCCAiIwDQYJKoZIhvcNAQEBBQADggIPADCCAgoCggIBAOThpkzntHIh
# C3miy9ckeb0O1YLT/e6cBwfSqWxOdcjKNVf2AX9sSuDivbk+F2Az/1xPx2b3lVNx
# WuJ+Slr+uDZnhUYjDLWNE893MsAQGOhgfWpSg0S3po5GawcU88V29YZQ3MFEyHFc
# UTE3oAo4bo3t1w/YJlN8OWECesSq/XJprx2rrPY2vjUmZNqYO7oaezOtgFt+jBAc
# nVL+tuhiJdxqD89d9P6OU8/W7IVWTe/dvI2k45GPsjksUZzpcGkNyjYtcI4xyDUo
# veO0hyTD4MmPfrVUj9z6BVWYbWg7mka97aSueik3rMvrg0XnRm7KMtXAhjBcTyzi
# YrLNueKNiOSWrAFKu75xqRdbZ2De+JKRHh09/SDPc31BmkZ1zcRfNN0Sidb9pSB9
# fvzZnkXftnIv231fgLrbqn427DZM9ituqBJR6L8FA6PRc6ZNN3SUHDSCD/AQ8rdH
# GO2n6Jl8P0zbr17C89XYcz1DTsEzOUyOArxCaC4Q6oRRRuLRvWoYWmEBc8pnol7X
# KHYC4jMYctenIPDC+hIK12NvDMk2ZItboKaDIV1fMHSRlJTYuVD5C4lh8zYGNRiE
# R9vcG9H9stQcxWv2XFJRXRLbJbqvUAV6bMURHXLvjflSxIUXk8A8FdsaN8cIFRg/
# eKtFtvUeh17aj54WcmnGrnu3tz5q4i6tAgMBAAGjggHdMIIB2TASBgkrBgEEAYI3
# FQEEBQIDAQABMCMGCSsGAQQBgjcVAgQWBBQqp1L+ZMSavoKRPEY1Kc8Q/y8E7jAd
# BgNVHQ4EFgQUn6cVXQBeYl2D9OXSZacbUzUZ6XIwXAYDVR0gBFUwUzBRBgwrBgEE
# AYI3TIN9AQEwQTA/BggrBgEFBQcCARYzaHR0cDovL3d3dy5taWNyb3NvZnQuY29t
# L3BraW9wcy9Eb2NzL1JlcG9zaXRvcnkuaHRtMBMGA1UdJQQMMAoGCCsGAQUFBwMI
# MBkGCSsGAQQBgjcUAgQMHgoAUwB1AGIAQwBBMAsGA1UdDwQEAwIBhjAPBgNVHRMB
# Af8EBTADAQH/MB8GA1UdIwQYMBaAFNX2VsuP6KJcYmjRPZSQW9fOmhjEMFYGA1Ud
# HwRPME0wS6BJoEeGRWh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2kvY3JsL3By
# b2R1Y3RzL01pY1Jvb0NlckF1dF8yMDEwLTA2LTIzLmNybDBaBggrBgEFBQcBAQRO
# MEwwSgYIKwYBBQUHMAKGPmh0dHA6Ly93d3cubWljcm9zb2Z0LmNvbS9wa2kvY2Vy
# dHMvTWljUm9vQ2VyQXV0XzIwMTAtMDYtMjMuY3J0MA0GCSqGSIb3DQEBCwUAA4IC
# AQCdVX38Kq3hLB9nATEkW+Geckv8qW/qXBS2Pk5HZHixBpOXPTEztTnXwnE2P9pk
# bHzQdTltuw8x5MKP+2zRoZQYIu7pZmc6U03dmLq2HnjYNi6cqYJWAAOwBb6J6Gng
# ugnue99qb74py27YP0h1AdkY3m2CDPVtI1TkeFN1JFe53Z/zjj3G82jfZfakVqr3
# lbYoVSfQJL1AoL8ZthISEV09J+BAljis9/kpicO8F7BUhUKz/AyeixmJ5/ALaoHC
# gRlCGVJ1ijbCHcNhcy4sa3tuPywJeBTpkbKpW99Jo3QMvOyRgNI95ko+ZjtPu4b6
# MhrZlvSP9pEB9s7GdP32THJvEKt1MMU0sHrYUP4KWN1APMdUbZ1jdEgssU5HLcEU
# BHG/ZPkkvnNtyo4JvbMBV0lUZNlz138eW0QBjloZkWsNn6Qo3GcZKCS6OEuabvsh
# VGtqRRFHqfG3rsjoiV5PndLQTHa1V1QJsWkBRH58oWFsc/4Ku+xBZj1p/cvBQUl+
# fpO+y/g75LcVv7TOPqUxUYS8vwLBgqJ7Fx0ViY1w/ue10CgaiQuPNtq6TPmb/wrp
# NPgkNWcr4A245oyZ1uEi6vAnQj0llOZ0dFtq0Z4+7X6gMTN9vMvpe784cETRkPHI
# qzqKOghif9lwY1NNje6CbaUFEMFxBmoQtB1VM1izoXBm8qGCAs4wggI3AgEBMIH4
# oYHQpIHNMIHKMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4G
# A1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSUw
# IwYDVQQLExxNaWNyb3NvZnQgQW1lcmljYSBPcGVyYXRpb25zMSYwJAYDVQQLEx1U
# aGFsZXMgVFNTIEVTTjpERDhDLUUzMzctMkZBRTElMCMGA1UEAxMcTWljcm9zb2Z0
# IFRpbWUtU3RhbXAgU2VydmljZaIjCgEBMAcGBSsOAwIaAxUAzdlp6t3ws/bnErbm
# 9c0M+9dvU0CggYMwgYCkfjB8MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGlu
# Z3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBv
# cmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1TdGFtcCBQQ0EgMjAxMDAN
# BgkqhkiG9w0BAQUFAAIFAOWsqGYwIhgPMjAyMjAyMDgxNjI5MjZaGA8yMDIyMDIw
# OTE2MjkyNlowdzA9BgorBgEEAYRZCgQBMS8wLTAKAgUA5ayoZgIBADAKAgEAAgIi
# 7QIB/zAHAgEAAgITmjAKAgUA5a355gIBADA2BgorBgEEAYRZCgQCMSgwJjAMBgor
# BgEEAYRZCgMCoAowCAIBAAIDB6EgoQowCAIBAAIDAYagMA0GCSqGSIb3DQEBBQUA
# A4GBAKFiWA9Ov0L2rGOdPitNu60BAeztCagpqKBsGnKaaSmjA/XcSXrtIvcUBBsw
# WlflwqnKvPDv3ihqUXsxY84IZZaMecGWzN1mXLh00VftiZuzoLMDlSEtvzmxeWYg
# 4nFHTR5oMqxy06auAM08mVl3P4MywHz4Yp21OIs2KSWQg3DSMYIEDTCCBAkCAQEw
# gZMwfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcT
# B1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEmMCQGA1UE
# AxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIwMTACEzMAAAGcD6ZNYdKeSygA
# AQAAAZwwDQYJYIZIAWUDBAIBBQCgggFKMBoGCSqGSIb3DQEJAzENBgsqhkiG9w0B
# CRABBDAvBgkqhkiG9w0BCQQxIgQg8oNFH9aqIpzxu29p4VzI2Hh6Hzv2T5HQOlhP
# W5ksqHkwgfoGCyqGSIb3DQEJEAIvMYHqMIHnMIHkMIG9BCA3D0WFII0syjoRd/Xe
# EIG0WUIKzzuy6P6hORrb0nqmvDCBmDCBgKR+MHwxCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1w
# IFBDQSAyMDEwAhMzAAABnA+mTWHSnksoAAEAAAGcMCIEIOSl7GUcmDL8AU+F0I3A
# iZQESHSkml1QPlOc3M8uqdF0MA0GCSqGSIb3DQEBCwUABIICAHV226l2xxnkjTdh
# Q6wi7u8betBvFNfYYn5cXwy+NoU30JjQ3ynnmNSB+TQVlmcthi8duP8dud+NU5xc
# +1jBTcJTHjWk/TDZUYpB/xNqsgUTJ8fnebVZhioPJhmFJMeQztT4NTD1ZbeJWO4O
# EABDpRqyns2eQptCrKcPIypjvdDeYfkwjgnUEyyaTZRHPZ2kNOLY6kO9feKfntFW
# 8YUIaas2IzI5GcGm+kw1efHveG0WvUpeGuFKsTUA6Jb18mOyamafu32ftD5t5LXV
# +GMgnaJLgdogAAGwK0GiB47YNIvPlKD6bGnyTR3KNYheI1GI38w0knOecCC4NW0k
# 21qROWJdnkKLKTtBX344yTnJbbKoIPzGgyWCzSINS2SD4JHzgTu8dsmMMJWwcEZh
# BLyrzmd/vWrjUFQFXVf6RZmJJVvF28s7LWrpXPVTR8Al75j8KqyZKou4fZvyaHtx
# S6K0jIqavLWqWd4wvx9seCtA0Pz1GWrBZADIRHG8d8cFoY95Z99z84NM81qXB246
# IsxU2iW+96zMJriMNEKaLHzlgFoWsoXLDQhuQItjwIvzwmwygwx0MxqoFYm+lDWG
# 8/KtSh47aZicAlwqNVGy3MZHDnX69kysQsrWoe2wDhJ3eHJ4zRL2WHVagz26L7xF
# oSkLphLv0GiFseJhPvXGQPypnyN8
# SIG # End signature block
