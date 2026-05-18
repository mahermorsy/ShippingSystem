using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile
{
    public class RateCalculator : IRateCalculator
    {
        public async Task<decimal> CalculateRate(DtoShipment dtoShipment)
        {
           return await Task.FromResult((decimal)(dtoShipment.Weight * 0.5 + dtoShipment.Length * 0.2 + dtoShipment.Width * 0.2 + dtoShipment.Height * 0.2));
        }
    }
}
