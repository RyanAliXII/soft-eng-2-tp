using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;
using EventManagementApp.Areas.Admin.ViewModels;

public class Event
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    public List<Activity> Activities { get; set; } = [];
    public Event() { }
    public Event(NewEventViewModel vm)
    {
        Name = vm.Name;
        Date = vm.Date;
        Activities = vm.Activities.Select(a => new Activity(a)).ToList();
    }
}
