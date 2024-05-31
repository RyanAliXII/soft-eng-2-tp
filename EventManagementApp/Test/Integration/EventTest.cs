using EventManagementApp.Areas.Admin.ViewModels;
using EventManagementApp.Data;
using EventManagementApp.Data.Seed;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
namespace EventManagementApp.Tests.Integration;
public class EventTests : BaseIntegrationTest
{
    private readonly HttpClient _httpClient;
    private readonly ITestOutputHelper _output;
    private readonly IntegrationTestWebAppFactory _factory;
    public EventTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true,
        });

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
        dbContext.Database.Migrate();
        RootUserSeed.Initialize(scope.ServiceProvider, new RootUser
        {
            Email = "juanmigueldc@gmail.com",
            GivenName = "Juan",
            MiddleName = "Miguel",
            Surname = "Dela Cruz",
            Password = "password"
        });
        var signInTask = AuthProvider.SignIn(_httpClient, new LoginViewModel
        {
            Email = "juanmigueldc@gmail.com",
            Password = "password",
        });
        signInTask.Wait();
    }
    [Fact]
    public async Task Index_ReturnEventsPage()
    {
        var response = await _httpClient.GetAsync("/Admin/Event");
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
    }

}