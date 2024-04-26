
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace EventManagementApp.Areas.Admin.ViewModels;
public class NewEventViewModel
{
    [JsonPropertyName("name")]
    [StringLength(maximumLength: 100, ErrorMessage = "Name must not exceed 100 characters.")]
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    public List<NewActivityViewModel> Activities { get; set; } = [];
}