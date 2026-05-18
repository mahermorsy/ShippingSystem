using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DTOLoginUser : BaseDto
{
    [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(Messages))]
    public string UserName { get; set; } = string.Empty;


    [Required(ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessageResourceType = typeof(Messages))]
    [Compare("Password", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Messages))]
    public string Password { get; set; } = string.Empty;

    public string? ReturnURL { get; set; }
}
