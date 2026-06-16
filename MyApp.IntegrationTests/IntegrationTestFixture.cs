using Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace MyApp.Integration.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
public class DbIntegrationTestFixture: IAsyncLifetime
{
    public required MsSqlContainer DbContainer { get; set; }
    public HttpClient? WebAppTestHttpClient { get; set; }
    
    public WebApplicationFactory<Program> WebAppFactory { get; set; }
    
    public async Task InitializeAsync()
    {
        var msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04").Build();
        await msSqlContainer.StartAsync();
        DbContainer =  msSqlContainer;

        WebAppFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices((services) =>
                {
                    services.RemoveAll<ProductDbContext>();
                    services.RemoveAll<DbContextOptions>();
                    services.AddDbContext<ProductDbContext>(options =>
                        options.UseSqlServer(DbContainer.GetConnectionString())
                    );
                    
                });
    
            });
    
        WebAppTestHttpClient = WebAppFactory.CreateClient();
    }


    public async Task DisposeAsync()
    {
        await DbContainer.DisposeAsync();
    }
}