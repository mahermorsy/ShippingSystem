using BusinessLayer.Dtos;
using DataAccessLayer;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLayer.Contracts
{
    public interface IVwBaseService<T, Dto> where T : class
    {
        Task<List<Dto>> GetAllAsync();
        Task<Dto?> GetByIdAsync(Guid id);
    }
}
