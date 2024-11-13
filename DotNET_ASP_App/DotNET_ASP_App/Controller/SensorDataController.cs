using DotNET_ASP_App.DTOs.Responses;
using DotNET_ASP_App.Service;
using Microsoft.AspNetCore.Mvc;

namespace DotNET_ASP_App.Controller;

[ApiController]
[Route("api")]
public class SensorDataController : ControllerBase
{
    private readonly SensorService sensorService;

    public SensorDataController(SensorService sensorService_)
    {
        sensorService = sensorService_;
    }

    [HttpGet]
    [Route("data")]
    public IActionResult getAllSensors(
        [FromQuery] string? sensorType = null,
        [FromQuery] string? sensorId = null,
        [FromQuery] string? startDate = null,
        [FromQuery] string? endDate = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortOrder = null,
        [FromQuery] string? dataType = null)
    {
        try
        {
            GetAllSensorsResponse response = sensorService.GetSensorsData(sensorType, sensorId, startDate, endDate, sortBy, sortOrder);
            
            if (string.IsNullOrEmpty(dataType)) return Ok(response);
            
            if (dataType.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                var csv = sensorService.createCsv(response);
                return File(csv, "text/csv");
            }
            else if (dataType.Equals("json", StringComparison.OrdinalIgnoreCase))
            {
                var json = sensorService.createJson(response);
                return File(json, "application/json");
            }
            
            return BadRequest("Invalid data type");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}