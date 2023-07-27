param
(
  [Parameter(Mandatory=$true, HelpMessage="Specify the csproj")][string]$csproj = '',
  [Parameter(Mandatory=$false, HelpMessage="App version name")][string]$VersionName = '1.0.0',
  [Parameter(Mandatory=$false, HelpMessage="App version code")][string]$VersionCode = '1'
)


if ($null -eq $csproj -Or $csproj -eq '') {
        return
    }

$FileXml = [xml] (Get-Content $csproj)
$version = $FileXml.Project.PropertyGroup.ApplicationVersion
$versionDisplay = $FileXml.Project.PropertyGroup.ApplicationDisplayVersion
$versionDisplay = $VersionName
$version = $VersionCode
$FileXml.Save($csproj)
