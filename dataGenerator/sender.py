import time


class Sender:
    def __init__(self, sensors, publish):
        self.sensors = sensors
        self.is_running = False
        self.publish = publish

    def set_range(self, sensor_index, signals_per_minute, data_min, data_max):
        self.sensors[sensor_index].set_data_range(data_min, data_max, signals_per_minute)

    def stop(self):
        print("Generating stopped!")
        self.is_running = False

    def start(self):
        print("Generating started!")
        self.is_running = True
        while self.is_running:
            for sensor in self.sensors:
                if sensor.should_send_next_signal(time.time()):
                    self.publish(sensor.get_data())
                    sensor.time_of_last_signal = time.time()

