if ($env:TEAMCITY_VERSION) {
	# When PowerShell is started through TeamCity's Command Runner, the standard
	# output will be wrapped at column 80 (a default). This has a negative impact
	# on service messages, as TeamCity quite naturally fails parsing a wrapped
	# message. The solution is to set a new, much wider output width. It will
	# only be set if TEAMCITY_VERSION exists, i.e., if started by TeamCity.
	$host.UI.RawUI.BufferSize = New-Object System.Management.Automation.Host.Size(8192,50)
}

function TeamCity-TestSuiteStarted([string]$name) {
	TeamCity-WriteServiceMessage 'testSuiteStarted' @{ name=$name }
}

function TeamCity-TestSuiteFinished([string]$name) {
	TeamCity-WriteServiceMessage 'testSuiteFinished' @{ name=$name }
}

function TeamCity-TestStarted([string]$name) {
	TeamCity-WriteServiceMessage 'testStarted' @{ name=$name }
}

function TeamCity-TestFinished([string]$name, [int]$duration) {
	$messageAttributes = @{name=$name; duration=$duration}
	
	if ($duration -gt 0) {
		$messageAttributes.duration=$duration
	}

	TeamCity-WriteServiceMessage 'testFinished' $messageAttributes
}

function TeamCity-TestIgnored([string]$name, [string]$message='') {
	TeamCity-WriteServiceMessage 'testIgnored' @{ name=$name; message=$message }
}

function TeamCity-TestOutput([string]$name, [string]$output) {
	TeamCity-WriteServiceMessage 'testStdOut' @{ name=$name; out=$output }
}

function TeamCity-TestError([string]$name, [string]$output) {
	TeamCity-WriteServiceMessage 'testStdErr' @{ name=$name; out=$output }
}

function TeamCity-TestFailed([string]$name, [string]$message, [string]$details='', [string]$type='', [string]$expected='', [string]$actual='') {
	$messageAttributes = @{ name=$name; message=$message; details=$details }

	if (![string]::IsNullOrEmpty($type)) {
		$messageAttributes.type = $type
	}
	
	if (![string]::IsNullOrEmpty($expected)) {
		$messageAttributes.expected=$expected
	}
	if (![string]::IsNullOrEmpty($actual)) {
		$messageAttributes.actual=$actual
	}

	TeamCity-WriteServiceMessage 'testFailed' $messageAttributes
}

# See http://confluence.jetbrains.net/display/TCD5/Manually+Configuring+Reporting+Coverage
function TeamCity-ConfigureDotNetCoverage([string]$key, [string]$value) {
    TeamCity-WriteServiceMessage 'dotNetCoverage' @{ $key=$value }
}

function TeamCity-ImportDotNetCoverageResult([string]$tool, [string]$path) {
	TeamCity-WriteServiceMessage 'importData' @{ type='dotNetCoverage'; tool=$tool; path=$path }
}

# See http://confluence.jetbrains.net/display/TCD5/FxCop_#FxCop_-UsingServiceMessages
function TeamCity-ImportFxCopResult([string]$path) {
	TeamCity-WriteServiceMessage 'importData' @{ type='FxCop'; path=$path }
}

function TeamCity-ImportNUnitReport([string]$path) {
	TeamCity-WriteServiceMessage 'importData' @{ type='nunit'; path=$path }
}

function TeamCity-ImportJSLintReport([string]$path) {
	TeamCity-WriteServiceMessage 'importData' @{ type='jslint'; path=$path }
}

function TeamCity-PublishArtifact([string]$path) {
	TeamCity-WriteServiceMessage 'publishArtifacts' $path
}

function TeamCity-ReportBuildStart([string]$message) {
	TeamCity-WriteServiceMessage 'progressStart' $message
}

function TeamCity-ReportBuildProgress([string]$message) {
	TeamCity-WriteServiceMessage 'progressMessage' $message
}

function TeamCity-ReportBuildFinish([string]$message) {
	TeamCity-WriteServiceMessage 'progressFinish' $message
}

function TeamCity-ReportBuildStatus([string]$status, [string]$text='') {
	TeamCity-WriteServiceMessage 'buildStatus' @{ status=$status; text=$text }
}

function TeamCity-SetBuildNumber([string]$buildNumber) {
	TeamCity-WriteServiceMessage 'buildNumber' $buildNumber
}

function TeamCity-SetBuildStatistic([string]$key, [string]$value) {
	TeamCity-WriteServiceMessage 'buildStatisticValue' @{ key=$key; value=$value }
}

function TeamCity-Block([string]$name, [scriptblock]$cmd) {
    TeamCity-WriteServiceMessage 'blockOpened' @{ name=$name }
    & $cmd    
    TeamCity-WriteServiceMessage 'blockClosed' @{ name=$name }
}

function TeamCity-CreateInfoDocument([string]$buildNumber='', [boolean]$status=$true, [string[]]$statusText=$null, [System.Collections.IDictionary]$statistics=$null) {
	$doc=New-Object xml;
	$buildEl=$doc.CreateElement('build');
	
	if (![string]::IsNullOrEmpty($buildNumber)) {
		$buildEl.SetAttribute('number', $buildNumber);
	}
	
	$buildEl=$doc.AppendChild($buildEl);
	
	$statusEl=$doc.CreateElement('statusInfo');
	if ($status) {
		$statusEl.SetAttribute('status', 'SUCCESS');
	} else {
		$statusEl.SetAttribute('status', 'FAILURE');
	}
	
	if ($statusText -ne $null) {
		foreach ($text in $statusText) {
			$textEl=$doc.CreateElement('text');
			$textEl.SetAttribute('action', 'append');
			$textEl.set_InnerText($text);
			$textEl=$statusEl.AppendChild($textEl);
		}
	}	
	
	$statusEl=$buildEl.AppendChild($statusEl);
	
	if ($statistics -ne $null) {
		foreach ($key in $statistics.Keys) {
			$val=$statistics.$key
			if ($val -eq $null) {
				$val=''
			}
			
			$statEl=$doc.CreateElement('statisticsValue');
			$statEl.SetAttribute('key', $key);
			$statEl.SetAttribute('value', $val.ToString());
			$statEl=$buildEl.AppendChild($statEl);
		}
	}
	
	return $doc;
}

function TeamCity-WriteInfoDocument([xml]$doc) {
	$dir=(Split-Path $buildFile)
	$path=(Join-Path $dir 'teamcity-info.xml')
	
	$doc.Save($path);
}

function TeamCity-WriteServiceMessage([string]$messageName, $messageAttributesHashOrSingleValue) {
	function escape([string]$value) {
		([char[]] $value | 
				%{ switch ($_) 
						{
								"|" { "||" }
								"'" { "|'" }
								"`n" { "|n" }
								"`r" { "|r" }
								"[" { "|[" }
								"]" { "|]" }
								([char] 0x0085) { "|x" }
								([char] 0x2028) { "|l" }
								([char] 0x2029) { "|p" }
								default { $_ }
						}
				} ) -join ''
		}

	if ($messageAttributesHashOrSingleValue -is [hashtable]) {
		$messageAttributesString = ($messageAttributesHashOrSingleValue.GetEnumerator() | 
			%{ "{0}='{1}'" -f $_.Key, (escape $_.Value) }) -join ' '
	} else {
		$messageAttributesString = ("'{0}'" -f (escape $messageAttributesHashOrSingleValue))
	}

	Write-Output "##teamcity[$messageName $messageAttributesString]"
}
