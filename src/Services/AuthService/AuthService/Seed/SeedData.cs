using System.Security.Claims;

using AuthService.Data;

using IdentityModel;

using IdentityServer7.EntityFramework.Storage.DbContexts;
using IdentityServer7.EntityFramework.Storage.Mappers;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using AuthService.Configuration;

namespace AuthService.Seed
{
    public static class SeedData
    {
        public static async Task EnsureSeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();

            await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            await context.Database.MigrateAsync();

            await CreateClients();

            await CreateScopes();

            await CreateApiResources();

            await CreateIdentityResources();

            await context.SaveChangesAsync();

            async Task CreateClients()
            {
                if (!await context.Clients.AnyAsync())
                {
                    foreach (var item in Config.Clients)
                    {
                        await context.Clients.AddAsync(item.ToEntity());
                    }
                }
            }

            async Task CreateScopes()
            {
                if (!await context.ApiScopes.AnyAsync())
                {
                    foreach (var item in Config.ApiScopes)
                    {
                        await context.ApiScopes.AddAsync(item.ToEntity());
                    }
                }
            }

            async Task CreateApiResources()
            {
                if (!await context.ApiScopes.AnyAsync())
                {
                    foreach (var item in Config.ApiResources)
                    {

                        await context.ApiResources.AddAsync(item.ToEntity());
                    }
                }
            }

            async Task CreateIdentityResources()
            {
                if (!await context.ApiResources.AnyAsync())
                {
                    foreach (var item in Config.IdentityResources)
                    {
                        await context.IdentityResources.AddAsync(item.ToEntity());
                    }
                }
            }
        }
    }
}
