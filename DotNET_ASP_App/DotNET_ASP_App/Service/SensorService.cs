using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.DTOs.Responses;
using DotNET_ASP_App.Repository;

// using System.Text.Json; string jsonString = JsonSerializer.Serialize(object);

namespace DotNET_ASP_App.Service;

public class SensorService
{
    public GetAllSensorsResponse GetAllSensorsData()
    {
        var sensors = MongoRepo.GetInstance().GetAllSensorsDTO();
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
        var sensors = MongoRepo.GetInstance().GetOneCategoryDTO(category);
        return new GetOneCategoryResponse
        {
            Sensors = sensors
        };
    }
    
    public GetOneCategoryResponse GetOneSensorResponse(string category, int id)
    {
        var sensors = MongoRepo.GetInstance().GetOneSensorDTO(category, id);
        return new GetOneCategoryResponse
        {
            Sensors = sensors
        };
    }

    public GetAllSensorsResponse GetAllSensorsDataSorted(bool isAscending)
    {
        var sensors = MongoRepo.GetInstance().GetAllSensorsDTO();
        return new GetAllSensorsResponse
        {
            FirstSensors = isAscending ? sensors.FindAll(s => s.SensorType == "FirstSensor").OrderBy(s => s.Data).ToList() : sensors.FindAll(s => s.SensorType == "FirstSensor").OrderByDescending(s => s.Data).ToList(),
            SecondSensors = isAscending ? sensors.FindAll(s => s.SensorType == "SecondSensor").OrderBy(s => s.Data).ToList() : sensors.FindAll(s => s.SensorType == "SecondSensor").OrderByDescending(s => s.Data).ToList(),
            ThirdSensors = isAscending ? sensors.FindAll(s => s.SensorType == "ThirdSensor").OrderBy(s => s.Data).ToList() : sensors.FindAll(s => s.SensorType == "ThirdSensor").OrderByDescending(s => s.Data).ToList(),
            FourthSensors = isAscending ? sensors.FindAll(s => s.SensorType == "FourthSensor").OrderBy(s => s.Data).ToList() : sensors.FindAll(s => s.SensorType == "FourthSensor").OrderByDescending(s => s.Data).ToList()
        };
    }

    public GetOneCategoryResponse GetOneCategorySorted(string category, bool isAscending)
    {
        var sensors = MongoRepo.GetInstance().GetOneCategoryDTO(category);
        return new GetOneCategoryResponse
        {
            Sensors = isAscending ? sensors.OrderBy(s => s.Data).ToList() : sensors.OrderByDescending(s => s.Data).ToList()
        };
    }

    public GetOneCategoryResponse GetOneSensorSorted(string category, int id, bool isAscending)
    {
        var sensors = MongoRepo.GetInstance().GetOneSensorDTO(category, id);
        return new GetOneCategoryResponse
        {
            Sensors = isAscending ? sensors.OrderBy(s => s.Data).ToList() : sensors.OrderByDescending(s => s.Data).ToList()
        };
    }

    public GetAllSensorsResponse GetAllSensorsDataSortByDate(bool isAscending)
    {
        var sensors = MongoRepo.GetInstance().GetAllSensors();
        var firstSensors = isAscending ? sensors.FirstSensors.OrderBy(s => s.DateTime).ToList() : sensors.FirstSensors.OrderByDescending(s => s.DateTime).ToList();
        var secondSensors = isAscending ? sensors.SecondSensors.OrderBy(s => s.DateTime).ToList() : sensors.SecondSensors.OrderByDescending(s => s.DateTime).ToList();
        var thirdSensors = isAscending ? sensors.ThirdSensors.OrderBy(s => s.DateTime).ToList() : sensors.ThirdSensors.OrderByDescending(s => s.DateTime).ToList();
        var fourthSensors = isAscending ? sensors.FourthSensors.OrderBy(s => s.DateTime).ToList() : sensors.FourthSensors.OrderByDescending(s => s.DateTime).ToList();
        return new GetAllSensorsResponse
        {
            FirstSensors = firstSensors.ConvertAll((listElement) => {return new SensorDTO{Data = listElement.Data, DateTime = listElement.DateTime.ToString(), SensorId = listElement.SensorId, SensorType = "FirstSensor"};}),
            SecondSensors = secondSensors.ConvertAll((listElement) => {return new SensorDTO{Data = listElement.Data.ToString(), DateTime = listElement.DateTime.ToString(), SensorId = listElement.SensorId, SensorType = "SecondSensor"};}),
            ThirdSensors = thirdSensors.ConvertAll((listElement) => {return new SensorDTO{Data = listElement.Data.ToString(), DateTime = listElement.DateTime.ToString(), SensorId = listElement.SensorId, SensorType = "SecondSensor"};}),
            FourthSensors = fourthSensors.ConvertAll((listElement) => {return new SensorDTO{Data = listElement.Data.ToString(), DateTime = listElement.DateTime.ToString(), SensorId = listElement.SensorId, SensorType = "SecondSensor"};})
        };
    }

    public GetOneCategoryResponse GetOneCategorySortByDate(string category, bool isAscending)
    {
        switch (category)
        {
            case "FirstSensor":
                var firstSensors = MongoRepo.GetInstance().GetOneCategory<FirstSensorDTO>("FirstSensor");
                if (isAscending)
                {
                    firstSensors = firstSensors.OrderBy(s => s.DateTime).ToList();
                }
                else
                {
                    firstSensors = firstSensors.OrderByDescending(s => s.DateTime).ToList();
                }

                return new GetOneCategoryResponse
                {
                    Sensors = firstSensors.ConvertAll((listElement) =>
                    {
                        return new SensorDTO
                        {
                            Data = listElement.Data, DateTime = listElement.DateTime.ToString(),
                            SensorId = listElement.SensorId, SensorType = "FirstSensor"
                        };
                    })
                };
            case "SecondSensor":
                var secondSensors = MongoRepo.GetInstance().GetOneCategory<SecondSensorDTO>("SecondSensor");
                if (isAscending)
                {
                    secondSensors = secondSensors.OrderBy(s => s.DateTime).ToList();
                }
                else
                {
                    secondSensors = secondSensors.OrderByDescending(s => s.DateTime).ToList();
                }

                return new GetOneCategoryResponse
                {
                    Sensors = secondSensors.ConvertAll((listElement) =>
                    {
                        return new SensorDTO
                        {
                            Data = listElement.Data.ToString(), DateTime = listElement.DateTime.ToString(),
                            SensorId = listElement.SensorId, SensorType = "SecondSensor"
                        };
                    })
                };
            case "ThirdSensor":
                var thirdSensor = MongoRepo.GetInstance().GetOneCategory<ThirdSensorDTO>("ThirdSensor");
                if (isAscending)
                {
                    thirdSensor = thirdSensor.OrderBy(s => s.DateTime).ToList();
                }
                else
                {
                    thirdSensor = thirdSensor.OrderByDescending(s => s.DateTime).ToList();
                }

                return new GetOneCategoryResponse
                {
                    Sensors = thirdSensor.ConvertAll((listElement) =>
                    {
                        return new SensorDTO
                        {
                            Data = listElement.Data.ToString(), DateTime = listElement.DateTime.ToString(),
                            SensorId = listElement.SensorId, SensorType = "ThirdSensor"
                        };
                    })
                };
                break;
            case "FourthSensor":
                var fourthSensor = MongoRepo.GetInstance().GetOneCategory<FourthSensorDTO>("FourthSensor");
                if (isAscending)
                {
                    fourthSensor = fourthSensor.OrderBy(s => s.DateTime).ToList();
                }
                else
                {
                    fourthSensor = fourthSensor.OrderByDescending(s => s.DateTime).ToList();
                }

                return new GetOneCategoryResponse
                {
                    Sensors = fourthSensor.ConvertAll((listElement) =>
                    {
                        return new SensorDTO
                        {
                            Data = listElement.Data.ToString(), DateTime = listElement.DateTime.ToString(),
                            SensorId = listElement.SensorId, SensorType = "FourthSensor"
                        };
                    })
                };
            default:
                Console.WriteLine("Unknown sensor type.");
                return null;
        }
    }

    public GetOneCategoryResponse GetOneSensorSortByDate(string category, int id, bool isAscending)
    {
        var categoryOfSensor = GetOneCategorySortByDate(category, isAscending);
        return new GetOneCategoryResponse
        {
            Sensors = categoryOfSensor.Sensors.FindAll(s => s.SensorId == id)
        };
    }
}