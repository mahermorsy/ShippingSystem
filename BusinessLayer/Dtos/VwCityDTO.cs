using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BusinessLayer.Dtos;

public partial class VwCityDTO 
{
    public Guid CityId { get; set; }
    public string? CityNameAr { get; set; }
    public string? CityNameEn { get; set; }
    public Guid CountryId { get; set; }
    public string? CountryNameAr { get; set; }
    public string? CountryNameEn{ get; set; }

}