using System;                      
using System.Collections.Generic;  
using System.Threading.Tasks;      
using Microsoft.Extensions.Configuration; 

namespace GarageQueueUpload.Models
{
    public class QueueFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadeAt { get; set; }
    }
}