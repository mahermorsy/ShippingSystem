using Domains.Models;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Dtos;

public partial class DtoShipmentStatus : BaseDto
{
    public Guid? ShipmentId { get; set; }
    public string? Notes { get; set; }

}
