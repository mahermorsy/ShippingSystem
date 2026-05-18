using AutoMapper;
using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using DataAccessLayer;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Interfaces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public class ShipmentStatusService : BaseService<TbShipmentStatus, DtoShipmentStatus>, IShipmentStatus
    {
        private readonly IUserService _UserService;

        public ShipmentStatusService(
            IGenericRepository<TbShipmentStatus> GenericRepo,
            IMapper mapper, IUserService UserService) :
            base(GenericRepo, mapper, UserService){

            _UserService = UserService;
       
        }
        public async Task AddShipmentStatus(Guid shipmentId, ShipmentStatusEnum status, string notes)
        {
            DtoShipmentStatus shipmentStatus = new DtoShipmentStatus
            {
                ShipmentId = shipmentId,
                CurrentState = (int)status,
                Notes = notes
            };
            await this.AddAsync(shipmentStatus, _UserService.GetLoggedInUser());
        }
    }
    
}
