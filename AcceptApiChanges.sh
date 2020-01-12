#!/usr/bin/env sh
## This script is related to approval tests that protect developers from unintentional changes to the public API
## If your change does change the API on purpose and you double-checked correctness of the changes you can use this script to change the "approved" state of the API
## Make sure that you run the approval tests before running this script, because the tests generate *.received.txt files.

find Tests/Approval.Tests/ApprovedApi/FluentAssertions/ -type f -name "*received.txt" | perl -pe 'print $_; s/received/approved/' | xargs -n2 mv
