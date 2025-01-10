using System;

public class QueueModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string CarParkId { get; set; }
    public string ProductTemplateId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public int CurrentPosition { get; set; }
    public decimal QueuePrice { get; set; }
}
