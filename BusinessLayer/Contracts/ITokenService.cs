using BusinessLayer.Dtos;
using DataAccessLayer;
using DataAccessLayer.Identity;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Contracts
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(DTOUser user);
        string CreateRefreshToken();
    }
}
