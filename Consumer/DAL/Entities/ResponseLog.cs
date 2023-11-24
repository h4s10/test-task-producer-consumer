using System.ComponentModel.DataAnnotations;

namespace Consumer.DAL.Entities;

public class ResponseLog
{
    public Guid Id { get; set; }
    
    [MaxLength(2000)]
    public string Url { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Response { get; set; } = null!;
}