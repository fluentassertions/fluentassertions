properties {
    $BaseDirectory = Resolve-Path ..     
    $NugetPath = "$BaseDirectory\Tools\NuGet.exe"
	$SlnFile = "$BaseDirectory\FluentAssertions.sln"
	$PackageDirectory = "$BaseDirectory\Package"
    $BuildNumber = 9999
    $MsBuildLoggerPath = ""
}

task default -depends Clean, ApplyAssemblyVersioning, ApplyPackageVersioning, Compile, BuildPackage

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
		& $NugetPath pack "$PackageDirectory\.nuspec" -o "$PackageDirectory\"
	}
}
