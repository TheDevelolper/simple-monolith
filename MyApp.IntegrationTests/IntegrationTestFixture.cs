using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.MsSql;

namespace MyApp.Integration.Tests;

public class DbIntegrationTestFixture: IAsyncLifetime
{
    public required MsSqlContainer DbContainer { get; set; }
    public HttpClient WebAppTestHttpClient { get; set; }
    
    public async Task InitializeAsync()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        
        var msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04").Build();
        await msSqlContainer.StartAsync();
        DbContainer =  msSqlContainer;
        
        webAppFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.Properties.Add("ConnectionString", DbContainer.GetConnectionString());
            });
        });
        
        WebAppTestHttpClient = webAppFactory.CreateClient();
    }



    public async Task DisposeAsync()
    {
        await DbContainer.DisposeAsync();
    }
}