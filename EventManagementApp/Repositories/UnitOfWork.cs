using EventManagementApp.Data;

namespace EventManagementApp.Repositories;
public class UnitOfWork(DefaultDbContext dbContext) : IUnitOfWork
{
    private readonly DefaultDbContext _dbContext = dbContext;
    public IUserRepository UserRepository { get; private set; } = new UserRepository(dbContext);
    public IEventRepository EventRepository { get; private set; } = new EventRepository(dbContext);
    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    public IEventRepository EventRepository { get; }
    public Task Save();
}