using DotNET_ASP_App.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DotNET_ASP_App.Repository;

public class MongoDBHandler
{
    private static MongoDBHandler instance;
    private MongoClient client;
    private MongoDBHandler()
    {
        client = new MongoClient("mongodb://admin:admin@localhost:27017");
        var collection = client.GetDatabase("DOTNET").GetCollection<FirstSensorDTO>("InitCollection");
        var filter = Builders<FirstSensorDTO>.Filter.Eq("Data", "value");
        if (collection.Find(filter) != null)
        {
            Console.WriteLine("Database already initialized");
        }
        else
        {
            collection.InsertOne(new FirstSensorDTO { SensorId = 1, Data = "value", Id = Guid.NewGuid()});
            var document = collection.Find(filter).First();
            if (document.Data == "value")
            {
                Console.WriteLine("Database init successful");
            }
        }
    }
    public static MongoClient GetClient()
    {
        if (instance == null)
        {
            instance = new MongoDBHandler();
        }
        return instance.client;
    }
}