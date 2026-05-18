using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile
{
    public class TrackingNumber : ITrackingNumber
    {
        public double CreateTrackingNumber(DtoShipment shipmentDto)
        {
            // التحقق من صحة البيانات
            if (shipmentDto == null)
                throw new ArgumentNullException(nameof(shipmentDto));

            if (shipmentDto.Weight <= 0)
                throw new ArgumentException("Weight must be greater than 0", nameof(shipmentDto.Weight));

            if (shipmentDto.Length <= 0 || shipmentDto.Width <= 0 || shipmentDto.Height <= 0)
                throw new ArgumentException("Dimensions must be greater than 0");

            // حساب الـ Tracking Number (الحجم / الوزن)
            double volume = shipmentDto.Length * shipmentDto.Width * shipmentDto.Height;
            double trackingFactor = Math.Round(volume / shipmentDto.Weight, 2);

            return trackingFactor;
        }
    }
}
