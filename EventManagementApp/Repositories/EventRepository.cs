using EventManagementApp.Data;
using Microsoft.EntityFrameworkCore;
namespace EventManagementApp.Repositories;
public class EventRepository : IEventRepository
{
    private readonly DefaultDbContext _dbContext;

    public EventRepository(DefaultDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public List<Event> GetAll()
    {
        return [];
    }
    public Event GetById(Guid Id)
    {
        return new Event();
    }

    public async Task<Event> Add(Event e)
    {
        await _dbContext.Event.AddAsync(e);
        return e;
    }
    public void Update(Event e)
    {

    }
    public void Delete(Event e)
    {

    }
    public async Task<bool> IsDateExists(DateTime date)
    {
        var e = await _dbContext.Event.SingleOrDefaultAsync(e => e.Date.Equals(date));
        return e != null;
    }
    public async Task<List<Event>> GetEventsByDateRange(DateTime? start, DateTime? end)
    {
        var events = await _dbContext.Event.Include(e => e.Activities).
        Where(e => e.Date >= start && e.Date <= end).ToListAsync();
        return events;
    }
}

public interface IEventRepository : IRepository<Event>
{
    public Task<bool> IsDateExists(DateTime date);
    public Task<List<Event>> GetEventsByDateRange(DateTime? start, DateTime? end);
}