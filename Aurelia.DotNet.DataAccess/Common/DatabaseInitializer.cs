using Aurelia.DotNet.DataAccess.Interfaces;
using Aurelia.DotNet.DataAccess.Models;
using Aurelia.DotNet.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aurelia.DotNet.DataAccess
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountLogic _accountLogic;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountLogic accountLogic, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _accountLogic = accountLogic;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            _logger.LogInformation("Migrating database");
            await _context.Database.MigrateAsync().ConfigureAwait(false);
            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Seeding Data");

                await AddDefaultRoles();
                await CreateDefaultAdminUser();
                _context.Manufacturers.AddRange(new List<Manufacturer>
                {
                    new Manufacturer
                    {
                        Name = "Tesla",
                        Vehicles = new []{
                            new Vehicle
                            {
                                Model = "Model S",
                                Trim ="P100D",
                                Year = DateTime.Now.Year,
                                Price = 100_000,

                            },
                             new Vehicle
                            {
                                 Model = "Model X",
                                Trim ="P100D",
                                Year = DateTime.Now.Year,
                                Price = 120_000,

                            }
                        }

                    },
                    new Manufacturer
                    {
                        Name = "Ford",
                         Vehicles = new []{
                            new Vehicle
                            {
                                Model = "GT",
                                Year = DateTime.Now.Year,
                                Price = 250_000,

                            },
                             new Vehicle
                            {
                                 Model = "Mustang",
                                Trim ="GT500",
                                Year = DateTime.Now.Year,
                                Price = 70_000,

                            }
                        }
                    },
                    new Manufacturer
                    {
                        Name = "Chevy",
                         Vehicles = new []{
                            new Vehicle
                            {
                                Model = "Silverado",
                                Trim ="3500 HD",
                                Year = DateTime.Now.Year,
                                Price = 80_000,

                            },
                             new Vehicle
                            {
                                 Model = "Model X",
                                Trim ="P100D",
                                Year = DateTime.Now.Year,
                                Price = 120_000,

                            }
                        }
                    },
                    new Manufacturer
                    {
                        Name = "Honda",
                         Vehicles = new []{
                            new Vehicle
                            {
                                Model = "CR-V",
                                Trim ="Touring",
                                Year = DateTime.Now.Year,
                                Price = 40_000,

                            },
                             new Vehicle
                            {
                                 Model = "Pilot",
                                Trim ="Elite",
                                Year = DateTime.Now.Year,
                                Price = 50_000,

                            }
                        }
                    },
                    new Manufacturer
                    {
                        Name = "Toyota",
                         Vehicles = new []{
                            new Vehicle
                            {
                                Model = "Tundra",
                                Year = DateTime.Now.Year,
                                Price = 50_000,

                            },
                             new Vehicle
                            {
                                 Model = "Supra",
                                Trim ="S",
                                Year = DateTime.Now.Year,
                                Price = 180_000
                            },
                             new Vehicle
                            {
                                 Model = "Swagger Wagon",
                                Year = DateTime.Now.Year,
                                Price = 70_000
                            }
                        }
                    }

                });

                await _context.SaveChangesAsync();
                _logger.LogInformation("Finished Seeding Data");
            }
            _logger.LogInformation("Finished Migrating database");
        }

        public void Seed()
        {
            this.SeedAsync().Wait();
        }

        private async Task CreateDefaultAdminUser()
        {
            _logger.LogInformation("Generating admin account with user/pass admin/admin");
            User user = new User
            {
                UserName = "admin",
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.com",
                PhoneNumber = "45645465",
                EmailConfirmed = true,
                IsEnabled = true
            };
            await this._accountLogic.CreateUserAsync(user, "admin", Roles.SystemAdmin);

            _logger.LogInformation("Finished generating admin account");
        }

        private async Task AddDefaultRoles()
        {
            _logger.LogInformation("Adding System Admin role to account");
            var role = new Role(Roles.SystemAdmin, "Super User");
            var result = await this._accountLogic.CreateRoleAsync(role, Claims.Permission);
            _logger.LogInformation("Finished Adding System Admin role to account");
        }
    }
}
