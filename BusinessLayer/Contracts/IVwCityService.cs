using BusinessLayer.Dtos;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLayer.Contracts
{
    public interface IVwCityService : IVwBaseService<VwCity, VwCityDTO>   
    {
        Task<VwCityDTO?> GetFirstOrDefault(Guid Id);
        Task<List<VwCityDTO>> GetList(Guid Id);
    }
}
