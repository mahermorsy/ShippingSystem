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
    public class UserSubscriptionService : BaseService<TbUserSubscription,DtoUserSubscription>, IUserSubscription
    {
        public UserSubscriptionService(IGenericRepository<TbUserSubscription> GenericRepo, IMapper mapper, IUserService UserService) : base(GenericRepo, mapper, UserService)
        {

        }
    }
}
