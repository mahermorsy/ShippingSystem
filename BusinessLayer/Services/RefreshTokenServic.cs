using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Domains.Models;
using DataAccessLayer.RefreshToken;
using DataAccessLayer.Exceptions;

namespace BusinessLayer.Services
{
    public class RefreshTokenService : BaseService<TbRefreshToken, DTORefreashToken>, IRefreshToken
    {

        private readonly IGenericRepository<TbRefreshToken> _GenericRepo;
        private readonly IMapper _mapper;
        public RefreshTokenService(IGenericRepository<TbRefreshToken> GenericRepo,IMapper mapper, IUserService UserService) 
            : base(GenericRepo, mapper, UserService)
        {
            _GenericRepo = GenericRepo;
            _mapper = mapper;
        }   

        public async Task<DTORefreashToken> GetByToken(string Token)
        {
                var RefreshToken = await _GenericRepo.GetFirstOrDefault(x => x.Token == Token);
                return _mapper.Map<TbRefreshToken,DTORefreashToken>(RefreshToken);
        }

        public async Task< bool> RefreshTokenClearAdd(DTORefreashToken DtoToken)
        {
          var UserRefreshTokenLst = await _GenericRepo.GetList(x => x.UserId == DtoToken.UserId && x.CurrentState == 0);

            foreach (var Token in UserRefreshTokenLst)
            {
                await _GenericRepo.ChangeStatus(Token.Id,DtoToken.UserId,1); 
            }

            var RefreshToken = _mapper.Map<DTORefreashToken, TbRefreshToken>(DtoToken);
            await _GenericRepo.AddAsync(RefreshToken, DtoToken.UserId);      
            return true;
        }
       
    }
}
