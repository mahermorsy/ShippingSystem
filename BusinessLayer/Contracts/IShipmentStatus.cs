using BusinessLayer.Dtos;
using BusinessLayer.Services.ShipmentFile.ManageStatus;
using DataAccessLayer;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Contracts
{
    public interface IShipmentStatus : IBaseService<TbShipmentStatus, DtoShipmentStatus>
    {
        Task AddShipmentStatus(Guid shipmentId, ShipmentStatusEnum status, string notes);
    }
   
}
