# Aurelia DotNet Tooling
*Official .NET tooling for Aurelia.*

# Using the .NET Extension DLL

### Cli generation
Generate Aurelia CLI project with any options provided and place in the project directory of the .NET Core application

### Place the following in the ConfigureServices Method in startup.cs
``` c#
// In production, the Aurelia files will be served from this directory
services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "%aurelia-root%/dist";
});
```
### Place the following in the configure method in Startup.cs
``` c#
app.UseStaticFiles();
app.UseSpaStaticFiles();
app.UseSpa(spa => {
    spa.Options.SourcePath = "%replaceWithAureliaCLIRoot%";

    if (env.IsDevelopment())
    {
        spa.UseAureliaCliServer(); // Optional HMR (Hot Module Reload)
    }
});

```

# TODOS
## Language Service
- VScode plugin
- Language Server/Client

## Visual Studio
- Project Templates utilizing dotnet new behind the scenes
- Item Templates utilizing CLI

## CLI Updates
- Unattended mode to help with generation of projects and usage of VsCode command pallete

## Templates - To be hosted on nuget (All options have default CLI appended for generation eg. SCSS/Webpack/Etc.)
- dotnet new aurelia - Api based no security quick start
- dotnet new aurelia-mvc - Usage of MVC views and integration of Aurelia
- dotnet new aurelia-secure - Add token based authentication and dashboard to manage. **All built with Aurelia**
- dotnet new aurelia-ssr - Generate a dotnet template that will use the embedded node process to render the Aurelia application

## Scaffolding
- Reverse engineer API endpoints and generate corresponding aurelia views based on the models

