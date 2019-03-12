#if (Secure)
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using AspNet.Security.OpenIdConnect.Primitives;
using Aurelia.DotNet.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Aurelia.DotNet.Spa.Security;
using Aurelia.DotNet.DataAccess.Common;
using Aurelia.DotNet.Logic.Interfaces;
using Aurelia.DotNet.Logic;
using Aurelia.DotNet.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using OpenIddict.Validation;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace Aurelia.DotNet.Spa
{
    public static class ServiceCollectionHelpers
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IHostingEnvironment hostingEnvironment)
        {
            services.Configure((System.Action<IdentityOptions>)(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                // Password settings
                //options.Password./*

                if (hostingEnvironment.IsDevelopment())
                {
                    RemoveAllPasswordRestrictions(options);
                }


                //// Lockout settings
                //options.Lockout./*

                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            }));
            return services;
        }

        private static void RemoveAllPasswordRestrictions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 0;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        }

        private static string AssemblyName => typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        private static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = $"{AssemblyName}.xml";
                return Path.Combine(basePath, fileName);
            }
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(options =>
            {
                // Add a swagger document for each discovered API version  
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new Info()
                    {
                        Title = $"Aurelia.DotNet.SecureSpa API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = description.IsDeprecated ? $"Aurelia.DotNet.SecureSpa API - DEPRECATED" : "Aurelia.DotNet.SecureSpa API",

                    });
                }
                options.OperationFilter<AuthorizeOperationFilter>();
                options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "password",
                    TokenUrl = "/connect/token",
                    Description = "LEAVE CLIENT INFO BLANK"
                });
                options.IncludeXmlComments(XmlCommentsFilePath);

            });
            return services;
        }

        public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy(Policies.Default, policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
                authorizationOptions.AddPolicy(Policies.GetVehicles, x => x.RequireClaim(Claims.Permission, Permissions.Admin));
            });
            return services;
        }

        public static IServiceCollection ConfigureApplicationDI(this IServiceCollection services)
        {
            services.AddScoped<IAccountLogic, AccountLogic>();
            services.AddScoped<IVehicleLogic, VehicleLogic>();
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);


            return services;
        }

        public static IServiceCollection ConfigureAuthorizationHandlers(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection ConfigureOpenId(this IServiceCollection services)
        {
            services.AddOpenIddict()
                            .AddCore(options =>
                            {
                                options.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
                            })
                            .AddServer(options =>
                            {
                                options.UseMvc();
                                options.EnableTokenEndpoint("/connect/token");
                                options.AllowPasswordFlow();
                                options.AllowRefreshTokenFlow();
                                options.AcceptAnonymousClients();
                                options.RegisterScopes(
                                                OpenIdConnectConstants.Scopes.OpenId,
                                                OpenIdConnectConstants.Scopes.Email,
                                                OpenIdConnectConstants.Scopes.Phone,
                                                OpenIdConnectConstants.Scopes.Profile,
                                                OpenIdConnectConstants.Scopes.OfflineAccess,
                                                OpenIddictConstants.Scopes.Roles);
                                //The following will always renew tokens on refresh request
                                // options.UseRollingTokens();
                                // Use the following to go to JWT 
                                // options.UseJsonWebTokens();
                            })
                            // For JWT tokens, use the Microsoft JWT bearer handler.
                            .AddValidation();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });
            return services;

        }
    }
}
#endif