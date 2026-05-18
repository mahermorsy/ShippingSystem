using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public interface IShipmentStatusHandlerFactory
    {
        IShipmentStatusHandler GetStatusHandler(ShipmentStatusEnum statusEnum);
    }
}
