using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.DTOs.Responses;
using DotNET_ASP_App.Repository;

// using System.Text.Json; string jsonString = JsonSerializer.Serialize(object);

namespace DotNET_ASP_App.Service;

public class SensorService
{
    public GetAllSensorsResponse GetAllSensorsData()
    {
        var sensors = MongoRepo.GetInstance().GetAllSensors();
        return new GetAllSensorsResponse
        {
            FirstSensors = sensors.FindAll(s => s.SensorType == "FirstSensor"),
            SecondSensors = sensors.FindAll(s => s.SensorType == "SecondSensor"),
            ThirdSensors = sensors.FindAll(s => s.SensorType == "ThirdSensor"),
            FourthSensors = sensors.FindAll(s => s.SensorType == "FourthSensor")
        };
    }

    public GetOneCategoryResponse GetOneCategoryResponse(string category)
    {
        var sensors = MongoRepo.GetInstance().GetOneCategory(category);
        return new GetOneCategoryResponse
        {
            Sensors = sensors
        };
    }
    
    public GetOneCategoryResponse GetOneSensorResponse(string category, int id)
    {
        var sensors = MongoRepo.GetInstance().GetOneSensor(category, id);
        return new GetOneCategoryResponse
        {
            Sensors = sensors
        };
    }

    public GetAllSensorsResponse GetAllSensorsDataSorted(bool isAscending)
    {
        var sensors = MongoRepo.GetInstance().GetAllSensors();
        return new GetAllSensorsResponse
        {
            FirstSensors = isAscending ? sensors.FindAll(s => s.SensorType == "FirstSensor").OrderBy(s => s.Data).ToList() : sensors.FindAll(s => s.SensorType == "FirstSensor").OrderByDescending(s => s.Data).ToList(),
            SecondSensors = isAscending ? sensors.FindAll(s => s.SensorType == "SecondSensor").OrderBy(s => s.Data).ToList() : sensors.FindAll(s => s.SensorType == "SecondSensor").OrderByDescending(s => s.Data).ToList(),
            ThirdSensors = isAscending ? sensors.FindAll(s => s.SensorType == "ThirdSensor").OrderBy(s => s.Data).ToList() : sensors.FindAll(s => s.SensorType == "ThirdSensor").OrderByDescending(s => s.Data).ToList(),
            FourthSensors = isAscending ? sensors.FindAll(s => s.SensorType == "FourthSensor").OrderBy(s => s.Data).ToList() : sensors.FindAll(s => s.SensorType == "FourthSensor").OrderByDescending(s => s.Data).ToList()
        };
    }
    
}