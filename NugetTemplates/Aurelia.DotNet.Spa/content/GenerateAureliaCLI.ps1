
param(
    [string]$bundler = "%bundler%",
    [string]$http = "%http%",
    [string]$transpiler = "%transpiler%",
    [string]$minification = "%minification%",
    [string]$cssProcessor = "%cssProcessor%",
    [string]$postProcessor = "%postProcessor%",
    [string]$unitTesting = "%unitTesting%",
    [string]$integrationTesting = "%integrationTesting%",
    [string]$editor = "%editor%",
    [string]$navigation = "%nav%"
)

$aureliaCli = npm info aurelia-cli version
if(!$aureliaCli){
npm install aurelia-cli@latest
}
cd Aurelia.DotNet.Spa
au new '%aurelia-root%' --unattended --select $bundler,$http,$transpiler,$minification,$cssProcessor,$postProcessor,$unitTesting,$integrationTesting,$editor,$navigation
#Rename-Item -path 'Aurelia.DotNet.CSharp' -newName 
#$configFiles = Get-ChildItem 'ClientApp' *.* -rec
#foreach ($file in $configFiles) {
#    (Get-Content $file.PSPath) |
#        Foreach-Object { $_ -replace "Aurelia.DotNet.CSharp", (Get-Location).Name } |
#        Set-Content $file.PSPath
#}
Remove-Item '..\GenerateAureliaCLI.ps1'
Remove-Item '..\GenerateAureliaCLI.sh'