namespace DotNET_ASP_App.DTOs;

public record SecondSensorDTO
{
    public Guid Id { get; set; }
      
    public int SensorId { get; set; }

    public double Data { get; set; }
}