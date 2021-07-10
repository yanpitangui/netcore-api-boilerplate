using System;
using System.Collections.Generic;
using System.Linq;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BC = BCrypt.Net.BCrypt;

namespace Boilerplate.Api.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            db.Heroes.RemoveRange(db.Heroes);
            db.Heroes.AddRange(GetSeedingHeroes());
            db.Users.AddRange(GetSeedingUsers());
            db.SaveChanges();
        }

        public static User[] GetSeedingUsers()
        {
            return new User[]
            {
                new User()
                {
                    Id = new Guid("2e3b7a21-f06e-4c47-b28a-89bdaa3d2a37"),
                    Password = BC.HashPassword("testpassword123"),
                    Email = "admin@boilerplate.com",
                    Role = "Admin"
                },
                new User()
                {
                    Id = new Guid("c68acd7b-9054-4dc3-b536-17a1b81fa7a3"),
                    Password = BC.HashPassword("testpassword123"),
                    Email = "user@boilerplate.com",
                    Role = "User"
                }
            };
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.Heroes.RemoveRange(db.Heroes.ToList());
            db.Users.RemoveRange(db.Users.ToList());
            InitializeDbForTests(db);
        }

        public static List<Hero> GetSeedingHeroes()
        {
            return new()
            {
                new(){ Id = new Guid("824a7a65-b769-4b70-bccb-91f880b6ddf1"), Name = "Corban Best", HeroType = HeroType.ProHero },
                new() { Id = new Guid("b426070e-ccb3-42e6-8fb4-ef6aa5a62cc4"), Name = "Priya Hull", HeroType = HeroType.Student },
                new() { Id = new Guid("634769f7-a7b8-4146-9cb2-ff2dd90e886b"), Name = "Harrison Vu", HeroType = HeroType.Teacher }
            };
        }

        public static WebApplicationFactory<Startup> BuildApplicationFactory(this WebApplicationFactory<Startup> factory)
        {
            var connectionString = $"Data Source={Guid.NewGuid()}.db";
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlite(connectionString);
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ApplicationDbContext>();
                    context.Database.EnsureCreated();
                    InitializeDbForTests(context);
                });
            });
        }


        public static WebApplicationFactory<Startup> RebuildDb(this WebApplicationFactory<Startup> factory)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices
                        .GetRequiredService<ApplicationDbContext>();
                    ReinitializeDbForTests(db);
                });
            });
        }
    }
}
