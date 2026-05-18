using AutoMapper;
using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLayer.Services
{
    public class VwCityService : VwBaseService<VwCity, VwCityDTO>, IVwCityService
    {
        private readonly IMapper _mapper;   
        private readonly IVwRepository<VwCity> _vwRepository;

        public VwCityService(IVwRepository<VwCity> vwRepository, IMapper mapper) : base(vwRepository, mapper) 
        {
            _mapper = mapper;
            _vwRepository = vwRepository;
        }

        public async Task<VwCityDTO?> GetFirstOrDefault(Guid Id)
        {
            var entity = await _vwRepository.GetFirstOrDefault(c => c.CountryId == Id);
            return _mapper.Map<VwCity, VwCityDTO>(entity);  
        }

        public async Task<List<VwCityDTO>> GetList(Guid Id)
        {
            var entities = await _vwRepository.GetList(c => c.CountryId == Id);
            return _mapper.Map<List<VwCity>, List<VwCityDTO>>(entities);
        }
    
    }
}
