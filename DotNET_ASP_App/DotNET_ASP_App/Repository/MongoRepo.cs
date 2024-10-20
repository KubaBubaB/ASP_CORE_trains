using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.Sensors;
using MongoDB.Driver;

namespace DotNET_ASP_App.Repository;

public class MongoRepo
{
    private IMongoDatabase mongo = MongoDBHandler.GetClient().GetDatabase("DOTNET");

    private static MongoRepo instance;
    
    public static MongoRepo GetInstance()
    {
        if (instance == null)
        {
            instance = new MongoRepo();
        }

        return instance;
    }
    
    public void SaveSensorData(SensorDTO sensorData)
    {
        try
        {
            if (new List<string> { "Temperature", "Vibration", "Humidity", "Pressure" }.Contains(sensorData.SensorType))
            {
                mongo.GetCollection<Sensor>(sensorData.SensorType).InsertOne(new Sensor
                {
                    Id = Guid.NewGuid(),
                    SensorType = sensorData.SensorType,
                    SensorId = sensorData.SensorId,
                    Data = sensorData.Data,
                    DateTime = DateTime.Parse(sensorData.DateTime)
                });
            }
            else
            {
                throw new FormatException("Unknown sensor type of " + sensorData.SensorType);
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void EraseDB()
    {
        MongoDBHandler.GetClient().DropDatabase("DOTNET");
    }
    
    public List<SensorDTO> GetAllSensorsDTO()
    {
        List<SensorDTO> sensors = new();
        sensors.AddRange(mongo.GetCollection<Sensor>("Temperature").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "Temperature", SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime.ToString("yyyy-MM-dd HH:mm:ss") }));
        sensors.AddRange(mongo.GetCollection<Sensor>("Vibration").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "Vibration", SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime.ToString("yyyy-MM-dd HH:mm:ss") }));
        sensors.AddRange(mongo.GetCollection<Sensor>("Humidity").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "Humidity", SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime.ToString("yyyy-MM-dd HH:mm:ss") }));
        sensors.AddRange(mongo.GetCollection<Sensor>("Pressure").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "Pressure", SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime.ToString("yyyy-MM-dd HH:mm:ss") }));
        return sensors;
    }

    public SensorAgregation GetAllSensors()
    {
        SensorAgregation sensors = new();
        sensors.TemperatureSensors = mongo.GetCollection<Sensor>("Temperature").Find(_ => true).ToList().Select(x => new Sensor { SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime }).ToList();
        sensors.VibrationSensors = mongo.GetCollection<Sensor>("Vibration").Find(_ => true).ToList().Select(x => new Sensor { SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime }).ToList();
        sensors.HumiditySensors = mongo.GetCollection<Sensor>("Humidity").Find(_ => true).ToList().Select(x => new Sensor { SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime }).ToList();
        sensors.PressureSensors = mongo.GetCollection<Sensor>("Pressure").Find(_ => true).ToList().Select(x => new Sensor { SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime }).ToList();
        return sensors;
    }
    
    public record SensorAgregation
    {
        public List<Sensor> TemperatureSensors { get; set; }
        public List<Sensor> VibrationSensors { get; set; }
        public List<Sensor> HumiditySensors { get; set; }
        public List<Sensor> PressureSensors { get; set; }
    }
    
    public List<Sensor> GetOneCategory(string collectionName)
    {
        return mongo.GetCollection<Sensor>(collectionName).Find(_ => true).ToList();
    }
    
    public List<SensorDTO> GetOneCategoryDTO(string category)
    {
        List<SensorDTO> sensors = new();
        try
        {
            sensors.AddRange(mongo.GetCollection<Sensor>(category).Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = x.SensorType, SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime.ToString("yyyy-MM-dd HH:mm:ss") }));
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return sensors;
    }
    
    public List<SensorDTO> GetOneSensorDTO(string category, int id)
    {
        List<SensorDTO> sensors = new();
        try
        {
            sensors.AddRange(mongo.GetCollection<Sensor>(category).Find(x => x.SensorId == id).ToList().Select(x => new SensorDTO { SensorType = x.SensorType, SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime.ToString("yyyy-MM-dd HH:mm:ss") }));
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return sensors;
    }
}