using DataAccessLayer.RefreshToken;
using Domains.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;


namespace DataAccessLayer.Identity;

public partial class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public ICollection<TbRefreshToken> RefreshTokens { get; set; } = new List<TbRefreshToken>();
}
