namespace EventManagementApp.Areas.Admin.Models;
public class LoginCredential {
    public Guid Id { get; set; } = Guid.Empty;
    public string Email {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public bool IsRoot {get; set;} = false;
}