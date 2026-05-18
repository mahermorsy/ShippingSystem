using AppResoursces;
using Domains.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BusinessLayer.Dtos;

public partial class DtoPaypalPayment 
{
    public string OrderId { get; set; } = null!;
    public decimal Amount { get; set; }

}
