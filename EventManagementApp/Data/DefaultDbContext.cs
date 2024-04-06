using EventManagementApp.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Data;
public class DefaultDbContext : DbContext {

    public DefaultDbContext(DbContextOptions<DefaultDbContext> options):base(options){}
    public DbSet<User> User {get; set;}
    public DbSet<LoginCredential> LoginCredential {get; set;}
}