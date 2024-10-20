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