
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
au new Aurelia.DotNet.CSharp --unattended --select $bundler,$http,$transpiler,$minification,$cssProcessor,$postProcessor,$unitTesting,$integrationTesting,$editor,$navigation
Rename-Item -path 'Aurelia.DotNet.CSharp' -newName 'ClientApp'
#$configFiles = Get-ChildItem 'ClientApp' *.* -rec
#foreach ($file in $configFiles) {
#    (Get-Content $file.PSPath) |
#        Foreach-Object { $_ -replace "Aurelia.DotNet.CSharp", (Get-Location).Name } |
#        Set-Content $file.PSPath
#}
Remove-Item '.\GenerateAureliaCLI.ps1'