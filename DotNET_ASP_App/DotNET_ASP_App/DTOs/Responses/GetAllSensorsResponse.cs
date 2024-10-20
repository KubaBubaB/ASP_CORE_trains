namespace DotNET_ASP_App.DTOs.Responses;

public record GetAllSensorsResponse
{
    public List<SensorDTO> Sensors { get; set; }
}