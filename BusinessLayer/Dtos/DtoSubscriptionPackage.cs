using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DtoSubscriptionPackage : BaseDto
{
    [Required(ErrorMessageResourceName = "PackageNameRequired", ErrorMessageResourceType = typeof(Messages))]
    public string PackageName { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessageResourceName = "ShippimentCountRange", ErrorMessageResourceType = typeof(Messages))]
    public int ShippimentCount { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessageResourceName = "NumberOfKiloMetersRange", ErrorMessageResourceType = typeof(Messages))]
    public double NumberOfKiloMeters { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessageResourceName = "TotalWeightRange", ErrorMessageResourceType = typeof(Messages))]
    public double TotalWeight { get; set; }

    public virtual ICollection<DtoUserSubscription> TbUserSubscriptions { get; set; } = new List<DtoUserSubscription>();
}
