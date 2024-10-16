namespace DotNET_ASP_App.DTOs.Responses;

public record GetOneCategoryResponse()
{
    public List<SensorDTO> Sensors { get; set; }
}