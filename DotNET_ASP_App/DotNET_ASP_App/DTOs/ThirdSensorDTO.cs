namespace DotNET_ASP_App.DTOs;

public record ThirdSensorDTO 
{
    public Guid Id { get; set; }
      
    public int SensorId { get; set; }

    public int Data { get; set; }
}