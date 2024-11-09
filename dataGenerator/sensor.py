import random
from datetime import datetime


class Sensor:
    def __init__(self, id, sensor_type, data_min, data_max, signals_per_minute):
        self.id = id
        self.sensor_type = sensor_type
        self.data_min = data_min
        self.data_max = data_max
        self.time_of_last_signal = 0
        self.signals_per_minute = signals_per_minute

    def set_data_range(self, data_min, data_max, signals_per_minute):
        self.data_min = data_min
        self.data_max = data_max
        self.signals_per_minute = signals_per_minute

    def should_send_next_signal(self, ttime):
        return self.time_of_last_signal < ttime - (60 / self.signals_per_minute)

    def get_data(self):
        numeric_data = random.uniform(self.data_min, self.data_max)
        return {
            "SensorType": str(self.sensor_type),
            "SensorId": self.id,
            "Data": numeric_data,
            "DateTime": datetime.now().isoformat()
        }
