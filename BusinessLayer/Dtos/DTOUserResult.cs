using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DTOUserResult
{
    public bool Sucess { get; set; }  
    public IEnumerable<string>? Errors { get; set; }    
}
