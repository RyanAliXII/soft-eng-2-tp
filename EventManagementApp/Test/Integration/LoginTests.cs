using EventManagementApp.Areas.Admin.ViewModels;
using EventManagementApp.Data;
using EventManagementApp.Data.Seed;
using EventManagementApp.Tests.Integration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

public class LoginTests : BaseIntegrationTest
{
    private readonly HttpClient _httpClient;
    private readonly ITestOutputHelper _output;
    private readonly IntegrationTestWebAppFactory _factory;
    public LoginTests(IntegrationTestWebAppFactory factory, ITestOutputHelper output)
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

    }
    [Fact]
    public async Task Login_ShouldFailIfEmailIsNotProvided()
    {
        var response = await AuthProvider.SignIn(_httpClient, new LoginViewModel
        {
            Email = "",
            Password = "password"
        });

        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }
    [Fact]
    public async Task Login_ShouldFailIfPasswordIsNotProvided()
    {
        var response = await AuthProvider.SignIn(_httpClient, new LoginViewModel
        {
            Email = "juanmigueldc@gmail.com",
            Password = ""
        });

        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldFailIfCredentialsAreWrong()
    {
        var wrongEmailResponse = await AuthProvider.SignIn(_httpClient, new LoginViewModel
        {
            Email = "juanmigueldc@gmail.com",
            Password = "pass"
        });

        Assert.Equal(StatusCodes.Status400BadRequest, (int)wrongEmailResponse.StatusCode);

        var wrongPasswordResponse = await AuthProvider.SignIn(_httpClient, new LoginViewModel
        {
            Email = "juanmigueldc@gmail.com",
            Password = "pass"
        });

        Assert.Equal(StatusCodes.Status400BadRequest, (int)wrongPasswordResponse.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldBeSuccess()
    {
        var loginResponse = await AuthProvider.SignIn(_httpClient, new LoginViewModel
        {
            Email = "juanmigueldc@gmail.com",
            Password = "password"
        });
        Assert.Equal(StatusCodes.Status302Found, (int)loginResponse.StatusCode);
        var dashboardResponse = await _httpClient.GetAsync("/Admin/Event");
        Assert.Equal(StatusCodes.Status200OK, (int)dashboardResponse.StatusCode);
    }
}