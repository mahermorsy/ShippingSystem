using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Dtos
{
    public class DTOCartItems
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }
}
