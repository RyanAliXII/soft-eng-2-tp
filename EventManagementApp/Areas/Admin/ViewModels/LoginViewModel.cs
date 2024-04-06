
using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Areas.Admin.ViewModels;
public class LoginViewModel {
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Value should be a valid email address.")]
    public string Email {get; set;} = string.Empty;
    [Required(ErrorMessage = "Password is required.")]
    public string Password {get; set;} = string.Empty;
}