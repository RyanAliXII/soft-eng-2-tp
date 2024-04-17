using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage;

class Event {
    [StringLength(maximumLength:100, ErrorMessage = "Name must not exceed 100 characters.")]
    [Required]
    public string Name {get; set;} = string.Empty;
    [Required]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    public DateTime Date {get; set;}
}
class EventModel:Event{
    public Guid Id {get; set;} = Guid.Empty;
    public List<Activity> Activities {get; set;} = [];
}

class NewEventViewModel: Event{
    public List<NewActivityViewModel> Activities {get; set;} = [];
}