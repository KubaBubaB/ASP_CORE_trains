using System.Text.Json;
using DotNET_ASP_App.DTOs;

namespace DotNET_ASP_App.WebSocket;

public class NotificationService
{
    private readonly WebSocketHandler _webSocketHandler;
    private readonly Dictionary<int, Queue<double>> _sensorData = new();

    public NotificationService(WebSocketHandler webSocketHandler)
    {
        _webSocketHandler = webSocketHandler;
    }

    public async Task NotifyClientsAsync(SensorDTO message)
    {
        if (!_sensorData.ContainsKey(message.SensorId))
        {
            _sensorData[message.SensorId] = new Queue<double>();
        }

        var sensorQueue = _sensorData[message.SensorId];
        if (sensorQueue.Count >= 100)
        {
            sensorQueue.Dequeue();
        }
        sensorQueue.Enqueue(message.Data);

        double mean = sensorQueue.Average();

        var sensorWithMean = new SensorWithMeanDTO
        {
            SensorId = message.SensorId,
            Data = message.Data,
            DateTime = message.DateTime,
            Mean = mean
        };
        try
        {
            await _webSocketHandler.BroadcastMessageAsync(JsonSerializer.Serialize<SensorWithMeanDTO>(sensorWithMean));
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine($"Error serializing payload: {ex.Message}");
        }
    }
}