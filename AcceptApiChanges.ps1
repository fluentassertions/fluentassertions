## This script is related to approval tests that protect developers from unintentional changes to the public API
## If your change does change the API on purpose and you double-checked correctness of the changes you can use this script to change the "approved" state of the API
## Make sure that you run the approval tests before running this script, because the tests generate *.received.txt files.


## Go to the directory where "approved" API is saved as .txt files
Set-Location -Path .\Tests\Approval.Tests\ApprovedApi\FluentAssertions

## Remove current "approved" API 
Remove-Item *.approved.txt*

## Copy new API from .received.txt files to .approved.txt files
## Note that .received.txt files are ignored in git and are not part of the repository
Get-ChildItem -Filter "*received.txt" | ForEach-Object {
	$NewName = $_.FullName -replace 'received.txt', 'approved.txt'
	Copy-Item $_.FullName $NewName
}