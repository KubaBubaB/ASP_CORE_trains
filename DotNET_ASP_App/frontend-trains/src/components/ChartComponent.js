import React from 'react';
import { Chart as ChartJS, CategoryScale, LinearScale, LineElement, PointElement, Title, Tooltip, Legend, TimeScale } from 'chart.js';
import { Line } from 'react-chartjs-2';
import 'chartjs-adapter-date-fns';

ChartJS.register(CategoryScale, LinearScale, LineElement, PointElement, Title, Tooltip, Legend, TimeScale);

const ChartComponent = ({ data }) => {
    const sensorTypeColors = {
        'Temperature': 'red',
        'Humidity': 'blue',
        'Vibration': 'green',
        'Pressure': 'orange',
    };

    const dataBySensorId = {};

    data.forEach(item => {
        const { sensorId, sensorType, dateTime, data: value } = item;
        if (!dataBySensorId[sensorId]) {
            dataBySensorId[sensorId] = {
                label: `Sensor ${sensorId}`,
                sensorType: sensorType,
                data: [],
            };
        }
        dataBySensorId[sensorId].data.push({ x: dateTime, y: value });
    });

    Object.values(dataBySensorId).forEach(sensorData => {
        sensorData.data.sort((a, b) => new Date(a.x) - new Date(b.x));
    });

    const datasets = Object.values(dataBySensorId).map(sensorData => {
        return {
            label: sensorData.label,
            data: sensorData.data,
            fill: false,
            borderColor: sensorTypeColors[sensorData.sensorType],
            tension: 0.1,
        };
    });

    const chartData = {
        datasets: datasets,
    };

    const options = {
        responsive: true,
        plugins: {
            legend: {
                display: true,
            },
            title: {
                display: true,
                text: 'Sensor Data Chart',
            },
            tooltip: {
                mode: 'index',
                intersect: false,
            },
        },
        scales: {
            x: {
                type: 'time',
                time: {
                    parser: 'yyyy-MM-dd HH:mm:ss',
                    tooltipFormat: 'yyyy-MM-dd HH:mm:ss',
                    displayFormats: {
                        minute: 'yyyy-MM-dd HH:mm',
                        hour: 'yyyy-MM-dd HH:mm',
                        day: 'yyyy-MM-dd',
                    },
                },
                title: {
                    display: true,
                    text: 'DateTime',
                },
            },
            y: {
                title: {
                    display: true,
                    text: 'Value',
                },
            },
        },
    };

    return (
        <div>
            <Line data={chartData} options={options} />
        </div>
    );
};

export default ChartComponent;
