using System.Buffers;
using System.Text;
using System.Text.Json;
using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.Repository;
using DotNET_ASP_App.Service;
using DotNET_ASP_App.WebSocket;
using MQTTnet;

namespace DotNET_ASP_App.Queue;

public class MQTTHandler
{
    private readonly NotificationService _notificationService;
    private readonly BlockchainService _blockchainService;
    
    public MQTTHandler(NotificationService notificationService, BlockchainService blockchainService)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _blockchainService = blockchainService ?? throw new ArgumentNullException(nameof(blockchainService));
    }
    
    public async Task Handle_Received_Application_Message()
    {
        var mqttFactory = new MqttClientFactory();

        using (var mqttClient = mqttFactory.CreateMqttClient())
        {
            int portOfBroker = 1883;
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("mqtt5", portOfBroker).Build();
            
            // Handler for received application messages
            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                Console.WriteLine("Received application message.");

                var payloadSequence = e.ApplicationMessage.Payload;

                if (payloadSequence.IsEmpty)
                {
                    Console.WriteLine("Payload is empty.");
                    return;
                }

                var payloadBytes = payloadSequence.ToArray();
                var payloadString = Encoding.UTF8.GetString(payloadBytes);
                Console.WriteLine($"Raw payload: '{payloadString}'");

                var formattedPayload = payloadString.Replace('\'', '\"');
                Console.WriteLine($"Formatted payload: '{formattedPayload}'");

                SensorDTO sensorData = null;

                try
                {
                    sensorData = JsonSerializer.Deserialize<SensorDTO>(formattedPayload);
                    if (sensorData != null)
                    {
                        Console.WriteLine("Deserialization successful.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to deserialize payload to SensorDTO.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deserializing payload: {ex.Message}");
                    return;
                }

                try
                {
                    Console.WriteLine("Attempting to save sensor data to MongoDB.");
                    var mongoRepo = MongoRepo.GetInstance();
                    if (mongoRepo == null)
                    {
                        Console.WriteLine("MongoRepo instance is null.");
                    }
                    else
                    {
                        mongoRepo.SaveSensorData(sensorData);
                        Console.WriteLine("Sensor data saved to MongoDB.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving sensor data to MongoDB: {ex}");
                }

                try
                {
                    Console.WriteLine("Attempting to notify clients via WebSocket.");
                    if (_notificationService == null)
                    {
                        Console.WriteLine("_notificationService is null.");
                    }
                    else
                    {
                        await _notificationService.NotifyClientsAsync(sensorData);
                        Console.WriteLine("Clients notified.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notifying clients: {ex}");
                }
                
                try
                {
                    Console.WriteLine("Attempting to reward sensor.");
                    if (_blockchainService == null)
                    {
                        Console.WriteLine("_blockchainService is null.");
                    }
                    else
                    {
                        await _blockchainService.RewardSensor(sensorData.SensorId);
                        Console.WriteLine($"Sensor {sensorData.SensorId} rewarded.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error rewarding sensor: {ex}");
                }

            };


            Console.WriteLine("Connecting to MQTT broker...");
            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            Console.WriteLine("Connected to MQTT broker.");
            
            string topic = "DOTNET";
            Console.WriteLine($"Subscribing to topic: {topic}");
                
            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => f.WithTopic(topic).WithExactlyOnceQoS())
                .Build();

            await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            Console.WriteLine($"Subscribed to topic: {topic}");

            // Keep the application running to continue processing messages
            await Task.Delay(Timeout.Infinite);
        }
    }
}