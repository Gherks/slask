
$TempPath="$Env:Temp"
if(Test-Path $TempPath)
{
    Remove-Item "$TempPath\specflow-stepmap-*" -Recurse -Force -ErrorAction 0
}

