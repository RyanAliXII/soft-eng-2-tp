using System.Runtime.CompilerServices;
using EventManagementApp.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Data.Seed;

public static class RootUserSeeder
{
     public async static void Initialize(IServiceProvider serviceProvider, IConfiguration config)
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
        Include(u=> u.LoginCredential).
        Where(u=> u.LoginCredential.IsRoot == true).
        FirstOrDefaultAsync();
        
        if(rootUser != null){
            return;
        }
        var user = new User(){
            GivenName = givenName,
            MiddleName = middleName,
            Surname = surname,
            LoginCredential = new LoginCredential(){
                Email = email,
                Password =  password,
                IsRoot = true,
            }
        };
        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
    }
}
