namespace DotNET_ASP_App.DTOs;

public class SensorDTO
{
    public string SensorType { get; set; }
      
    public int SensorId { get; set; }

    public string Data { get; set; }
    // FORMAT: "DD/MM/YYYY HH:MM:SS"
    public string DateTime { get; set; }
}