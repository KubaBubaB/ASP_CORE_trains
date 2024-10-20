using System.Text;
using System.Text.Json;
using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.Repository;
using MQTTnet;

namespace DotNET_ASP_App.Queue;

public class MQTTHandler
{
    public static async Task Handle_Received_Application_Message()
    {
        var mqttFactory = new MqttClientFactory();

        using (var mqttClient = mqttFactory.CreateMqttClient())
        {
            int portOfBroker = 1883;
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("localhost", portOfBroker).Build();
            
            // Handler for received application messages
            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                Console.WriteLine("Received application message.");
                try
                {
                    var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload).Replace('\'', '\"');
                    var sensorData = JsonSerializer.Deserialize<SensorDTO>(payload);

                    if (sensorData != null)
                    {
                        MongoRepo.GetInstance().SaveSensorData(sensorData);
                    }
                    else
                    {
                        Console.WriteLine("Failed to deserialize payload.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deserializing payload: {ex.Message}");
                }

                return Task.CompletedTask;
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