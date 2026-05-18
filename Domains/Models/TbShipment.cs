namespace Domains.Models;

public partial class TbShipment : BaseTable
{
    public DateTime ShippingDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid ShippingTypeId { get; set; }
    public Guid? ShippingPackagingId { get; set; }
    public Guid? CarrierId { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }
    public decimal PackageValue { get; set; }
    public decimal ShippingRate { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public Guid? UserSubscriptionId { get; set; }
    public double? TrackingNumber { get; set; }
    public Guid? ReferenceId { get; set; }
    public virtual TbUserSender Sender { get; set; } = null!;
    public virtual TbUserReceiver Receiver { get; set; } = null!;
    public virtual TbShippingType ShippingType { get; set; } = null!;
    public virtual TbShippingPackaging? ShippingPackaging { get; set; }
    public virtual TbPaymentMethod? PaymentMethod { get; set; }
    public virtual TbCarrier? Carrier { get; set; }
    public virtual ICollection<TbShipmentStatus> TbShipmentStatuses { get; set; } = new List<TbShipmentStatus>();
}