function Run-MsTestWithTeamCityOutput {
    param(
        [string]$msTestExePath,
        [string]$name,
        [string[]]$assemblies,
        [string]$configfile,
        [string]$category
    )

    TeamCity-TestSuiteStarted $name
    
	$exec = """$msTestExePath"" /nologo /noprompt /testSettings:""$configFile"" /detail:duration /detail:errormessage /detail:errorstacktrace /detail:stdout"
	
	if ($category){
		$exec += " /category:""$category"""
	}
	
	foreach($assembly in $assemblies){
		$exec += " /testcontainer:""$assembly"""
	}
	
	echo $exec
	
    # exec { iex "& $exec" } | Transform-MSTestOuputToTeamCity
     iex "& $exec" | Transform-MSTestOuputToTeamCity

    TeamCity-TestSuiteFinished $name
}

function Transform-MSTestOuputToTeamCity {
    param(
        [Parameter(ValueFromPipeline=$true)]
        [string]$InputObject
    )

    begin {
        $testOuputRegExString = ".*(?<result>Passed|Failed|Inconclusive|Ignored|Not Executed)\s+(?<test>\S+)(?:\n|\r|\rn)?" `
			+ "(?:\[errorstacktrace\] = (?<errorstacktrace>.*?))?" `
			+ "(?:\[stdout\] = (?<stdout>.*?))?" `
			+ "(?:\[errormessage\] = (?<errormessage>.*?))?" `
			+ "\[duration\] = (?<duration>.*)"
                                 
        $options = [System.Text.RegularExpressions.RegExOptions]::SingleLine -bor [System.Text.RegularExpressions.RegExOptions]::Compiled

        $testOuputRegEx = New-Object regex $testOuputRegExString, $options

        $currentTestOutput = ""
		$testRunFinished = $False
    }

    process {
		if(!$testRunFinished) {
			if($InputObject -match "(?<Passed>\d+)/(?<Total>\d+) test\(s\) Passed") {
				# Stop trying to parse the rest of output
				$testRunFinished = $True
				$passed = $matches['Passed']
				$total = $matches['Total']
				"Test run finished, passed: $passed, total: $total"
			}
			else {
		
		        $currentTestOutput += $InputObject + "`n"
									
		        if($testOuputRegEx.IsMatch($currentTestOutput)) { 
		                                                
		            $match = $testOuputRegEx.Match($currentTestOutput) 

		            $testName = $match.Groups['test'].Value
		            $testResult = $match.Groups['result'].Value
		            $testDuration = $match.Groups['duration'].Value
		            $testOutput = $match.Groups['stdout'].Value
		            $testErrorMessage = $match.Groups['errormessage'].Value
		            $testErrorStacktrace = $match.Groups['errorstacktrace'].Value
					
					if($testResult -eq 'Ignored' -or $testResult -eq 'Not Executed') {
						TeamCity-TestIgnored $testName
					}
					elseif ($testResult -eq 'Inconclusive') {
						TeamCity-TestIgnored $testName 'Test inconclusive'
					}
					else {			
						TeamCity-TestStarted $testName
						TeamCity-TestOutput $testName $testOutput

			            if($testResult -eq 'Failed'){
			                TeamCity-TestFailed $testName $testErrorMessage $testErrorStacktrace
			            }

			            TeamCity-TestFinished $testName ([timespan]$testDuration).TotalMilliseconds
					}

		            $currentTestOutput = ""
		        }
			}
		}
    }
}
