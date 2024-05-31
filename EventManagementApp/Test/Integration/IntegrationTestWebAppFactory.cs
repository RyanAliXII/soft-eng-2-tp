using EventManagementApp.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Xunit;
namespace EventManagementApp.Tests.Integration;
public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server:2022-CU11-ubuntu-22.04").Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "test");
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DefaultDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<DefaultDbContext>(opts =>
            {
                opts.UseSqlServer(_dbContainer.GetConnectionString());
            });

        });

    }
    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }
    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}


