using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Dtos
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public int CurrentState { get; set; }
    }
}
