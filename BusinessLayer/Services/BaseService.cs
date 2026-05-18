using BusinessLayer.Contracts;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace BusinessLayer.Services
{
    public class BaseService<T, DTO> : IBaseService<T, DTO> where T : BaseTable
    {
        protected readonly IGenericRepository<T> _GenericRepo;
        protected private IMapper _mapper;
        protected readonly IUserService _UserService;
        protected readonly IUnitOfWork _UnitOfWork;
        public BaseService(IGenericRepository<T> GenericRepo, IMapper mapper, IUserService userService)
        {
            _GenericRepo = GenericRepo;
            _mapper = mapper;
            _UserService = userService;
        }
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _UnitOfWork = unitOfWork;
            _GenericRepo = _UnitOfWork.Repository<T>();
            _mapper = mapper;
            _UserService = userService;
        }
        public async Task<List<DTO>> GetAllAsync()
        {
            var entities = await _GenericRepo.GetAllAsync();
            return _mapper.Map<List<T>, List<DTO>>(entities);
        }
        public async Task<DTO> GetByIdAsync(Guid id)
        {
            var entity = await _GenericRepo.GetByIdAsync(id);
            return _mapper.Map<T, DTO>(entity);
        }
        public async Task<bool> AddAsync(DTO entity )
        {
            return await AddAsync(entity, _UserService.GetLoggedInUser());
        }
        public async Task<bool> AddAsync(DTO entity, Guid auditUserId)
        {
            var mappedEntity = _mapper.Map<DTO, T>(entity);
            return await _GenericRepo.AddAsync(mappedEntity, auditUserId);
        }
        public async Task<(bool Success, Guid EntityId)> AddAsyncWithID(DTO entity)
        {
            return await AddAsyncWithID(entity, _UserService.GetLoggedInUser());
        }
        public async Task<(bool Success, Guid EntityId)> AddAsyncWithID(DTO entity, Guid auditUserId)
        {
            var mappedEntity = _mapper.Map<DTO, T>(entity);
            return await _GenericRepo.AddAsyncGetID(mappedEntity, auditUserId);
        }
        public async Task<bool> ChangeStatus(Guid id, int Status = 1)
        {
            try
            {
                await _GenericRepo.ChangeStatus(id, _UserService.GetLoggedInUser(), Status);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Task UpdateAsync(DTO entity)
        {
            var mappedEntity = _mapper.Map<DTO, T>(entity);
            return _GenericRepo.UpdateAsync(mappedEntity, _UserService.GetLoggedInUser());
        }

    }
    
}
