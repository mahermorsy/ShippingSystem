using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Dtos
{
    public class CreatePaymentRequist
    {
        public List<DTOCartItems> CartItems { get; set; } = new List<DTOCartItems>();

        public decimal TotalAmount { get; set; } = 0;

    }
}
