using System;

public class QueueModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CarParkId { get; set; }
    public Guid ProductTemplateId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public int CurrentPosition { get; set; }
    public decimal QueuePrice { get; set; }
}
