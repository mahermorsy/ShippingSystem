using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DtoShippingType : BaseDto
{
    [Required(ErrorMessageResourceName = "NameArRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Messages))]
    public string? ShippingTypeAname { get; set; }

    [Required(ErrorMessageResourceName = "NameArRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100,MinimumLength =5, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Messages))]
    public string? ShippingTypeEname { get; set; }

    [Required(ErrorMessageResourceName = "FactorIsRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [Range(0.25,10, ErrorMessageResourceName = "FactorRange", ErrorMessageResourceType = typeof(Messages))]
    public double? ShippingFactor { get; set; }

    public virtual ICollection<DtoShipment> TbShippments { get; set; } = new List<DtoShipment>();
}
