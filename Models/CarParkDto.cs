namespace GarageQueueUpload.Models
{
    public class CarParkDto
    {
        public Guid Id { get; set; }
        public int DSNumber { get; set; }
        public bool ActivityStatus { get; set; }
        public DateTime? OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string? ReasonForClosure { get; set; }
        public string? GeoCoordinates { get; set; }
        public double GeoLatitude { get; set; }
        public double GeoLongitude { get; set; }
        public string? Description { get; set; }
        public string? Permalink { get; set; }
        public string? FacilityName { get; set; }
        public string? AllotmentSetName { get; set; }
        public string? SFCostCenterNumber { get; set; }
        public int Capacity { get; set; }
        public int PhysicalPlacesCount { get; set; }
        public int RentableSpacesLimit { get; set; } = 0;
        public string? CSComment { get; set; }
        public int? NoticePeriod { get; set; }
        public int? ExtensionPeriod { get; set; }
        public bool VatByDefault { get; set; }
        public string? VatCode { get; set; }
        public string? ContractTemplateName { get; set; }
        public string? OnlineCustomerInfo { get; set; }     //Description for online customers
        public bool? HasDigitalPermit { get; set; }
        public int? District { get; set; }
        public int? Region { get; set; }
        public Guid PropertyOwnerId { get; set; }
        public Guid LandlordId { get; set; }
        public Guid PropertyManagerId { get; set; }
        public string? SiteIDLong { get; set; }
        public string? CarParkLongName { get; set; }
        public string? GeoFence { get; set; }
        public CarParkHasKeys HasKeys { get; set; }
        public Guid? KeyManagerId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid BannerId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
