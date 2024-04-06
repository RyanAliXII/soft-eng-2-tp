using EventManagementApp.Areas.Admin.Models;
using EventManagementApp.Data;
using EventManagementApp.Repositories;


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
 
  
}

public interface IUserRepository: IRepository<User>{


}