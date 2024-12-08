import React, { useEffect, useState, useRef } from 'react';

const LiveComponent = () => {
    const [sensorData, setSensorData] = useState({});
    const [meanData, setMeanData] = useState({});
    const socketRef = useRef(null);

    useEffect(() => {
        console.log('Mounting LiveComponent');

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
                        data: parseFloat(data.Data).toFixed(2),
                        dateTime: data.DateTime.replace('T', ' ').slice(0, 19),
                        mean: data.Mean !== undefined ? parseFloat(data.Mean).toFixed(2) : undefined,
                    };

                    setSensorData(prevData => ({
                        ...prevData,
                        [transformedData.sensorId]: transformedData,
                    }));

                    if (transformedData.mean !== undefined) {
                        setMeanData(prevMeanData => ({
                            ...prevMeanData,
                            [transformedData.sensorId]: transformedData.mean,
                        }));
                    }
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
            console.log('Unmounting LiveComponent');
            if (socketRef.current && socketRef.current.readyState === WebSocket.OPEN) {
                socketRef.current.close();
            }
        };
    }, []);

    const sensorIds = Array.from({ length: 16 }, (_, i) => i);

    const tableData = sensorIds.map(id => sensorData[id] || { sensorId: id, data: '-', dateTime: '-' });
    const meanTableData = sensorIds.map(id => ({
        sensorId: id,
        mean: meanData[id] !== undefined ? meanData[id] : '-',
    }));

    const tableStyle = {
        width: '100%',
        border: '1px solid black',
        borderCollapse: 'collapse',
        textAlign: 'center',
    };

    const thTdStyle = {
        border: '1px solid black',
        padding: '8px',
    };

    return (
        <div>
            <h2>Live Sensor Data</h2>
            <table style={tableStyle}>
                <thead>
                    <tr>
                        <th style={thTdStyle}>Sensor ID</th>
                        <th style={thTdStyle}>Value</th>
                        <th style={thTdStyle}>DateTime</th>
                    </tr>
                </thead>
                <tbody>
                    {tableData.map(sensor => (
                        <tr key={sensor.sensorId}>
                            <td style={thTdStyle}>{sensor.sensorId}</td>
                            <td style={thTdStyle}>{sensor.data}</td>
                            <td style={thTdStyle}>{sensor.dateTime}</td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <h2>Average Sensor Data</h2>
            <table style={tableStyle}>
                <thead>
                    <tr>
                        <th style={thTdStyle}>Sensor ID</th>
                        <th style={thTdStyle}>Mean Value</th>
                    </tr>
                </thead>
                <tbody>
                    {meanTableData.map(sensor => (
                        <tr key={sensor.sensorId}>
                            <td style={thTdStyle}>{sensor.sensorId}</td>
                            <td style={thTdStyle}>{sensor.mean}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default LiveComponent;
