using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DTOUser : BaseDto
{
    [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Messages))]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = "FirstNameRequired", ErrorMessageResourceType = typeof(Messages))]
    public string? FirstName { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = "LastNameRequired", ErrorMessageResourceType = typeof(Messages))]
    public string? LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Messages))]
    public string? Email { get; set; } = string.Empty;

    public string? Role { get; set; } = string.Empty;

    [Phone(ErrorMessageResourceName = "PhoneNumberInvalid", ErrorMessageResourceType = typeof(Messages))]
    public string? PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Messages))]
    [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(Messages))]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Messages))]
    public string? ConfirmationPassword { get; set; }

    public string? ReturnURL { get; set; }
}
