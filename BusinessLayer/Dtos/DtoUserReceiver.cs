using AppResoursces;
using Domains.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Dtos
{
    public partial class DtoUserReceiver : BaseDto
    {
 
        [Required]
        public Guid UserId { get; set; }


        [Required(ErrorMessageResourceName = "NameArRequired", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(100, MinimumLength = 3,
            ErrorMessageResourceName = "NameLength",
            ErrorMessageResourceType = typeof(Messages))]
        public string ReceiverName { get; set; } = null!;

        [Required(ErrorMessageResourceName = "EnterEmail", ErrorMessageResourceType = typeof(Messages))]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Messages))]
        public string Email { get; set; } = null!;

        [Required(ErrorMessageResourceName = "EnterPhone", ErrorMessageResourceType = typeof(Messages))]
        [StringLength(20, MinimumLength = 7)]
        public string Phone { get; set; } = null!;
        [Required]
        public Guid CityId { get; set; }

        [Required]
        public Guid CountryId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Address { get; set; } = null!;
        [StringLength(200)]
        public string? AddressDetails { get; set; }

        [StringLength(200)]
        public string? OtherAddress { get; set; }

        public bool IsDefault { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }
    }
}