using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Data;
public class DefaultDbContext : DbContext {
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options):base(options){
        
    }
   
}