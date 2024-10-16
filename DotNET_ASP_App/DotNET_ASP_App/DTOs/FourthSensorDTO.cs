namespace DotNET_ASP_App.DTOs;

public record FourthSensorDTO
{
    public Guid Id { get; set; }
      
    public int SensorId { get; set; }

    public bool Data { get; set; }
}