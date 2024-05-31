using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace EventManagementApp.Areas.Admin.ViewModels;

public class EventViewModel
{
    [JsonPropertyName("name")]
    [StringLength(maximumLength: 100, ErrorMessage = "Name must not exceed 100 characters.")]
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

}
public class NewEventViewModel : EventViewModel
{
    public List<NewActivityViewModel> Activities { get; set; } = [];
}


public class EditEventViewModel : EventViewModel
{
    public Guid Id { get; set; }
    public List<EditActivityViewModel> Activities { get; set; } = [];
}