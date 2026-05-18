using DataAccessLayer;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Domains.Models;

namespace BusinessLayer.Contracts
{
    public interface ICities : IBaseService<TbCity,DtoCity>
    {
       Task<List<DtoCity>> GetByCountryId(Guid CountryId);
    }
}
