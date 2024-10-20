using DotNET_ASP_App.Controller;
using DotNET_ASP_App.Queue;
using DotNET_ASP_App.Repository;
using DotNET_ASP_App.Service;
using MQTTnet;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var sensorService = new SensorService();
var sensorController = new SensorController(sensorService);

// Initialize connection to Mongo
MongoDBHandler.GetClient();

// Start the MQTT handler
_ = Task.Run(async () => await MQTTHandler.Handle_Received_Application_Message());

//ROUTING

// START MAP GET SENSORS
app.MapGet("/sensors", () => sensorController.GetAllSensorsData());
app.MapGet("/sensors/{category}", (string category) => sensorController.GetOneCategoryResponse(category));
app.MapGet("/sensors/{category}/{id}", (string category, int id) => sensorController.GetOneSensorResponse(category, id));
app.MapGet("/sensors/sorted/{isAscending}", (bool isAscending) => sensorController.GetAllSensorsDataSorted(isAscending));
// END MAP GET SENSORS

// START MAP PUT
app.MapPut("/dropdb", () => sensorController.DropDB());
// END MAP PUT

//END ROUTING
app.Run();