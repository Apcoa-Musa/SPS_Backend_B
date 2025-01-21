using System;

namespace GarageQueueUpload.Models
{
    public class QueueModel
    {
        public Guid QueueId { get; set; } 
        public string QueueName { get; set; } 
        public Guid CarParkId { get; set; } 
        public int CarParkDSNumber { get; set; } 
        public string Priority { get; set; } 
        public string Description { get; set; } 
        public string Status { get; set; } 
        public DateTime DateCreated { get; set; } 
        public decimal QueuePrice { get; set; }
        public Guid ProductTemplateId { get; set; }

    }
}
