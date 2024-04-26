using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class NewActivityViewModel
{
    [JsonPropertyName("name")]
    [StringLength(maximumLength: 100, ErrorMessage = "Name must not exceed 100 characters.")]
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("startTime")]
    [Required(ErrorMessage = "Start time is required.")]
    [DataType(DataType.Time)]

    public TimeOnly StartTime { get; set; }

    [JsonPropertyName("endTime")]
    [Required(ErrorMessage = "End time is required.")]
    [DataType(DataType.Time)]
    public TimeOnly EndTime { get; set; }
    public int Index = 0;

}