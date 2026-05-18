using AutoMapper;
using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLayer.Services
{
    public class VwBaseService<T, Dto> : IVwBaseService<T, Dto> where T :class
    {
        private readonly IVwRepository<T> _vwRepository;   
        private readonly IMapper _mapper;   
        public VwBaseService(IVwRepository<T> vwRepository, IMapper mapper)
        {
            _vwRepository = vwRepository;
            _mapper = mapper;
        }
        public async Task<List<Dto>> GetAllAsync()
        {
            var entities = await _vwRepository.GetAllAsync();
            return _mapper.Map<List<T>, List<Dto>>(entities);  

        }

        public async Task<Dto?> GetByIdAsync(Guid id)
        {
            var entity = await _vwRepository.GetByIdAsync(id);
            return _mapper.Map<T, Dto>(entity);
        }

    }
}
