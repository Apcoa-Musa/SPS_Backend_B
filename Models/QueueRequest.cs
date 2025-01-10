using System;                      
using System.Collections.Generic;  
using System.Threading.Tasks;      
using Microsoft.Extensions.Configuration; 

namespace GarageQueueUpload.Models
{
    public class QueueRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<QueueMember> QueueMembers { get; set; } = new();
    }
}
