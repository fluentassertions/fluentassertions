properties {
    $BaseDirectory = Resolve-Path ..     
    $SrcDirectory ="$BaseDirectory\Src"
    $TestsDirectory ="$BaseDirectory\Tests"
    $Nuget = "$BaseDirectory\.nuget\NuGet.exe"
    $SlnFile = "$BaseDirectory\FluentAssertions.sln"
    $7zip = "$BaseDirectory\Lib\7z.exe"
    $GitVersionExe = "$BaseDirectory\Lib\GitVersion.exe"
    $ArtifactsDirectory = "$BaseDirectory\Artifacts"

    $NuGetPushSource = ""
    
    $MsBuildLoggerPath = ""
    $Branch = ""
    $MsTestPath = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe"
    $RunTests = $false
}

task default -depends Clean, ApplyAssemblyVersioning, ApplyPackageVersioning, RestoreNugetPackages, Compile, RunTests, RunFrameworkTests, RunSilverLightTests, BuildZip, BuildPackage, BuildJsonPackage, PublishToMyget

task Clean {    
    TeamCity-Block "Clean" {
        Get-ChildItem $PackageDirectory *.nupkg | foreach { Remove-Item $_.FullName }
        Get-ChildItem $PackageDirectory *.zip | foreach { Remove-Item $_.FullName }
    }
}

task ExtractVersionsFromGit {

        $json = . "$GitVersionExe" "$BaseDirectory" 

        if ($LASTEXITCODE -eq 0) {
            $version = (ConvertFrom-Json ($json -join "`n"));

            TeamCity-SetBuildNumber $version.FullSemVer;

            $script:AssemblyVersion = $version.AssemblySemVer;
            $script:InfoVersion = $version.InformationalVersion;
            $script:NuGetVersion = $version.NuGetVersion;
        }
        else {
            Write-Output $json -join "`n";
        }
}

task ApplyAssemblyVersioning -depends ExtractVersionsFromGit {
    TeamCity-Block "Updating solution info versions with build number $script:AssemblyVersion" {   
    
        $infos = Get-ChildItem -Path $SrcDirectory -Filter SolutionInfo.cs -Recurse
        
        foreach ($info in $infos) {
            Write-Host "Updating " + $info.FullName
            Set-ItemProperty -Path $info.FullName -Name IsReadOnly -Value $false
            
            $content = Get-Content $info.FullName
            $content = $content -replace 'AssemblyVersion\("(.+)"\)', ('AssemblyVersion("' + $script:AssemblyVersion + '")')
            $content = $content -replace 'AssemblyFileVersion\("(.+)"\)', ('AssemblyFileVersion("' + $script:AssemblyVersion + '")')
            $content = $content -replace 'AssemblyInformationalVersion\("(.+)"\)', ('AssemblyInformationalVersion("' + $script:InfoVersion + '")')
            Set-Content -Path $info.FullName $content
        }   
    }
}

task ApplyPackageVersioning -depends ExtractVersionsFromGit {
    TeamCity-Block "Updating package version to $script:NuGetVersion" {   
    
        $fullName = "$SrcDirectory\FluentAssertions.nuspec"

        Set-ItemProperty -Path $fullName -Name IsReadOnly -Value $false
        
        $content = Get-Content $fullName
        $content = $content -replace '<version>.+</version>', ('<version>' + "$script:NuGetVersion" + '</version>')
        Set-Content -Path $fullName $content
    }
}

task RestoreNugetPackages {
    TeamCity-Block "Restoring NuGet packages" {
        
        & $Nuget restore "$BaseDirectory\FluentAssertions.sln"  
        & $Nuget install "$BaseDirectory\Build\packages.config" -OutputDirectory "$BaseDirectory\Packages" -ConfigFile "$BaseDirectory\NuGet.Config"
    }
}

task Compile {
    TeamCity-Block "Compiling" {  
       
        if ($MsBuildLoggerPath -ne "")
        {
            Write-Host "Using TeamCity MSBuild logger"
            $logger = "/logger:JetBrains.BuildServer.MSBuildLoggers.MSBuildLogger," + $MsBuildLoggerPath
        }
            
        exec { msbuild /v:m /p:Platform="Any CPU" $SlnFile /p:Configuration=Release /p:SourceAnalysisTreatErrorsAsWarnings=false /t:Rebuild $logger}
    }
}

task RunTests -precondition { return $RunTests -eq $true } {
    TeamCity-Block "Running unit tests" {
    
        Get-ChildItem $ArtifactsDirectory *.trx | ForEach { Remove-Item $_.FullName }

        exec {
            . $MsTestPath /nologo /noprompt `
                /testSettings:"$TestsDirectory\Default.testsettings" `
                /detail:duration /detail:errormessage /detail:errorstacktrace /detail:stdout `
                /testcontainer:"$TestsDirectory\FluentAssertions.Net40.Specs\bin\Release\FluentAssertions.Net40.Specs.dll" `
                /resultsfile:"$ArtifactsDirectory\FluentAssertions.Net40.Specs.trx"
        }

        exec {
            . $MsTestPath /nologo /noprompt `
                /testSettings:"$TestsDirectory\Default.testsettings" `
                /detail:duration /detail:errormessage /detail:errorstacktrace /detail:stdout `
                /testcontainer:"$TestsDirectory\FluentAssertions.Net45.Specs\bin\Release\FluentAssertions.Net45.Specs.dll" `
                /resultsfile:"$ArtifactsDirectory\FluentAssertions.Net45.Specs.trx"
        }

        exec {
            . $MsTestPath /nologo /noprompt `
                /testSettings:"$TestsDirectory\Default.testsettings" `
                /detail:duration /detail:errormessage /detail:errorstacktrace /detail:stdout `
                /testcontainer:"$TestsDirectory\FluentAssertions.Portable.Specs\bin\Release\FluentAssertions.Portable.Specs.dll" `
                /resultsfile:"$ArtifactsDirectory\FluentAssertions.Portable.Specs.trx"
        }       

        exec {
            . $MsTestPath /nologo /noprompt `
                /testSettings:"$TestsDirectory\Default.testsettings" `
                /detail:duration /detail:errormessage /detail:errorstacktrace /detail:stdout `
                /testcontainer:"$TestsDirectory\FluentAssertions.WinRT81.Specs\bin\Release\FluentAssertions.WinRT.Specs.dll" `
                /resultsfile:"$ArtifactsDirectory\FluentAssertions.WinRT81.Specs.trx"
        }

		exec {
            . $MsTestPath /nologo /noprompt `
                /testSettings:"$TestsDirectory\Default.testsettings" `
                /detail:duration /detail:errormessage /detail:errorstacktrace /detail:stdout `
                /testcontainer:"$TestsDirectory\FluentAssertions.Json.Net45.Specs\bin\Release\FluentAssertions.Json.Net45.Specs.dll" `
                /resultsfile:"$ArtifactsDirectory\FluentAssertions.Json.Net45.Specs.trx"
        }
    }
}

task RunFrameworkTests -precondition { return $RunTests } {

    $xunitRunner = "$BaseDirectory\Packages\xunit.runner.console.2.0.0\tools\xunit.console.exe"

    exec { . $xunitRunner "$TestsDirectory\TestFrameworks\XUnit2.Specs\bin\Release\XUnit2.Specs.dll" -html "$ArtifactsDirectory\XUnit2.Specs.dll.html"  }
}

task RunSilverLightTests -precondition { return $RunTests -eq $true } {

    exec { 
    . "$BaseDirectory\Lib\Lighthouse\Lighthouse.exe" -m:xap -lf:"$ArtifactsDirectory\Lighthouse.log" `
    "$TestsDirectory\FluentAssertions.Silverlight.Specs\Bin\Release\FluentAssertions.Silverlight.Specs.xap" `
    "$ArtifactsDirectory\Lighthouse.xml" 
}
}

task BuildZip {
    TeamCity-Block "Zipping up the binaries" {
        $assembly = Get-ChildItem -Path "$ArtifactsDirectory\Lib" -Filter FluentAssertions.dll -Recurse | Select-Object -first 1
                
        $versionNumber = $assembly.VersionInfo.FileVersion

        & $7zip a -r "$ArtifactsDirectory\Fluent.Assertions.$versionNumber.zip" "$ArtifactsDirectory\Lib\*" -y
    }
}

task BuildPackage {
    TeamCity-Block "Building NuGet Package" {  
        & $Nuget pack "$SrcDirectory\FluentAssertions.nuspec" -o "$ArtifactsDirectory\" 
    }
}

task BuildJsonPackage -depends ExtractVersionsFromGit {
    TeamCity-Block "Building NuGet Package (Json)" {  
        & $Nuget pack "$SrcDirectory\FluentAssertions.Json.nuspec" -o "$ArtifactsDirectory\" -Properties Version=$script:NuGetVersion 
    }
}

task PublishToMyget -precondition { return $env:NuGetApiKey } {
    TeamCity-Block "Publishing NuGet Package to Myget" {  
        $packages = Get-ChildItem $ArtifactsDirectory *.nupkg
        
        foreach ($package in $packages) {
        
            if ($NuGetPushSource) {
                & $Nuget push $package.FullName $env:NuGetApiKey -Source "$NuGetPushSource"
            } else {
                & $Nuget push $package.FullName $env:NuGetApiKey 
            }
        }
    }
}


