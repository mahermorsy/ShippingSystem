namespace Domains.Models;

public partial class TbUserReceiver : BaseTable
{
    public Guid UserId { get; set; }
    public string ReceiverName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public Guid CityId { get; set; }
    public Guid CountryId { get; set; }
    public string Address { get; set; } = null!;
    public string? AddressDetails { get; set; }
    public string? OtherAddress { get; set; }
    public string? PostalCode { get; set; }
    public virtual TbCity City { get; set; } = null!;
    public virtual TbCountry Country { get; set; } = null!;
    public virtual ICollection<TbShipment> TbShipments { get; set; } = new List<TbShipment>();
}