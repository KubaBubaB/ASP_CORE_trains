import threading
import tkinter as tk
from generatorController import GeneratorController

sensor_types = ['Temperature', 'Humidity', 'Vibration', 'Pressure']
sensor_parameters_tags = ['Min', 'Max', 'SPM']


def get_sensors_data(sensors):
    data = []
    for sensor in sensors:
        data.append(sensor.data_min)
        data.append(sensor.data_max)
        data.append(sensor.signals_per_minute)
    return data


if __name__ == '__main__':
    generator = GeneratorController()

    # GUI
    default_values = get_sensors_data(generator.sensors)
    root = tk.Tk()
    root.title("Train sensors generator")
    frame = tk.Frame(root)
    frame.pack(padx=10, pady=10)

    input_fields = []

    for i in range(48):
        row = i // 3
        column = i % 3

        if row % 4 == 0:
            test_label = tk.Label(frame, text=sensor_types[int(row/4)])
            test_label.grid(row=row * 2, column=0, columnspan=6, padx=5, pady=5,
                            sticky='ew')

        label_name = str(row % 4) + ': ' + sensor_parameters_tags[column]
        label = tk.Label(frame, text=label_name)
        label.grid(row=row * 2 + 1, column=column * 2, padx=5, pady=5, sticky='e')

        entry = tk.Entry(frame)
        entry.grid(row=row * 2 + 1, column=column * 2 + 1, padx=5, pady=5)
        entry.insert(0, default_values[i])
        input_fields.append(entry)


    def on_submit():
        values = [entry.get() for entry in input_fields]
        generator.update_sensors_data(values)

    def on_stop():
        if generator.is_generating():
            generator.stop_generating()

    def on_start():
        if not generator.is_generating():
            generator_thread = threading.Thread(target=generator.start_generating)
            generator_thread.start()


    submit_button = tk.Button(frame, text="Submit", command=on_submit)
    submit_button.grid(row=32, column=0, columnspan=2, pady=20)
    submit_button = tk.Button(frame, text="Start", command=on_start)
    submit_button.grid(row=32, column=2, columnspan=2, pady=20)
    submit_button = tk.Button(frame, text="Stop", command=on_stop)
    submit_button.grid(row=32, column=4, columnspan=2, pady=20)

    root.mainloop()
