using DataAccessLayer;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Domains.Models;

namespace BusinessLayer.Contracts
{
    public interface ISettings : IBaseService<TbSetting,DtoSetting>
    {
 
    }
}
