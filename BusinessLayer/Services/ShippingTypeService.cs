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
    public class ShippingTypeService : BaseService<TbShippingType,DtoShippingType>, IShippingTypes
    {
        public ShippingTypeService(IGenericRepository<TbShippingType> GenericRepo, IMapper mapper, IUserService UserService) : base(GenericRepo, mapper, UserService)
        {

        }
    }
}
