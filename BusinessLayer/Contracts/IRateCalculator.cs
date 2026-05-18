using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Contracts
{
    public interface IRateCalculator
    {
        public Task< decimal> CalculateRate(DtoShipment dtoShipment); 
    }
}
