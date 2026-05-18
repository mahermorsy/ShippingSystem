namespace Domains.Models;

public partial class TbShipmentStatus : BaseTable
{
    public Guid? ShipmentId { get; set; }
    public string? Notes { get; set; }
    public virtual TbCarrier? Carrier { get; set; }
    public virtual TbShipment? TbShipments { get; set; }
}
