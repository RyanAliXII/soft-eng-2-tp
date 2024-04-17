
using System.ComponentModel.DataAnnotations;

public class Activity {
   [StringLength(maximumLength:100, ErrorMessage = "Name must not exceed 100 characters.")]
   [Required]
   public string Name {get; set;} = string.Empty;
   [Required]
   public TimeSpan StartTime {get; set;}

   [Required]
   public TimeSpan EndTime {get; set;}
}

public class ActivityModel: Activity {
    public Guid Id {get; set;} = Guid.Empty;
}
public class NewActivityViewModel: Activity {};