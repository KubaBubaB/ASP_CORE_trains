namespace DotNET_ASP_App.DTOs;

public class SensorDTO
{
    public string SensorType { get; set; }
      
    public int SensorId { get; set; }

    public double Data { get; set; }
    // FORMAT: "yyyy-MM-dd HH:mm:ss"
    public string DateTime { get; set; }
}