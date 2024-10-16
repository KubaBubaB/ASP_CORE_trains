namespace DotNET_ASP_App.DTOs;

public record FirstSensorDTO
{
    public Guid Id { get; set; }
    
    public int SensorId { get; set; }
    
    public string Data { get; set; }
}