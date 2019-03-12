aureliaCli = npm info aurelia-cli version
if(!$aureliaCli){
npm install aurelia-cli@latest
}
cd Aurelia.DotNet.Spa
au new '%aurelia-root%' --unattended --select %bundler%,%http%,%transpiler%,%minification%,%cssProcessor%,%postProcessor%,%unitTesting%,%integrationTesting%,%editor%,%nav%
rm '..\GenerateAureliaCLI.sh'
rm '..\GenerateAureliaCLI.ps1'