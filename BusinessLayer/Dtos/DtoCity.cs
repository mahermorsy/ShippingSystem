using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BusinessLayer.Dtos;

public partial class DtoCity: BaseDto
{

    [Required(ErrorMessageResourceName = "NameArRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Messages))]
    public string? CityAname { get; set; }

    [Required(ErrorMessageResourceName = "NameEnRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Messages))]
    public string? CityEname { get; set; }

    [Required(ErrorMessageResourceName = "CountryRequired", ErrorMessageResourceType = typeof(Messages))]
    public Guid? CountryId { get; set; }
    public virtual DtoCounty? Country { get; set; } = null!;
    public virtual ICollection<DtoUserReceiver> TbUserReceivers { get; set; } = new List<DtoUserReceiver>();
    public virtual ICollection<DtoUserSender> TbUserSenders { get; set; } = new List<DtoUserSender>();
}
