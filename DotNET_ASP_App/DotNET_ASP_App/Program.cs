using DotNET_ASP_App.Controller;
using DotNET_ASP_App.Queue;
using DotNET_ASP_App.Repository;
using DotNET_ASP_App.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SensorService>();
builder.Services.AddSingleton<BlockchainService>();
builder.Services.AddControllers();

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
app.MapGet("/sensors/sorted/{category}/{isAscending}", (string category, bool isAscending) => sensorController.GetOneCategorySorted(category, isAscending));
app.MapGet("/sensors/sorted/{category}/{id}/{isAscending}", (string category, int id, bool isAscending) => sensorController.GetOneSensorSorted(category, id, isAscending));
app.MapGet("/sensors/sorted/date/{isAscending}", (bool isAscending) => sensorController.GetAllSensorsDataSortByDate(isAscending));
app.MapGet("/sensors/sorted/date/{category}/{isAscending}", (string category, bool isAscending) => sensorController.GetOneCategorySortByDate(category, isAscending));
app.MapGet("/sensors/sorted/date/{category}/{id}/{isAscending}", (string category, int id, bool isAscending) => sensorController.GetOneSensorSortByDate(category, id, isAscending));
app.MapGet("/sensors/dateRange/{from}/{to}", (string from, string to) => sensorController.GetAllSensorsDataFromDateToDate(from, to)); // YYYY-MM-DDTHH:MM:SS
app.MapGet("/sensors/dateRange/{category}/{from}/{to}", (string category, string from, string to) => sensorController.GetOneCategoryFromDateToDate(category, from, to)); // YYYY-MM-DDTHH:MM:SS)
app.MapGet("/sensors/dateRange/{category}/{id}/{from}/{to}", (string category, int id, string from, string to) => sensorController.GetOneSensorFromDateToDate(category, id, from, to)); // YYYY-MM-DDTHH:MM:SS
// END MAP GET SENSORS

// START MAP PUT
app.MapPut("/dropdb", () => sensorController.DropDB());
// END MAP PUT

//END ROUTING
app.MapControllers();
app.Run();