
#if Secure
using Aurelia.DotNet.DataAccess;
using Aurelia.DotNet.DataAccess.Interfaces;
using Aurelia.DotNet.DataAccess.Models;
#endif
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
#if signalR
using Aurelia.DotNet.Spa.Hubs;
#endif
//using Aurelia.Dotnet;

namespace Aurelia.DotNet.Spa
{

#if Secure
    public class Startup
    {
        private IHostingEnvironment _currentEnvironment { get; set; }

        public Startup(IHostingEnvironment currentEnvironment, IConfiguration configuration)
        {
            Configuration = configuration;
            _currentEnvironment = currentEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Conventions.Add(new AuthorizeFilterConvention()); //secures the entire application
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the Aurelia files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

#if signalR
            services.AddSignalR();
#endif

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Aurelia.DotNet.Spa"));
                options.UseOpenIddict();
            });

            services.AddIdentity<User, Role>()
                           .AddEntityFrameworkStores<ApplicationDbContext>()
                           .AddDefaultTokenProviders();

            services.ConfigureIdentity(_currentEnvironment)
            .ConfigureOpenId()
            .ConfigureAuthorization()
            .ConfigureApplicationDI()
            .ConfigureAuthorizationHandlers()
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            })
            .AddCors()
            .ConfigureSwagger();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDatabaseInitializer initializer, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Seed database
            initializer.Seed();
            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseSwagger(y => y.PreSerializeFilters.Add((swagger, _) => swagger.Schemes = new[] { "https" }));
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"Aurelia.DotNet.Spa API {description.GroupName.ToUpperInvariant()}");
                }

            });

#if signalR
            app.UseSignalR(routes =>
            {
                routes.MapHub<SampleHub>("/sample");
            });
#endif

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAureliaCliServer();
                }
            });
        }
    }
#else
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the Aurelia files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "%aurelia-root%/dist";
            });

#if signalR
            services.AddSignalR();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
#if signalR
            app.UseSignalR(routes =>
            {
                routes.MapHub<SampleHub>("/sample");
            });
#endif
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "%aurelia-root%";

                if (env.IsDevelopment())
                {
                    spa.UseAureliaCliServer();
                }
            });
        }
    }
#endif
}
