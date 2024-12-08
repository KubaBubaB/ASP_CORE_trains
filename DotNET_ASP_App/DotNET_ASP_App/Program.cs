using System.Net.WebSockets;
using DotNET_ASP_App.Controller;
using DotNET_ASP_App.Queue;
using DotNET_ASP_App.Repository;
using DotNET_ASP_App.Service;
using DotNET_ASP_App.WebSocket;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SensorService>();
builder.Services.AddSingleton<BlockchainService>();
builder.Services.AddSingleton<WebSocketHandler>();
builder.Services.AddSingleton<NotificationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.AddControllers();

var app = builder.Build();
var sensorService = app.Services.GetRequiredService<SensorService>();
var blockchainService = app.Services.GetRequiredService<BlockchainService>();
var sensorController = new SensorController(sensorService);
var webSocketHandler = app.Services.GetRequiredService<WebSocketHandler>();
var notificationService = app.Services.GetRequiredService<NotificationService>();

// Init WebSocket

app.UseWebSockets();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            webSocketHandler.AddSocket(webSocket);

            // Keep the connection open
            while (webSocket.State == WebSocketState.Open)
            {
                await Task.Delay(1000); // Prevent high CPU usage
            }
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});
/*
 * Example of how to connect to the WebSocket from javascript :))
 * 
 *const socket = new WebSocket('ws://localhost:5000/ws');
 *
 *
 *socket.addEventListener('message', (event) => {
 *    console.log('Message from server:', event.data);
 *}); 
 */


app.UseCors("AllowAllOrigins");
app.UseRouting();

// Initialize connection to Mongo
MongoDBHandler.GetClient();

// Start the MQTT handler
var mqttHandler = new MQTTHandler(notificationService, blockchainService);
_ = Task.Run(async () => await mqttHandler.Handle_Received_Application_Message());

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