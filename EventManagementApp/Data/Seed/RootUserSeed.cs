using EventManagementApp.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Data.Seed;

public static class RootUserSeed
{
    public async static void InitializeAsync(IServiceProvider serviceProvider, IConfiguration config)
    {
        using var context = new DefaultDbContext(
          serviceProvider.GetRequiredService<
              DbContextOptions<DefaultDbContext>>());

        var section = config.GetSection("RootUser") ?? throw new Exception("App settings or env has no RootUser section");
        var email = section.GetValue<string>("email");
        var password = section.GetValue<string>("password");
        var givenName = section.GetValue<string>("GivenName");
        var middleName = section.GetValue<string>("MiddleName");
        var surname = section.GetValue<string>("Surname");
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)
        || string.IsNullOrEmpty(givenName) || string.IsNullOrEmpty(middleName)
        || string.IsNullOrEmpty(surname)
        )
        {
            throw new Exception("Root user's Email, Password, GivenName, MiddleName and Surname should be defined in appsettings or env");
        }
        var rootUser = await context.User.
        Include(u => u.LoginCredential).
        Where(u => u.LoginCredential.IsRoot == true).
        FirstOrDefaultAsync();

        if (rootUser != null)
        {
            return;
        }
        var user = new User()
        {
            GivenName = givenName,
            MiddleName = middleName,
            Surname = surname,
            LoginCredential = new LoginCredential()
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 15),
                IsRoot = true,
            }
        };
        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
    }
    public async static void InitializeAsync(IServiceProvider serviceProvider, RootUser userArgs)
    {
        using var context = new DefaultDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<DefaultDbContext>>());
        if (string.IsNullOrEmpty(userArgs.Email) || string.IsNullOrEmpty(userArgs.Password)
        || string.IsNullOrEmpty(userArgs.GivenName) || string.IsNullOrEmpty(userArgs.MiddleName)
        || string.IsNullOrEmpty(userArgs.Surname)
        )
        {
            throw new Exception("Root user's Email, Password, GivenName, MiddleName and Surname should be defined.");
        }
        var rootUser = await context.User.
        Include(u => u.LoginCredential).
        Where(u => u.LoginCredential.IsRoot == true).
        FirstOrDefaultAsync();

        if (rootUser != null)
        {
            return;
        }
        var user = new User()
        {
            GivenName = userArgs.GivenName,
            MiddleName = userArgs.MiddleName,
            Surname = userArgs.Surname,
            LoginCredential = new LoginCredential()
            {
                Email = userArgs.Email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(userArgs.Password, 15),
                IsRoot = true,
            }
        };
        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public static void Initialize(IServiceProvider serviceProvider, RootUser userArgs)
    {
        using var context = new DefaultDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<DefaultDbContext>>());
        if (string.IsNullOrEmpty(userArgs.Email) || string.IsNullOrEmpty(userArgs.Password)
        || string.IsNullOrEmpty(userArgs.GivenName) || string.IsNullOrEmpty(userArgs.MiddleName)
        || string.IsNullOrEmpty(userArgs.Surname)
        )
        {
            throw new Exception("Root user's Email, Password, GivenName, MiddleName and Surname should be defined.");
        }
        var rootUser = context.User.
        Include(u => u.LoginCredential).
        Where(u => u.LoginCredential.IsRoot == true).
        FirstOrDefault();

        if (rootUser != null)
        {
            return;
        }
        var user = new User()
        {
            GivenName = userArgs.GivenName,
            MiddleName = userArgs.MiddleName,
            Surname = userArgs.Surname,
            LoginCredential = new LoginCredential()
            {
                Email = userArgs.Email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(userArgs.Password, 15),
                IsRoot = true,
            }
        };
        context.User.Add(user);
        context.SaveChanges();
    }

    public static void Initialize(IServiceProvider serviceProvider, IConfiguration config)
    {
        using var context = new DefaultDbContext(
          serviceProvider.GetRequiredService<
              DbContextOptions<DefaultDbContext>>());

        var section = config.GetSection("RootUser") ?? throw new Exception("App settings or env has no RootUser section");
        var email = section.GetValue<string>("email");
        var password = section.GetValue<string>("password");
        var givenName = section.GetValue<string>("GivenName");
        var middleName = section.GetValue<string>("MiddleName");
        var surname = section.GetValue<string>("Surname");
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)
        || string.IsNullOrEmpty(givenName) || string.IsNullOrEmpty(middleName)
        || string.IsNullOrEmpty(surname)
        )
        {
            throw new Exception("Root user's Email, Password, GivenName, MiddleName and Surname should be defined in appsettings or env");
        }
        var rootUser = context.User.
        Include(u => u.LoginCredential).
        Where(u => u.LoginCredential.IsRoot == true).
        FirstOrDefault();

        if (rootUser != null)
        {
            return;
        }
        var user = new User()
        {
            GivenName = givenName,
            MiddleName = middleName,
            Surname = surname,
            LoginCredential = new LoginCredential()
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 15),
                IsRoot = true,
            }
        };
        context.User.Add(user);
        context.SaveChanges();
    }
}

public class RootUser
{
    public string GivenName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
