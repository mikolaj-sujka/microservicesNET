﻿using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data;
public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
    }

    private static void SeedData(AppDbContext? context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("--> Applying Migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        if (!context.Platforms.Any())
        {
            Console.WriteLine("--> Seeding data...");
            context.Platforms.AddRange(
                               new Models.Platform { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                                              new Models.Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                                              new Models.Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                                          );
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}

