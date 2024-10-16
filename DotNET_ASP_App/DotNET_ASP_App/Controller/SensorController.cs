
using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.DTOs.Responses;
using DotNET_ASP_App.Repository;
using DotNET_ASP_App.Service;

namespace DotNET_ASP_App.Controller;

public class SensorController 
{
    public SensorService SensorService { get; }
    
    public SensorController(SensorService sensorService)
    {
        SensorService = sensorService;
    }
    
    public GetAllSensorsResponse GetAllSensorsData()
    {
        return SensorService.GetAllSensorsData();
    }
    
    public GetOneCategoryResponse GetOneCategoryResponse(string category)
    {
        return SensorService.GetOneCategoryResponse(category);
    }
    
    public GetOneCategoryResponse GetOneSensorResponse(string category, int id)
    {
        return SensorService.GetOneSensorResponse(category, id);
    }
    
    public GetAllSensorsResponse GetAllSensorsDataSorted(bool isAscending)
    {
        return SensorService.GetAllSensorsDataSorted(isAscending);
    }
    
    public void DropDB()
    {
        MongoRepo.GetInstance().EraseDB();
    }
}