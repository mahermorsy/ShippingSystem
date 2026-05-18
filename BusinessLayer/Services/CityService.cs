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
    public class CityService : BaseService<TbCity, DtoCity>, ICities
    {
        protected readonly IGenericRepository<TbCity> _repository;  
        protected readonly IVwRepository<VwCity> _vwRepository;
        public CityService(IGenericRepository<TbCity> GenericRepo, IMapper mapper, IUserService UserService, IVwRepository<VwCity> vwRepository) :
            base(GenericRepo, mapper, UserService)
        {
            _vwRepository = vwRepository;
        }

        public async Task<List<DtoCity>> GetByCountryId(Guid CountryId)
        {
            var city = await _repository.GetList(c => c.CountryId == CountryId);
            return _mapper.Map<List<TbCity>, List<DtoCity>>(city);

        }
    }
}
