using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Contracts
{
    public interface ITrackingNumber
    {
        public double CreateTrackingNumber(DtoShipment shipmentDto);
    }
}
