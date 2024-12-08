using System.Buffers;
using System.Text;
using System.Text.Json;
using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.Repository;
using DotNET_ASP_App.Service;
using DotNET_ASP_App.WebSocket;
using MQTTnet;
using MQTTnet.Protocol;

namespace DotNET_ASP_App.Queue;

public class MQTTSensorsHandler
{
    private readonly NotificationService _notificationService;
    
    public MQTTSensorsHandler(NotificationService notificationService)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
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
                
                var newPayload = JsonSerializer.Serialize(sensorData.SensorId);
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("REWARDS")
                    .WithPayload(newPayload)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                    .Build();

                await mqttClient.PublishAsync(message);
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