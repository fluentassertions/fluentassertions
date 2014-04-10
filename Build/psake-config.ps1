$config.buildFileName="default.ps1"
$config.framework = "4.5.1x86"
$config.taskNameFormat="Executing {0}"
$config.verboseError= $false
$config.coloredOutput = $true
$config.modules=(".\modules\*.psm1")
$config.moduleScope="global"
$config.taskNameFormat= { param($taskName) "Executing $taskName at $(get-date)" }
