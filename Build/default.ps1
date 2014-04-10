properties {
    $BaseDirectory = Resolve-Path ..     
    $NugetPath = "$BaseDirectory\Tools\NuGet.exe"
	$SlnFile = "$BaseDirectory\FluentAssertions.sln"
    $BuildNumber = 9999
    $MsBuildLoggerPath = ""
}

task default -depends Clean, Compile

task Clean {	
    TeamCity-Block "Clean" {
    }
}

task ApplyAssemblyVersioning {
    TeamCity-Block "Apply Assembly Versioning" {   
        Get-ChildItem -Path $SrcDirectory -Filter "?*AssemblyInfo.cs" -Recurse -Force |
        foreach-object{  
	        Write-Host "Updating " $_.FullName "with build number" $BuildNumber

            Set-ItemProperty -Path $_.FullName -Name IsReadOnly -Value $false

	        $content = Get-Content $_.FullName
	        $content = $content -replace '"(\d+)\.(\d+)\.(\d+)\.(\d+)"', ('"$1.$2.$3.' + $BuildNumber + '"')
	        Set-Content -Path $_.FullName $content
        }    
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



