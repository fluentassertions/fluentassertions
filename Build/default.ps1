properties {
    $BaseDirectory = Resolve-Path ..     
    $Nuget = "$BaseDirectory\.nuget\NuGet.exe"
	$SlnFile = "$BaseDirectory\FluentAssertions.sln"
	$7zip = "$BaseDirectory\Tools\7z.exe"
	$PackageDirectory = "$BaseDirectory\Package"

	$NuGetPushSource = ""
	$NuGetApiKey = ""
	
    $AssemblyVer = "1.2.3.4"
	$InformationalVersion = "1.2.3-unstable.34+34.Branch.develop.Sha.19b2cd7f494c092f87a522944f3ad52310de79e0"
	$NuGetVersion = "1.2.3-unstable0012"
    $MsBuildLoggerPath = ""
	$Branch = ""
	$MsTestPath = "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\MSTest.exe"
	$RunTests = $false
}

task default -depends Clean, ApplyAssemblyVersioning, ApplyPackageVersioning, RestoreNugetPackages, Compile, RunTests, RunSilverLightTests, BuildZip, BuildPackage, PublishToMyget

task Clean {	
    TeamCity-Block "Clean" {
		Get-ChildItem $PackageDirectory *.nupkg | ForEach { Remove-Item $_.FullName }
		Get-ChildItem $PackageDirectory *.zip | ForEach { Remove-Item $_.FullName }
    }
}

task ApplyAssemblyVersioning {
    TeamCity-Block "Updating solution info versions with build number $BuildNumber" {   
	
		$infos = Get-ChildItem -Path $BaseDirectory -Filter SolutionInfo.cs -Recurse
		
		foreach ($info in $infos) {
		    Write-Host "Updating " + $info.FullName
			Set-ItemProperty -Path $info.FullName -Name IsReadOnly -Value $false
			
		    $content = Get-Content $info.FullName
		    $content = $content -replace 'AssemblyVersion\("(.+)"\)', ('AssemblyVersion("' + $AssemblyVer + '")')
			$content = $content -replace 'AssemblyFileVersion\("(.+)"\)', ('AssemblyFileVersion("' + $AssemblyVer + '")')
			$content = $content -replace 'AssemblyInformationalVersion\("(.+)"\)', ('AssemblyInformationalVersion("' + $InformationalVersion + '")')
		    Set-Content -Path $info.FullName $content
		}	
	}
}

task ApplyPackageVersioning {
    TeamCity-Block "Updating package version with build number $BuildNumber" {   
	
		$fullName = "$BaseDirectory\Package\.nuspec"

	    Set-ItemProperty -Path $fullName -Name IsReadOnly -Value $false
		
	    $content = Get-Content $fullName
	    $content = $content -replace '<version>.+</version>', ('<version>' + "$NuGetVersion" + '</version>')
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
			"$BaseDirectory\FluentAssertions.Net40.Specs\bin\Release\FluentAssertions.Net40.Specs.dll"`
			"$BaseDirectory\Default.testsettings"

		Run-MsTestWithTeamCityOutput `
			"$MsTestPath"`
			".NET 4.5"`
			"$BaseDirectory\FluentAssertions.Net45.Specs\bin\Release\FluentAssertions.Net45.Specs.dll"`
			"$BaseDirectory\Default.testsettings"

		Run-MsTestWithTeamCityOutput `
			"$MsTestPath"`
			"PCL"`
			"$BaseDirectory\FluentAssertions.Portable.Specs\bin\Release\FluentAssertions.Portable.Specs.dll"`
			"$BaseDirectory\Default.testsettings"

		Run-MsTestWithTeamCityOutput `
			"$MsTestPath"`
			"WinRT"`
			"$BaseDirectory\FluentAssertions.WinRT.Specs\bin\Release\FluentAssertions.WinRT.Specs.dll"`
			"$BaseDirectory\Default.testsettings"
	}
}

task RunSilverLightTests {

	. "$BaseDirectory\Tools\Lighthouse\Lighthouse.exe" -m:xap `
	"$BaseDirectory\FluentAssertions.Silverlight.Specs\Bin\Release\FluentAssertions.Silverlight.Specs.xap" `
	"$BaseDirectory\TestResults\Lighthouse.xml"
}

task BuildZip {
	TeamCity-Block "Zipping up the binaries" {
		$assembly = Get-ChildItem -Path $BaseDirectory\Package\Lib -Filter FluentAssertions.dll -Recurse | Select-Object -first 1
				
		$versionNumber = $assembly.VersionInfo.FileVersion

		& $7zip a -r "$BaseDirectory\Package\Fluent.Assertions.$versionNumber.zip" "$BaseDirectory\Package\Lib\*" -y
	}
}

task BuildPackage {
    TeamCity-Block "Building NuGet Package" {  
		& $Nuget pack "$PackageDirectory\.nuspec" -o "$PackageDirectory\" 
	}
}

task PublishToMyget -precondition { return $NuGetPushSource -and $NuGetApiKey } {
    TeamCity-Block "Publishing NuGet Package to Myget" {  
		$packages = Get-ChildItem $PackageDirectory *.nupkg
		foreach ($package in $packages) {
			& $Nuget push $package.FullName $NuGetApiKey -Source "$NuGetPushSource"
		}
	}
}


