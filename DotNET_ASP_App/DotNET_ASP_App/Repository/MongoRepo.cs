using DotNET_ASP_App.DTOs;
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
    
    public void SaveSensorData(SensorDTO sensorData, bool alt)
    {
        try
        {
            if (alt)
            {
                sensorData.Id = Guid.NewGuid().ToString();
                mongo.GetCollection<SensorDTO>(sensorData.SensorType).InsertOne(sensorData);
                return;
            }

            switch (sensorData.SensorType)
            {
                case "FirstSensor":
                    FirstSensorDTO data = new FirstSensorDTO
                        { Data = sensorData.Data, SensorId = sensorData.SensorId, Id = Guid.NewGuid() };
                    mongo.GetCollection<FirstSensorDTO>("FirstSensor").InsertOne(data);
                    break;
                case "SecondSensor":
                    SecondSensorDTO data2 = new SecondSensorDTO
                    {
                        Data = double.Parse(sensorData.Data, System.Globalization.CultureInfo.InvariantCulture),
                        SensorId = sensorData.SensorId, Id = Guid.NewGuid()
                    };
                    mongo.GetCollection<SecondSensorDTO>("SecondSensor").InsertOne(data2);
                    break;
                case "ThirdSensor":
                    ThirdSensorDTO data3 = new ThirdSensorDTO
                        { Data = int.Parse(sensorData.Data), SensorId = sensorData.SensorId, Id = Guid.NewGuid() };
                    mongo.GetCollection<ThirdSensorDTO>("ThirdSensor").InsertOne(data3);
                    break;
                case "FourthSensor":
                    FourthSensorDTO data4 = new FourthSensorDTO
                        { Data = Boolean.Parse(sensorData.Data), SensorId = sensorData.SensorId, Id = Guid.NewGuid() };
                    mongo.GetCollection<FourthSensorDTO>("FourthSensor").InsertOne(data4);
                    break;
                default:
                    Console.WriteLine("Unknown sensor type.");
                    break;
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
        sensors.AddRange(mongo.GetCollection<FirstSensorDTO>("FirstSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "FirstSensor", SensorId = x.SensorId, Data = x.Data }));
        sensors.AddRange(mongo.GetCollection<SecondSensorDTO>("SecondSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "SecondSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
        sensors.AddRange(mongo.GetCollection<ThirdSensorDTO>("ThirdSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "ThirdSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
        sensors.AddRange(mongo.GetCollection<FourthSensorDTO>("FourthSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "FourthSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
        return sensors;
    }

    public SensorAgregation GetAllSensors()
    {
        SensorAgregation sensors = new();
        sensors.FirstSensors = mongo.GetCollection<FirstSensorDTO>("FirstSensor").Find(_ => true).ToList().Select(x => new FirstSensorDTO { SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime }).ToList();
        sensors.SecondSensors = mongo.GetCollection<SecondSensorDTO>("SecondSensor").Find(_ => true).ToList().Select(x => new SecondSensorDTO { SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime }).ToList();
        sensors.ThirdSensors = mongo.GetCollection<ThirdSensorDTO>("ThirdSensor").Find(_ => true).ToList().Select(x => new ThirdSensorDTO { SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime }).ToList();
        sensors.FourthSensors = mongo.GetCollection<FourthSensorDTO>("FourthSensor").Find(_ => true).ToList().Select(x => new FourthSensorDTO { SensorId = x.SensorId, Data = x.Data, DateTime = x.DateTime }).ToList();
        return sensors;
    }
    
    public record SensorAgregation
    {
        public List<FirstSensorDTO> FirstSensors { get; set; }
        public List<SecondSensorDTO> SecondSensors { get; set; }
        public List<ThirdSensorDTO> ThirdSensors { get; set; }
        public List<FourthSensorDTO> FourthSensors { get; set; }
    }
    
    public List<T> GetOneCategory<T>(string collectionName)
    {
        return mongo.GetCollection<T>(collectionName).Find(_ => true).ToList();
    }
    
    public List<SensorDTO> GetOneCategoryDTO(string category)
    {
        List<SensorDTO> sensors = new();
        switch (category)
        {
            case "FirstSensor":
                sensors.AddRange(mongo.GetCollection<FirstSensorDTO>("FirstSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "FirstSensor", SensorId = x.SensorId, Data = x.Data }));
                break;
            case "SecondSensor":
                sensors.AddRange(mongo.GetCollection<SecondSensorDTO>("SecondSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "SecondSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
                break;
            case "ThirdSensor":
                sensors.AddRange(mongo.GetCollection<ThirdSensorDTO>("ThirdSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "ThirdSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
                break;
            case "FourthSensor":
                sensors.AddRange(mongo.GetCollection<FourthSensorDTO>("FourthSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "FourthSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
                break;
            default:
                Console.WriteLine("Unknown sensor type.");
                break;
        }
        return sensors;
    }
    
    public List<SensorDTO> GetOneSensorDTO(string category, int id)
    {
        List<SensorDTO> sensors = new();
        switch (category)
        {
            case "FirstSensor":
                sensors.AddRange(mongo.GetCollection<FirstSensorDTO>("FirstSensor").Find(x => x.SensorId == id).ToList().Select(x => new SensorDTO { SensorType = "FirstSensor", SensorId = x.SensorId, Data = x.Data }));
                break;
            case "SecondSensor":
                sensors.AddRange(mongo.GetCollection<SecondSensorDTO>("SecondSensor").Find(x => x.SensorId == id).ToList().Select(x => new SensorDTO { SensorType = "SecondSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
                break;
            case "ThirdSensor":
                sensors.AddRange(mongo.GetCollection<ThirdSensorDTO>("ThirdSensor").Find(x => x.SensorId == id).ToList().Select(x => new SensorDTO { SensorType = "ThirdSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
                break;
            case "FourthSensor":
                sensors.AddRange(mongo.GetCollection<FourthSensorDTO>("FourthSensor").Find(x => x.SensorId == id).ToList().Select(x => new SensorDTO { SensorType = "FourthSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
                break;
            default:
                Console.WriteLine("Unknown sensor type.");
                break;
        }
        return sensors;
    }
}