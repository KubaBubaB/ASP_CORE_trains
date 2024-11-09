namespace DotNET_ASP_App.Sensors;

public class Sensor
{
    public Guid Id { get; set; }
    public string SensorType { get; set; }
    public int SensorId { get; set; }
    public double Data { get; set; }
    public DateTime DateTime { get; set; }
}