import React, { useState, useEffect } from 'react';
import { getData, downloadData } from '../services/apiService';
import TableComponent from './TableComponent';
import FilterComponent from './FilterComponent';
import ChartComponent from './ChartComponent';

const DetailsComponent = () => {
    const [data, setData] = useState([]);
    const [filters, setFilters] = useState({
        startDate: new Date(Date.now() - 60000),
        endDate: null,
        sensorType: null,
        sensorId: null,
        sortBy: "",
        sortOrder: "",
    });

    const fetchData = async () => {
        try {
            const result = await getData(filters);
            setData(result.sensors || []);
        } catch (error) {
            console.error(error);
        }
    };

    const handleFilterChange = (newFilters) => {
        setFilters(newFilters);
        fetchData();
    };

    const handleDownload = async (format) => {
        try {
            const response = await downloadData(filters, format);
            const blob = new Blob([response.data], { type: format === 'csv' ? 'text/csv' : 'application/json' });
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `data.${format}`;
            a.click();
        } catch (error) {
            console.error(error);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    const columns = [
        { Header: 'Sensor ID', accessor: 'sensorId' },
        { Header: 'Sensor Type', accessor: 'sensorType' },
        { Header: 'Timestamp', accessor: 'dateTime' },
        { Header: 'Value', accessor: 'data' },
    ];

    return (
        <div>
            <h2>Details</h2>
            <FilterComponent onFilterChange={handleFilterChange} initialFilters={filters} />
            <button onClick={() => handleDownload('csv')}>Download CSV</button>
            <button onClick={() => handleDownload('json')}>Download JSON</button>
            <ChartComponent data={data} />
            <TableComponent columns={columns} data={data} />
        </div>
    );
};

export default DetailsComponent;
