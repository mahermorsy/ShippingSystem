namespace Domains.Models;

public partial class TbCarrier : BaseTable
{
    public string CarrierName { get; set; } = null!;

    public virtual ICollection<TbShipment> Shipments { get; set; } = new List<TbShipment>();
}
