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
    
    public void SaveSensorData(SensorDTO sensorData)
    {
        switch (sensorData.SensorType)
        {
            case "FirstSensor":
                FirstSensorDTO data = new FirstSensorDTO{Data = sensorData.Data, SensorId = sensorData.SensorId, Id = Guid.NewGuid()};
                mongo.GetCollection<FirstSensorDTO>("FirstSensor").InsertOne(data);
                break;
            case "SecondSensor":
                SecondSensorDTO data2 = new SecondSensorDTO{Data = double.Parse(sensorData.Data, System.Globalization.CultureInfo.InvariantCulture), SensorId = sensorData.SensorId, Id = Guid.NewGuid()};
                mongo.GetCollection<SecondSensorDTO>("SecondSensor").InsertOne(data2);
                break;
            case "ThirdSensor":
                ThirdSensorDTO data3 = new ThirdSensorDTO{Data = int.Parse(sensorData.Data), SensorId = sensorData.SensorId, Id = Guid.NewGuid()};
                mongo.GetCollection<ThirdSensorDTO>("ThirdSensor").InsertOne(data3);
                break;
            case "FourthSensor":
                FourthSensorDTO data4 = new FourthSensorDTO{Data = Boolean.Parse(sensorData.Data), SensorId = sensorData.SensorId, Id = Guid.NewGuid()};
                mongo.GetCollection<FourthSensorDTO>("FourthSensor").InsertOne(data4);
                break;
            default:
                Console.WriteLine("Unknown sensor type.");
                break;
        }
    }

    public void EraseDB()
    {
        MongoDBHandler.GetClient().DropDatabase("DOTNET");
    }
    
    public List<SensorDTO> GetAllSensors()
    {
        List<SensorDTO> sensors = new();
        sensors.AddRange(mongo.GetCollection<FirstSensorDTO>("FirstSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "FirstSensor", SensorId = x.SensorId, Data = x.Data }));
        sensors.AddRange(mongo.GetCollection<SecondSensorDTO>("SecondSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "SecondSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
        sensors.AddRange(mongo.GetCollection<ThirdSensorDTO>("ThirdSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "ThirdSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
        sensors.AddRange(mongo.GetCollection<FourthSensorDTO>("FourthSensor").Find(_ => true).ToList().Select(x => new SensorDTO { SensorType = "FourthSensor", SensorId = x.SensorId, Data = x.Data.ToString() }));
        return sensors;
    }
    
    public List<SensorDTO> GetOneCategory(string category)
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
    
    public List<SensorDTO> GetOneSensor(string category, int id)
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