import React, { useEffect, useState, useRef } from 'react';
import { Sparklines, SparklinesLine } from 'react-sparklines';

const getSensorType = (sensorId) => {
    if (sensorId >= 0 && sensorId <= 3) return 'Temperature';
    if (sensorId >= 4 && sensorId <= 7) return 'Humidity';
    if (sensorId >= 8 && sensorId <= 11) return 'Vibration';
    if (sensorId >= 12 && sensorId <= 15) return 'Pressure';
    return 'Unknown';
};

const getSensorUnit = (sensorType) => {
    switch (sensorType) {
        case 'Temperature':
            return '°C';
        case 'Humidity':
            return '%';
        case 'Vibration':
            return 'm/s²';
        case 'Pressure':
            return 'Pa';
        default:
            return '';
    }
};

const HomeComponent = () => {
    const [sensorData, setSensorData] = useState({});
    const socketRef = useRef(null);

    useEffect(() => {
        console.log('Mounting HomeComponent');

        if (!socketRef.current) {
            const protocol = window.location.protocol === 'https:' ? 'wss' : 'ws';
            const socketUrl = `${protocol}://${window.location.hostname}:8080/ws`;
            console.log(`Attempting to connect to ${socketUrl}`);
            socketRef.current = new WebSocket(socketUrl);

            socketRef.current.onopen = () => {
                console.log('WebSocket connection established.');
            };

            socketRef.current.onmessage = (event) => {
                console.log('Received message:', event.data);

                try {
                    const data = JSON.parse(event.data);

                    const transformedData = {
                        sensorId: data.SensorId,
                        data: data.Data,
                        dateTime: data.DateTime,
                    };

                    setSensorData(prevData => {
                        const prevSensorData = prevData[transformedData.sensorId]?.dataArray || [];
                        const updatedDataArray = [...prevSensorData, transformedData.data].slice(-10);

                        return {
                            ...prevData,
                            [transformedData.sensorId]: {
                                ...transformedData,
                                dataArray: updatedDataArray,
                            },
                        };
                    });
                } catch (error) {
                    console.error('Error parsing message:', error);
                }
            };

            socketRef.current.onerror = (error) => {
                console.error('WebSocket error:', error);
            };

            socketRef.current.onclose = (event) => {
                console.log('WebSocket connection closed:', event);
            };
        }

        return () => {
            console.log('Unmounting HomeComponent');
            if (socketRef.current && socketRef.current.readyState === WebSocket.OPEN) {
                socketRef.current.close();
            }
        };
    }, []);

    const sensorIds = Array.from({ length: 16 }, (_, i) => i);
    const sensorComponents = sensorIds.map(id => {
        const sensor = sensorData[id];

        const dataArray = sensor?.dataArray || [];
        const currentValue = sensor?.data !== undefined ? sensor.data.toFixed(2) : '-';
        const sensorType = getSensorType(id);

        return (
            <div key={id} style={sensorCardStyle}>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <h4>Sensor {id}</h4>
                    <span>
                        {currentValue !== '-' ? `${currentValue} ${getSensorUnit(sensorType)}` : '-'}
                    </span>
                </div>
                <div>
                    <Sparklines data={dataArray} limit={10}>
                        <SparklinesLine color={getSensorColor(sensorType)} />
                    </Sparklines>
                </div>
            </div>
        );
    });

    return (
        <div>
            <h2>Trains app</h2>
            <div style={gridContainerStyle}>
                {sensorComponents}
            </div>
        </div>
    );
};

const getSensorColor = (sensorType) => {
    const sensorTypeColors = {
        'Temperature': 'red',
        'Humidity': 'blue',
        'Vibration': 'green',
        'Pressure': 'orange',
    };
    return sensorTypeColors[sensorType] || 'gray';
};

const sensorCardStyle = {
    border: '1px solid #ccc',
    borderRadius: '5px',
    padding: '10px',
    margin: '5px',
    width: '200px',
};

const gridContainerStyle = {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'space-around',
};

export default HomeComponent;
