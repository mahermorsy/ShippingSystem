namespace Domains.Models;

public partial class TbShippingPackaging : BaseTable
{
    public string? ShippingPackagingAname { get; set; }

    public string? ShippingPackagingEname { get; set; }

    public virtual ICollection<TbShipment> TbShipments { get; set; } = new List<TbShipment>();
}
