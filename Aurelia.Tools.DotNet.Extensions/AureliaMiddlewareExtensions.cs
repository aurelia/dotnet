using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using System;

namespace Aurelia.DotNet.Extensions
{
    public static class AureliaMiddlewareExtensions
    {
        public static void UseAureliaCliServer (
            this ISpaBuilder spaBuilder,
            string npmScript = "au run",
            //PackageManager packageManager = PackageManager.Npm
            bool hotModuleReload = true
            )
        {
            if (spaBuilder == null)
            {
                throw new ArgumentNullException(nameof(spaBuilder));
            }

            var spaOptions = spaBuilder.Options;

            if (string.IsNullOrEmpty(spaOptions.SourcePath))
            {
                throw new InvalidOperationException($"To use {nameof(AureliaMiddlewareExtensions)}, you must supply a non-empty value for the {nameof(SpaOptions.SourcePath)} property of {nameof(SpaOptions)} when calling {nameof(SpaApplicationBuilderExtensions.UseSpa)}.");
            }

            AureliaCliDevelopmentServer.Attach(spaBuilder, npmScript, PackageManager.Npm, hotModuleReload);
        }
    }
}
