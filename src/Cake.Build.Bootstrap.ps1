
<#PSScriptInfo

.VERSION 1.0

.GUID d20c6f39-3c88-4385-8f1c-a93f28d4ca36

.AUTHOR dwolan

.COMPANYNAME 

.COPYRIGHT 

.TAGS 

.LICENSEURI 

.PROJECTURI 

.ICONURI 

.EXTERNALMODULEDEPENDENCIES 

.REQUIREDSCRIPTS 

.EXTERNALSCRIPTDEPENDENCIES 

.RELEASENOTES


#>

<# 

.DESCRIPTION 
 Configures Cake build 

#> 
Param()

$buildFolder = ".\Build";

if(-Not (Test-Path $buildFolder)){
    New-Item .\Build -ItemType Directory
}

"build.ps1","build.cake"|%{Invoke-RestMethod -Uri "https://raw.githubusercontent.com/cake-build/bootstrapper/master/res/scripts/$($_)" -OutFile $buildFolder\$_}