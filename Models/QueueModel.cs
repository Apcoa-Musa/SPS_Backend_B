#nullable enable
using System;

namespace GarageQueueUpload.Models
{

    public class QueueModel
    {
        public Guid QueueId { get; set; }
        public string QueueName { get; set; } = "N/A";
        public Guid CarParkId { get; set; }
        public int CarParkDSNumber { get; set; }
        public string Priority { get; set; } = "N/A";
        public string Description { get; set; } = "N/A";
        public string Status { get; set; } = "N/A";
        public DateTime? DateCreated { get; set; } = null;
        public decimal QueuePrice { get; set; } = 0m;
        public Guid ProductTemplateId { get; set; } 
        public Guid Id { get; set; }
        public string? CarParkFacilityName { get; set; }
        public int VerifiedMemberCount { get; set; } = 0;
        public object? ActiveQueueMembers { get; set; } 
    }
}

