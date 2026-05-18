using DataAccessLayer;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Domains.Models;
using DataAccessLayer.RefreshToken;

namespace BusinessLayer.Contracts
{
    public interface IRefreshToken : IBaseService<TbRefreshToken,DTORefreashToken>
    {
        Task<bool> RefreshTokenClearAdd(DTORefreashToken DtoToken);
        Task<DTORefreashToken> GetByToken(string Token);

    }
}
