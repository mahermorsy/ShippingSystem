namespace Domains.Models;
public partial class VwCity 
{
    public Guid CityId { get; set; }
    public string? CityNameAr { get; set; }
    public string? CityNameEn { get; set; }
    public Guid CountryId { get; set; }
    public string? CountryNameAr { get; set; }
    public string? CountryNameEn { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
