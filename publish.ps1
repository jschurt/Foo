#https://www.red-gate.com/simple-talk/sysadmin/powershell/how-to-use-parameters-in-powershell/

if ($versionNumber -eq $null)
{
    $versionNumber = Read-Host 'Version Number'
}

if ($versionNumber -notmatch "^\d+\.\d+\.\d+(-\w+)?$")
{
    Write-Host "Invalid Version Number."
    return
}

$hasError = 0

foreach ($projectName in $args)
{
    $projectDir = $PSScriptRoot + "\" + $projectName

    Remove-Item $projectDir\bin\Release\*.nupkg
    
    dotnet pack $projectDir -c Release -p:Version=$versionNumber

    if ($LASTEXITCODE -ne 0)
    {
        return
    }

    dotnet nuget push $projectDir\bin\Release\*.nupkg -s Local

    if ($LASTEXITCODE -ne 0)
    {
        $contine = Read-Host 'Continue? Y/(N)'

        if ($contine -ne "Y")
        {
            return
        }
    }   
}


