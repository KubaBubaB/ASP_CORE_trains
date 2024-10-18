import random
import time

from queueConnector import QueueConnector
from sender import Sender
from sensor import Sensor

class GeneratorController:
    def __init__(self):
        def create_sensors():
            sensors = []
            sensors.append(Sensor(0, "Temperature", 0, 10, 60))
            sensors.append(Sensor(1, "Temperature", 0, 10, 60))
            sensors.append(Sensor(2, "Temperature", 0, 10, 60))
            sensors.append(Sensor(3, "Temperature", 0, 10, 60))
            sensors.append(Sensor(0, "Humidity", 0, 10, 60))
            sensors.append(Sensor(1, "Humidity", 0, 10, 60))
            sensors.append(Sensor(2, "Humidity", 0, 10, 60))
            sensors.append(Sensor(3, "Humidity", 0, 10, 60))
            sensors.append(Sensor(0, "Vibration", 0, 10, 60))
            sensors.append(Sensor(1, "Vibration", 0, 10, 60))
            sensors.append(Sensor(2, "Vibration", 0, 10, 60))
            sensors.append(Sensor(3, "Vibration", 0, 10, 60))
            sensors.append(Sensor(0, "Pressure", 0, 10, 60))
            sensors.append(Sensor(1, "Pressure", 0, 10, 60))
            sensors.append(Sensor(2, "Pressure", 0, 10, 60))
            sensors.append(Sensor(3, "Pressure", 0, 10, 60))

            return sensors

        queue_connector = QueueConnector()
        queue_connector.connect_mqtt()
        self.sensors = create_sensors()

        self.sender = Sender(self.sensors, queue_connector.publish)

    def stop_generating(self):
        self.sender.stop()

    def start_generating(self):
        self.sender.start()

    def update_sensors_data(self, data):
        i = 0
        for sensor in self.sensors:
            sensor.data_min = int(data[i * 3])
            sensor.data_max = int(data[i * 3 + 1])
            sensor.signals_per_minute = int(data[i * 3 + 2])
            i += 1

    def is_generating(self):
        return self.sender.is_running