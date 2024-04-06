using EventManagementApp.Data;

namespace EventManagementApp.Repositories;
public class UnitOfWork(DefaultDbContext dbContext) : IUnitOfWork {
    private readonly DefaultDbContext _dbContext = dbContext;
    public IUserRepository UserRepository {get; private set;} = new UserRepository(dbContext);
    public void Save(){
        _dbContext.SaveChanges();
    }
}
public interface IUnitOfWork {
    public void Save();
}