using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public class ApproveService : IShipmentStatusHandler
    {
        IShipmentCommand _Ishipment;
        IShipmentStatus _IshipmentStatus;
        public ApproveService(IShipmentCommand Ishipment, IShipmentStatus IshipmentStatus)
        {
            _Ishipment = Ishipment;
            _IshipmentStatus = IshipmentStatus;
        }
        public ShipmentStatusEnum statusEnum => ShipmentStatusEnum.Approved;
    
        public async Task StatusHandler(DtoShipment Shipment)
        {
            await _Ishipment.EditAsync(Shipment);
            await _Ishipment.ChangeStatus(Shipment.Id, (int)statusEnum);
            await _IshipmentStatus.AddShipmentStatus(Shipment.Id, statusEnum, "Shipment approved.");
        }
    }
}
