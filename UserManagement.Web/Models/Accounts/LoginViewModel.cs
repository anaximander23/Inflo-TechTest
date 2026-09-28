using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email must be provided")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    public String? Email { get; set; }

    [Required(ErrorMessage = "Password must be provided")]
    [DataType(DataType.Password)]
    public String? Password { get; set; }

    public bool RememberMe { get; set; }

    public String? ReturnUrl { get; set; }
}