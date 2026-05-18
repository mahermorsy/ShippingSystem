using AutoMapper;
using BusinessLayer.Contracts;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Domains.Models;

namespace BusinessLayer.Services
{
    public class UserSenderService : BaseService<TbUserSender,DtoUserSender>, IUserSender
    {
        public UserSenderService(IGenericRepository<TbUserSender> GenericRepo, IUnitOfWork unitOfWork, IMapper mapper, IUserService UserService) : base(unitOfWork, mapper, UserService)
        {

        }
    }
}
