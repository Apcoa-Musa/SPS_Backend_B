using System;

public class QueueModel
{
    public Guid QueueId { get; set; }
    public string QueueName { get; set; }
    public int CarParkDSNumber { get; set; }
    public string CarParkFacilityName { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public decimal QueuePrice { get; set; }
    public int VerifiedMemberCount { get; set; }
    public object ActiveQueueMembers { get; set; }
    public Guid CarParkId { get; set; }

    // Lägg till dessa egenskaper
    public int Priority { get; set; } 
    public string Description { get; set; } = string.Empty; 
    public string Status { get; set; } = "Active"; 
}
