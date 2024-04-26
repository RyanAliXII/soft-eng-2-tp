public class Event
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<Activity> Activities { get; set; } = [];
}