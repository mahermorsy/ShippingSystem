using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Contracts;



namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public class DeliveredService : IShipmentStatusHandler
    {
        IShipmentCommand _Ishipment;
        IShipmentStatus _IshipmentStatus;

        public DeliveredService(IShipmentCommand Ishipment, IShipmentStatus IshipmentStatus)
        {
            _Ishipment = Ishipment;
            _IshipmentStatus = IshipmentStatus;
        }
        public ShipmentStatusEnum statusEnum => ShipmentStatusEnum.Delivered;
        public async Task StatusHandler(DtoShipment Shipment)
        {
            await _IshipmentStatus.AddShipmentStatus(Shipment.Id, statusEnum, "Shipment delivered.");
            await _Ishipment.ChangeStatus(Shipment.Id, (int)statusEnum);
        }

    }
}
