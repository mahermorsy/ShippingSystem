using BusinessLayer.Dtos;
using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Config;

namespace BusinessLayer.Contracts
{
    public interface IUserService
    {
        Task<RegisterResponse> RegisterAsync(DTOUser UserRegister);
        Task<DTOUserResult> LoginAsync(DTOLoginUser UserLogin);
        Task LogoutAsync();
        Task<DTOUser>  GetUserbyIdAsync(Guid Userid);
        Task<DTOUser?>GetUserByNameAsync(string username);
        Task<IEnumerable<DTOUser>> GetAllUsersAsync();
        Guid GetLoggedInUser();
        Task<Claim[]> GetClaims(string Username);
    }
}
