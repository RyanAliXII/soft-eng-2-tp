namespace EventManagementApp.Areas.Admin.Models;

public class User {
    public Guid Id { get; set; } = Guid.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime CreatedAt {get; set;}
    public DateTime? DeletedAt  {get; set;}
    
    public string Email { get; set; } = string.Empty;
    public LoginCredential? LoginCredential {get; set; }
}