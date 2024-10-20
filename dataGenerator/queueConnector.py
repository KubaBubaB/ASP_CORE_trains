import paho.mqtt.client as paho

broker_address = 'localhost'
port = 1883
topic = "DOTNET"
client_id = 'pendolino-123'
login = 'user1'
password = 'password1'
class QueueConnector:
    def __init__(self):
        self.client = None

    def connect_mqtt(self):
        def on_connect(client, userdata, flags, reason_code, properties):
            if reason_code == 0:
                print("Connected to MQTT Broker!")
            else:
                print(f"Failed to connect, return code {reason_code}\n")

        self.client = paho.Client(client_id=client_id,
                             transport="tcp",
                             protocol=paho.MQTTv5)
        self.client.username_pw_set(login, password)
        self.client.on_connect = on_connect
        self.client.connect(broker_address, port)

    def publish(self, msg):
        result = self.client.publish(topic, str(msg))  # Convert msg to string before publishing
        status = result[0]
        if status == 0:
            print(f"Sent `{msg}` to topic `{topic}`")
        else:
            print(f"Failed to send message to topic {topic}")

