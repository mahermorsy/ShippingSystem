using Domains.Models;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Dtos;

public partial class DtoCreateShipment : BaseDto
{
    public DateTime ShippingDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid ShippingTypeId { get; set; }
    public Guid? ShippingPackagingId { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public double Length { get; set; }
    public decimal PackageValue { get; set; }
    public decimal ShippingRate { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public Guid? UserSubscriptionId { get; set; }
    public string? TrackingNumber { get; set; }
    public Guid? ReferenceId { get; set; }
    public string ReceiverName { get; set; } = null!;
    public string ReceiverEmail { get; set; } = null!;
    public string ReceiverPhone { get; set; } = null!;
    public Guid ReceiverCityId { get; set; }
    public Guid ReceiverCountryId { get; set; }
    public string ReceiverAddress { get; set; } = null!;
    public string? ReceiverAddressDetails { get; set; }
    public string? ReceiverOtherAddress { get; set; }
    public string? ReceiverPostalCode { get; set; }

        
    public Guid UserId { get; set; }
    public string SenderName { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
    public string SenderPhone { get; set; } = null!;
    public string? SenderContact { get; set; }
    public Guid SenderCityId { get; set; }
    public Guid SenderCountryId { get; set; }
    public string SenderAddress { get; set; } = null!;
    public string? SenderAddressDetails { get; set; }
    public string? SenderOtherAddress { get; set; }
    public string? SenderPostalCode { get; set; }
}
