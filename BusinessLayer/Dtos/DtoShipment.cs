
using AppResoursces;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public class DtoShipment : BaseDto
    {
        [Required(ErrorMessageResourceName = "ShippingDateRequired", ErrorMessageResourceType = typeof(Messages))]
        public DateTime ShippingDate { get; set; }
        [Required(ErrorMessageResourceName = "DeliveryDateRequired", ErrorMessageResourceType = typeof(Messages))]
        public DateTime DeliveryDate { get; set; }
        [Required(ErrorMessageResourceName = "ShippingTypeRequired", ErrorMessageResourceType = typeof(Messages))]
        public Guid ShippingTypeId { get; set; }
        public Guid? ShippingPackagingId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public Guid? CarrierId { get; set; }
        public Guid? UserSubscriptionId { get; set; }
        public Guid? ReferenceId { get; set; }
        [Required(ErrorMessageResourceName = "WidthRequired", ErrorMessageResourceType = typeof(Messages))]
        [Range(typeof(double), "0.01", "500", ErrorMessageResourceName = "WidthRange", ErrorMessageResourceType = typeof(Messages))]
        public double Width { get; set; }
        [Required(ErrorMessageResourceName = "HeightRequired", ErrorMessageResourceType = typeof(Messages))]
        [Range(typeof(double), "0.01", "500", ErrorMessageResourceName = "HeightRange", ErrorMessageResourceType = typeof(Messages))]
        public double Height { get; set; }
        [Required(ErrorMessageResourceName = "LengthRequired", ErrorMessageResourceType = typeof(Messages))]
        [Range(typeof(double), "0.01", "500", ErrorMessageResourceName = "LengthRange", ErrorMessageResourceType = typeof(Messages))]
        public double Length { get; set; }
        [Required(ErrorMessageResourceName = "TotalWeightRequired", ErrorMessageResourceType = typeof(Messages))]
        [Range(typeof(double), "0.01", "1000", ErrorMessageResourceName = "TotalWeightRange", ErrorMessageResourceType = typeof(Messages))]
        public double Weight { get; set; }
        public double? TrackingNumber { get; set; }
        [Required(ErrorMessageResourceName = "PackageValueRequired", ErrorMessageResourceType = typeof(Messages))]
        [Range(typeof(decimal), "0.01", "500000", ErrorMessageResourceName = "PackageValueRange", ErrorMessageResourceType = typeof(Messages))]
        public decimal PackageValue { get; set; }
        public decimal ShippingRate { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DtoUserSender? Sender { get; set; }
        public DtoUserReceiver? Receiver { get; set; }
    }
}