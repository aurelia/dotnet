npm install aurelia-cli@latest -g
cd Aurelia.DotNet.Spa
au new '%aurelia-root%' --unattended --select %bundler%,%http%,%transpiler%,%minification%,%cssProcessor%,%postProcessor%,%unitTesting%,%integrationTesting%,%editor%,%nav%
rm '..\GenerateAureliaCLI.sh'
rm '..\GenerateAureliaCLI.ps1'