using System.Buffers;
using System.Text;
using System.Text.Json;
using DotNET_ASP_App.DTOs;
using DotNET_ASP_App.Repository;
using DotNET_ASP_App.Service;
using DotNET_ASP_App.WebSocket;
using MQTTnet;

namespace DotNET_ASP_App.Queue;

public class MQTTRewardsHandler
{
    private readonly BlockchainService _blockchainService;
    
    public MQTTRewardsHandler(BlockchainService blockchainService)
    {
        _blockchainService = blockchainService ?? throw new ArgumentNullException(nameof(blockchainService));
    }
    
    public async Task Handle_Received_Application_Message()
    {
        var mqttFactory = new MqttClientFactory();

        using (var mqttClient = mqttFactory.CreateMqttClient())
        {
            int portOfBroker = 1883;
            var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("mqtt5", portOfBroker).Build();
            
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
                
                var sensorId = JsonSerializer.Deserialize<int>(payloadString);
                
                try
                {
                    Console.WriteLine("Attempting to reward sensor.");
                    if (_blockchainService == null)
                    {
                        Console.WriteLine("_blockchainService is null.");
                    }
                    else
                    {
                        await _blockchainService.RewardSensor(sensorId);
                        Console.WriteLine($"Sensor {sensorId} rewarded.");
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
            
            string topic = "REWARDS";
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