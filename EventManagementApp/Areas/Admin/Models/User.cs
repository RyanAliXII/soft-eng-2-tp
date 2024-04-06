using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementApp.Areas.Admin.Models;

public class User {
    public Guid Id { get; set; } = Guid.Empty;
    public string GivenName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt {get; set;}
  
    // public DateTime DeletedAt  {get; set;}
    public Guid LoginCredentialId { get; set; } = Guid.Empty;
    [ForeignKey("LoginCredentialId")]
    public LoginCredential LoginCredential {get; set; } = new LoginCredential();
}