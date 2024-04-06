using EventManagementApp.Areas.Admin.Models;
using EventManagementApp.Data;
using EventManagementApp.Repositories;
using Microsoft.EntityFrameworkCore;

class UserRepository: IUserRepository {
    private readonly DefaultDbContext _dbContext;
    public UserRepository(DefaultDbContext dbContext){
        _dbContext = dbContext;
    } 
    public void Add(User driver){
        
        _dbContext.SaveChanges();
    }
    public List<User> GetAll(){
        return new List<User>();
    }
    public User GetById(Guid id){
        return new User();
    }
    public void Update(User driver){

      _dbContext.SaveChanges();
    }
    public void Delete(User user){
        
    }
    public async Task<User?> GetUserByEmail(string email){
       email = email.ToLower();
       return await _dbContext.User.Include(u=> u.LoginCredential).
       Where(u=> u.LoginCredential.Email.ToLower().Equals(email)).
       FirstOrDefaultAsync();
    }
  
}

public interface IUserRepository: IRepository<User>{
    public  Task<User?> GetUserByEmail(string email);
}