properties {
    $BaseDirectory = Resolve-Path ..     
    $Nuget = "$BaseDirectory\Tools\NuGet.exe"
	$SlnFile = "$BaseDirectory\FluentAssertions.sln"
	$PackageDirectory = "$BaseDirectory\Package"
	$ApiKey = ""
    $BuildNumber = 9999
    $MsBuildLoggerPath = ""
	$Branch = ""
}

task default -depends Clean, ApplyAssemblyVersioning, ApplyPackageVersioning, Compile, BuildPackage, PublishToMyget

task Clean {	
    TeamCity-Block "Clean" {
		Get-ChildItem $PackageDirectory *.nupkg | ForEach { Remove-Item $_.FullName }
    }
}

task ApplyAssemblyVersioning {
    TeamCity-Block "Updating assembly version with build number $BuildNumber" {   
	
		$fullName = "$BaseDirectory\SolutionInfo.cs"

	    Set-ItemProperty -Path $fullName -Name IsReadOnly -Value $false

	    $content = Get-Content $fullName
	    $content = $content -replace '"(\d+)\.(\d+)\.(\d+)"', ('"$1.$2.' + $BuildNumber + '"')
	    Set-Content -Path $fullName $content
	}
}

task ApplyPackageVersioning {
    TeamCity-Block "Updating package version with build number $BuildNumber" {   
	
		$fullName = "$BaseDirectory\Package\.nuspec"

	    Set-ItemProperty -Path $fullName -Name IsReadOnly -Value $false

	    $content = Get-Content $fullName
	    $content = $content -replace '<version>(\d+)\.(\d+)\.(\d+)(.*)</version>', ('<version>$1.$2.' + $BuildNumber + '$4</version>')
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
            
	    exec { msbuild /v:q /p:Platform="Any CPU" $SlnFile /p:Configuration=Release /t:Rebuild $logger}
    }
}

task BuildPackage {
    TeamCity-Block "Building NuGet Package" {  
		& $Nuget pack "$PackageDirectory\.nuspec" -o "$PackageDirectory\" 
	}
}

task PublishToMyget -precondition { return ($Branch -eq "master") -and ($ApiKey -ne "") } {
    TeamCity-Block "Publishing NuGet Package to Myget" {  
		$packages = Get-ChildItem $PackageDirectory *.nupkg
		foreach ($package in $packages) {
			& $Nuget push $package.FullName $ApiKey -Source "https://www.myget.org/F/fluentassertions/api/v2/package"
		}
	}
}