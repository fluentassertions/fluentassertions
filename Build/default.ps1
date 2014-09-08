properties {
    $BaseDirectory = Resolve-Path ..     
    $Nuget = "$BaseDirectory\Tools\NuGet.exe"
	$SlnFile = "$BaseDirectory\FluentAssertions.sln"
	$7zip = "$BaseDirectory\Tools\7z.exe"
	$PackageDirectory = "$BaseDirectory\Package"
	$ApiKey = ""
    $BuildNumber = 999
    $MsBuildLoggerPath = ""
	$Branch = ""
	$MsTestPath = "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\MSTest.exe"
	$RunTests = $false
}

task default -depends Clean, ApplyPackageVersioning, Compile, RunTests, BuildZip, BuildPackage, PublishToMyget

task Clean {	
    TeamCity-Block "Clean" {
		Get-ChildItem $PackageDirectory *.nupkg | ForEach { Remove-Item $_.FullName }
		Get-ChildItem $PackageDirectory *.zip | ForEach { Remove-Item $_.FullName }
    }
}

task ApplyPackageVersioning {
    TeamCity-Block "Updating package version with build number $BuildNumber" {   
	
		$fullName = "$BaseDirectory\Package\.nuspec"

	    Set-ItemProperty -Path $fullName -Name IsReadOnly -Value $false
		
		$postfix = "";
		if ($Branch -eq "develop") {
			$postfix = "-dev"
		}

	    $content = Get-Content $fullName
	    $content = $content -replace '<version>(\d+)\.(\d+)\.(\d+).*</version>', ('<version>$1.$2.$3-build' + $BuildNumber.ToString("000") + $postfix + '</version>')
	    Set-Content -Path $fullName $content
	}
}

task Compile {
    TeamCity-Block "Compiling" {  
       
        if ($MsBuildLoggerPath -ne "")
        {
            Write-Host "Using TeamCity MSBuild logger"
            $logger = "/logger:JetBrains.BuildServer.MSBuildLoggers.MSBuildLogger," + $MsBuildLoggerPath
        }
            
	    exec { msbuild /v:m /p:Platform="Any CPU" $SlnFile /p:Configuration=Release /t:Rebuild $logger}
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

task BuildZip {
	TeamCity-Block "Zipping up the binaries" {
		$assembly = Get-ChildItem -Path $BaseDirectory\Package\Lib -Filter FluentAssertions.dll -Recurse | Select-Object -first 1
				
		$versionNumber = $assembly.VersionInfo.ProductVersion

		& $7zip a -r "$BaseDirectory\Package\Fluent.Assertions.$versionNumber.zip" "$BaseDirectory\Package\Lib\*" -y
	}
}

task BuildPackage {
    TeamCity-Block "Building NuGet Package" {  
		& $Nuget pack "$PackageDirectory\.nuspec" -o "$PackageDirectory\" 
	}
}

task PublishToMyget -precondition { return ($Branch -eq "master" -or $Branch -eq "<default>" -or $Branch -eq "develop") -and ($ApiKey -ne "") } {
    TeamCity-Block "Publishing NuGet Package to Myget" {  
		$packages = Get-ChildItem $PackageDirectory *.nupkg
		foreach ($package in $packages) {
			& $Nuget push $package.FullName $ApiKey -Source "https://www.myget.org/F/fluentassertions/api/v2/package"
		}
	}
}


