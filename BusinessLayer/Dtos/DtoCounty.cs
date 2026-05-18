using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppResoursces;

namespace BusinessLayer.Dtos;

public partial class DtoCounty : BaseDto
{
    [Required(ErrorMessageResourceName = "NameArRequired", ErrorMessageResourceType =typeof(Messages),AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Messages))]
    public string? CountryAname { get; set; }

    [Required(ErrorMessageResourceName = "NameEnRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Messages))]
    public string? CountryEname { get; set; }

    public virtual ICollection<DtoCity> TbCities { get; set; } = new List<DtoCity>();
}
