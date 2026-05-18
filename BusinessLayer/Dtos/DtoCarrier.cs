using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DtoCarrier: BaseDto
{
    [Required(ErrorMessageResourceName = "CarrierNameRequired", ErrorMessageResourceType = typeof(Messages))]
    public string CarrierName { get; set; } = null!;

}
