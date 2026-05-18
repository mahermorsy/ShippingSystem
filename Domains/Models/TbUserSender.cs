namespace Domains.Models;

public partial class TbUserSender : BaseTable
{
    public Guid UserId { get; set; }

    public string SenderName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Contact { get; set; }
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public string Address { get; set; } = null!;
    public string? AddressDetails { get; set; }
    public string? OtherAddress { get; set; }
    public bool IsDefault { get; set; }
    public string? PostalCode { get; set; }

    public virtual TbCity City { get; set; } = null!;
    public virtual TbCountry Country { get; set; } = null!;

    public virtual ICollection<TbShipment> TbShipments { get; set; } = new List<TbShipment>();
}