using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DtoPaymentMethod : BaseDto
{
    [StringLength(100, MinimumLength = 4, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Messages))]
    public string? MethdAname { get; set; }

    [StringLength(100, MinimumLength = 4, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Messages))]
    public string? MethodEname { get; set; }

    [Range(0, 100, ErrorMessageResourceName = "CommissionRange", ErrorMessageResourceType = typeof(Messages))]
    public double? Commission { get; set; }

    public virtual ICollection<DtoShipment> TbShippments { get; set; } = new List<DtoShipment>();
}
