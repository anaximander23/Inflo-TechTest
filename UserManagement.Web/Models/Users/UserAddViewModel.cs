using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class UserAddViewModel
{
    [Required(ErrorMessage="Forename must be provided")]
    public String? Forename { get; set; }
    [Required(ErrorMessage="Surname must be provided")]
    public String? Surname { get; set; }

    [Required(ErrorMessage = "Date of birth must be provided")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Email must be provided")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    public String? Email { get; set; }

    public bool IsActive { get; set; }
}
