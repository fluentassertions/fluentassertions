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
	$NuGetApiKey = ""
	
    $MsBuildLoggerPath = ""
	$Branch = ""
	$MsTestPath = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe"
	$RunTests = $false
}

task default -depends Clean, ApplyAssemblyVersioning, ApplyPackageVersioning, RestoreNugetPackages, Compile, RunTests, RunSilverLightTests, BuildZip, BuildPackage, PublishToMyget

task Clean {	
    TeamCity-Block "Clean" {
		Get-ChildItem $PackageDirectory *.nupkg | ForEach { Remove-Item $_.FullName }
		Get-ChildItem $PackageDirectory *.zip | ForEach { Remove-Item $_.FullName }
    }
}

task ExtractVersionsFromGit {

        $json = . "$GitVersionExe" "$BaseDirectory" 

        if ($LASTEXITCODE -eq 0) {
            $version = (ConvertFrom-Json ($json -join "`n"));

            TeamCity-SetBuildNumber $version.FullSemVer;

            $script:AssemblyVersion = $version.ClassicVersion;
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
	
		$fullName = "$SrcDirectory\.nuspec"

	    Set-ItemProperty -Path $fullName -Name IsReadOnly -Value $false
		
	    $content = Get-Content $fullName
	    $content = $content -replace '<version>.+</version>', ('<version>' + "$script:NuGetVersion" + '</version>')
	    Set-Content -Path $fullName $content
	}
}

task RestoreNugetPackages {
	TeamCity-Block "Restoring NuGet packages" {
		
		& $Nuget restore "$BaseDirectory\FluentAssertions.sln"	
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
	
        Run-MsTestWithTeamCityOutput `
			"$MsTestPath"`
			".NET 4.0"`
			"$TestsDirectory\FluentAssertions.Net40.Specs\bin\Release\FluentAssertions.Net40.Specs.dll"`
			"$TestsDirectory\Default.testsettings"

		Run-MsTestWithTeamCityOutput `
			"$MsTestPath"`
			".NET 4.5"`
			"$TestsDirectory\FluentAssertions.Net45.Specs\bin\Release\FluentAssertions.Net45.Specs.dll"`
			"$TestsDirectory\Default.testsettings"

		Run-MsTestWithTeamCityOutput `
			"$MsTestPath"`
			"PCL"`
			"$TestsDirectory\FluentAssertions.Portable.Specs\bin\Release\FluentAssertions.Portable.Specs.dll"`
			"$TestsDirectory\Default.testsettings"

		Run-MsTestWithTeamCityOutput `
			"$MsTestPath"`
			"WinRT"`
			"$TestsDirectory\FluentAssertions.WinRT.Specs\bin\Release\FluentAssertions.WinRT.Specs.dll"`
			"$TestsDirectory\Default.testsettings"
	}
}

task RunSilverLightTests -precondition { return $RunTests -eq $true } {

	. "$BaseDirectory\Lib\Lighthouse\Lighthouse.exe" -m:xap `
	"$TestsDirectory\FluentAssertions.Silverlight.Specs\Bin\Release\FluentAssertions.Silverlight.Specs.xap" `
	"$ArtifactsDirectory\TestResults\Lighthouse.xml"
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
		& $Nuget pack "$SrcDirectory\.nuspec" -o "$ArtifactsDirectory\" 
	}
}

task PublishToMyget -precondition { return $NuGetPushSource -and $NuGetApiKey } {
    TeamCity-Block "Publishing NuGet Package to Myget" {  
		$packages = Get-ChildItem $ArtifactsDirectory *.nupkg
		foreach ($package in $packages) {
			& $Nuget push $package.FullName $NuGetApiKey -Source "$NuGetPushSource"
		}
	}
}


