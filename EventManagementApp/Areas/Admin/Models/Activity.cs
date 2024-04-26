public class Activity
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

}
