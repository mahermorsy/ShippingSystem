using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DtoSetting : BaseDto
{
    [Range(0.01, double.MaxValue, ErrorMessageResourceName = "KiloMeterRateRange", ErrorMessageResourceType = typeof(Messages))]
    public double? KiloMeterRate { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessageResourceName = "KilooGramRateRange", ErrorMessageResourceType = typeof(Messages))]
    public double? KilooGramRate { get; set; }
}
