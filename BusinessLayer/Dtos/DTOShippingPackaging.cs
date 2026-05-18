using AppResoursces;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos;

public partial class DTOShippingPackaging : BaseDto
{
    [Required(
        ErrorMessageResourceType = typeof(Messages),
        ErrorMessageResourceName = "ShippingPackagingAnameRequired")]
    [MaxLength(200,
        ErrorMessageResourceType = typeof(Messages),
        ErrorMessageResourceName = "ShippingPackagingAnameMax")]
    public string? ShippingPackagingAname { get; set; }


    [Required(
        ErrorMessageResourceType = typeof(Messages),
        ErrorMessageResourceName = "ShippingPackagingEnameRequired")]
    [MaxLength(200,
        ErrorMessageResourceType = typeof(Messages),
        ErrorMessageResourceName = "ShippingPackagingEnameMax")]
    public string? ShippingPackagingEname { get; set; }
}
