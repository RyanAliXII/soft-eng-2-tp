using System.ComponentModel.DataAnnotations.Schema;

public class Activity
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.Empty;
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid EventId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public Activity() { }
    public Activity(NewActivityViewModel vm)
    {
        Name = vm.Name;
        StartTime = vm.StartTime;
        EndTime = vm.EndTime;
    }

}
