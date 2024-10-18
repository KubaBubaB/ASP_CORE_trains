
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
    
    public GetOneCategoryResponse GetOneCategorySorted(string category, bool isAscending)
    {
        return SensorService.GetOneCategorySorted(category, isAscending);
    }
    
    public GetOneCategoryResponse GetOneSensorSorted(string category, int id, bool isAscending)
    {
        return SensorService.GetOneSensorSorted(category, id, isAscending);
    }

    public GetAllSensorsResponse GetAllSensorsDataSortByDate(bool isAscending)
    {
        return SensorService.GetAllSensorsDataSortByDate(isAscending);
    }
    
    public GetOneCategoryResponse GetOneCategorySortByDate(string category, bool isAscending)
    {
        return SensorService.GetOneCategorySortByDate(category, isAscending);
    }
    
    public GetOneCategoryResponse GetOneSensorSortByDate(string category, int id, bool isAscending)
    {
        return SensorService.GetOneSensorSortByDate(category, id, isAscending);
    }
    
    public void DropDB()
    {
        MongoRepo.GetInstance().EraseDB();
    }
}