using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Contracts;

namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public class ReturnedService : IShipmentStatusHandler
    {
        IShipmentCommand _Ishipment;
        IShipmentStatus _IshipmentStatus;
        public ReturnedService(IShipmentCommand Ishipment, IShipmentStatus IshipmentStatus)
        {
            _Ishipment = Ishipment;
            _IshipmentStatus = IshipmentStatus;
        }
        public ShipmentStatusEnum statusEnum => ShipmentStatusEnum.Returned;
        public async Task StatusHandler(DtoShipment Shipment)
        {
            await _IshipmentStatus.AddShipmentStatus(Shipment.Id, statusEnum, "Shipment returned.");
            await _Ishipment.ChangeStatus(Shipment.Id, (int)statusEnum);
        }
    }
}
