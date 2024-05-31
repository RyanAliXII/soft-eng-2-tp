using EventManagementApp.Areas.Admin.Models;
using EventManagementApp.Data;
using EventManagementApp.Repositories;
using Microsoft.EntityFrameworkCore;

class UserRepository : IUserRepository
{
    private readonly DefaultDbContext _dbContext;
    public UserRepository(DefaultDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> Add(User user)
    {

        await _dbContext.SaveChangesAsync();
        return user;
    }
    public List<User> GetAll()
    {
        var users = _dbContext.User.ToList();
        return users;
    }
    public async Task<User> GetById(Guid id)
    {
        var user = await _dbContext.User.SingleOrDefaultAsync((u) => u.Id == id);
        return user;
    }
    public void Update(User driver)
    {

        _dbContext.SaveChanges();
    }
    public void Delete(User user)
    {

    }
    public async Task<User?> GetUserByEmail(string email)
    {
        email = email.ToLower();
        return await _dbContext.User.Include(u => u.LoginCredential).
        Where(u => u.LoginCredential.Email.ToLower().Equals(email)).
        FirstOrDefaultAsync();
    }

}

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetUserByEmail(string email);
}