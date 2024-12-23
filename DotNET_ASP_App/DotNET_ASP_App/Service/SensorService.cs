﻿using System.Text;
using System.Text.Json;
using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.DTOs.Responses;
using DotNET_ASP_App.Repository;
using DotNET_ASP_App.Sensors;

namespace DotNET_ASP_App.Service;

public class SensorService
{
    public GetAllSensorsResponse GetAllSensorsData()
    {
        var sensors = MongoRepo.GetInstance().GetAllSensorsDTO();
        return new GetAllSensorsResponse
        {
            Sensors = sensors
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
            Sensors = isAscending ? sensors.OrderBy(s => s.Data).ToList() : sensors.OrderByDescending(s => s.Data).ToList(),
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
        var sensorsToRet = new List<Sensor>();
        sensorsToRet.AddRange(sensors.PressureSensors);
        sensorsToRet.AddRange(sensors.TemperatureSensors);
        sensorsToRet.AddRange(sensors.HumiditySensors);
        sensorsToRet.AddRange(sensors.VibrationSensors);
        sensorsToRet = isAscending ? sensorsToRet.OrderBy(s => s.DateTime).ToList() : sensorsToRet.OrderByDescending(s => s.DateTime).ToList();
        return new GetAllSensorsResponse
        {
            Sensors = sensorsToRet.ConvertAll(listElement => new SensorDTO
            {
                Data = listElement.Data, DateTime = listElement.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                SensorId = listElement.SensorId, SensorType = listElement.SensorType
            })
        };
    }

    public GetOneCategoryResponse GetOneCategorySortByDate(string category, bool isAscending)
    {
        try
        {
            var sensors = MongoRepo.GetInstance().GetOneCategory(category);
            if(isAscending)
            {
                sensors = sensors.OrderBy(s => s.DateTime).ToList();
            }
            else
            {
                sensors = sensors.OrderByDescending(s => s.DateTime).ToList();
            }

            return new GetOneCategoryResponse
            {
                Sensors = sensors.ConvertAll(listElement => new SensorDTO
                {
                    Data = listElement.Data, DateTime = listElement.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    SensorId = listElement.SensorId, SensorType = category
                })
            };
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
            return new GetOneCategoryResponse
            {
                Sensors = new List<SensorDTO>()
            };
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
    
    public GetAllSensorsResponse GetAllSensorsDataFromDateToDate(DateTime fromDate, DateTime toDate)
    {
        var sensors = MongoRepo.GetInstance().GetAllSensors();
        var sensorsToRet = new List<Sensor>();
        sensorsToRet.AddRange(sensors.PressureSensors);
        sensorsToRet.AddRange(sensors.TemperatureSensors);
        sensorsToRet.AddRange(sensors.HumiditySensors);
        sensorsToRet.AddRange(sensors.VibrationSensors);
        sensorsToRet = sensorsToRet.FindAll(s => s.DateTime >= fromDate && s.DateTime <= toDate);
        return new GetAllSensorsResponse
        {
            Sensors = sensorsToRet.ConvertAll(listElement => new SensorDTO
            {
                Data = listElement.Data, DateTime = listElement.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                SensorId = listElement.SensorId, SensorType = listElement.SensorType
            })
        };
    }
    
    public GetOneCategoryResponse GetOneCategoryFromDateToDate(string category, DateTime fromDate, DateTime toDate)
    {
        try
        {
            var sensors = MongoRepo.GetInstance().GetOneCategory(category);
            sensors = sensors.FindAll(s => s.DateTime >= fromDate && s.DateTime <= toDate);
            return new GetOneCategoryResponse
            {
                Sensors = sensors.ConvertAll(listElement => new SensorDTO
                {
                    Data = listElement.Data, DateTime = listElement.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    SensorId = listElement.SensorId, SensorType = category
                })
            };
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
            return new GetOneCategoryResponse
            {
                Sensors = new List<SensorDTO>()
            };
        }
    }
    
    public GetOneCategoryResponse GetOneSensorFromDateToDate(string category, int id, DateTime fromDate, DateTime toDate)
    {
        var categoryOfSensor = GetOneCategoryFromDateToDate(category, fromDate, toDate);
        return new GetOneCategoryResponse
        {
            Sensors = categoryOfSensor.Sensors.FindAll(s => s.SensorId == id)
        };
    }
    
    public GetAllSensorsResponse GetSensorsData(string? sensorType,
        string? sensorId,
        string? startDate,
        string? endDate,
        string? sortBy,
        string? sortOrder)
    {
        var sensorsToRet = new List<Sensor>();
            
        if (string.IsNullOrEmpty(sensorType)) {
            var sensors = MongoRepo.GetInstance().GetAllSensors();
            sensorsToRet.AddRange(sensors.PressureSensors);
            sensorsToRet.AddRange(sensors.TemperatureSensors);
            sensorsToRet.AddRange(sensors.HumiditySensors);
            sensorsToRet.AddRange(sensors.VibrationSensors);
        }
        else {
            sensorsToRet = MongoRepo.GetInstance().GetOneCategory(sensorType);
        }

        if (!string.IsNullOrEmpty(sensorId))
        {
            sensorsToRet = sensorsToRet.FindAll(s => s.SensorId == Int32.Parse(sensorId));
        }
        
        if (!string.IsNullOrEmpty(startDate))
        {
            sensorsToRet = sensorsToRet.FindAll(s => s.DateTime >= DateTime.Parse(startDate));
        }
        
        if (!string.IsNullOrEmpty(endDate))
        {
            sensorsToRet = sensorsToRet.FindAll(s => s.DateTime <= DateTime.Parse(endDate));
        }
        
        if (!string.IsNullOrEmpty(sortBy))
        {
            if(!string.IsNullOrEmpty(sortOrder) && sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                if (sortBy.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                {
                    sensorsToRet = sensorsToRet.OrderByDescending(s => s.DateTime).ToList();
                }
                else if (sortBy.Equals("Data", StringComparison.OrdinalIgnoreCase))
                {
                    sensorsToRet = sensorsToRet.OrderByDescending(s => s.Data).ToList();
                }
                else if (sortBy.Equals("SensorId", StringComparison.OrdinalIgnoreCase))
                {
                    sensorsToRet = sensorsToRet.OrderByDescending(s => s.SensorId).ToList();
                }
                else if (sortBy.Equals("SensorType", StringComparison.OrdinalIgnoreCase))
                {
                    sensorsToRet = sensorsToRet.OrderByDescending(s => s.SensorType).ToList();
                }
            }
            else
            {
                if (sortBy.Equals("DateTime", StringComparison.OrdinalIgnoreCase))
                {
                    sensorsToRet = sensorsToRet.OrderBy(s => s.DateTime).ToList();
                }
                else if (sortBy.Equals("Data", StringComparison.OrdinalIgnoreCase))
                {
                    sensorsToRet = sensorsToRet.OrderBy(s => s.Data).ToList();
                }
                else if (sortBy.Equals("SensorId", StringComparison.OrdinalIgnoreCase))
                {
                    sensorsToRet = sensorsToRet.OrderBy(s => s.SensorId).ToList();
                }
                else if (sortBy.Equals("SensorType", StringComparison.OrdinalIgnoreCase))
                {
                    sensorsToRet = sensorsToRet.OrderBy(s => s.SensorType).ToList();
                }
            }
        }
        
        return new GetAllSensorsResponse
        {
            Sensors = sensorsToRet.ConvertAll(listElement => new SensorDTO
            {
                Data = listElement.Data, DateTime = listElement.DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                SensorId = listElement.SensorId, SensorType = listElement.SensorType
            })
        };
    }

    public Stream createCsv(GetAllSensorsResponse sensors)
    {
        var memoryStream = new MemoryStream();
        
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
        {   
            writer.WriteLine("SensorId;SensorType;Data;DateTime");
            foreach (var sensor in sensors.Sensors)
            {
                var line = $"{sensor.SensorId};{sensor.SensorType};{sensor.Data};{sensor.DateTime}";
                writer.WriteLine(line);
            }
        }
        memoryStream.Position = 0;
        return memoryStream;
    }
    
    public Stream createJson(GetAllSensorsResponse sensors)
    {
        var memoryStream = new MemoryStream();
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
        {
            string jsonString = JsonSerializer.Serialize(sensors, new JsonSerializerOptions
            {
                WriteIndented = true 
            });
            
            writer.Write(jsonString);
        }
        
        memoryStream.Position = 0;
        return memoryStream;
    }
}