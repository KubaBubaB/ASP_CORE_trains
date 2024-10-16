namespace DotNET_ASP_App.DTOs.Responses;

public record GetAllSensorsResponse
{
    public List<SensorDTO> FirstSensors { get; set; }
    public List<SensorDTO> SecondSensors { get; set; }
    public List<SensorDTO> ThirdSensors { get; set; }
    public List<SensorDTO> FourthSensors { get; set; }
}