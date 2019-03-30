(npm list --depth 0 --global aurelia-cli | findstr aurelia-cli@1.0.0-beta.14 && (echo found)) || (call npm install aurelia-cli@latest -g)
cd Aurelia.DotNet.Spa
au new %aurelia-root% --unattended --select %bundler%,%http%,%transpiler%,%minification%,%cssProcessor%,%postProcessor%,%unitTesting%,%integrationTesting%,%editor%,%nav%
del ..\GenerateAureliaCLI*