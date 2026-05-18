using AppResoursces;
using DataAccessLayer.Identity;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DTORefreashToken : BaseDto
{
    public string Token { get; set; } = null!;

    public DateTime ExpiresOn { get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

}
