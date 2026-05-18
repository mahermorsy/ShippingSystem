using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public interface IShipmentStatusHandler
    {
        public ShipmentStatusEnum statusEnum { get; }
        Task StatusHandler(DtoShipment Shipment);
    }
}
