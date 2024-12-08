import React, { useState, useEffect } from 'react';
import DatePicker from 'react-datepicker';
import Select from 'react-select';
import "react-datepicker/dist/react-datepicker.css";

const FilterComponent = ({ onFilterChange, initialFilters }) => {
    const [filters, setFilters] = useState(initialFilters || {
        sensorType: null,
        sensorId: null,
        startDate: new Date(Date.now() - 60000),
        endDate: null,
        sortBy: "",
        sortOrder: "",
    });

    const sensorTypeOptions = [
        { value: 'Pressure', label: 'Pressure' },
        { value: 'Vibration', label: 'Vibration' },
        { value: 'Humidity', label: 'Humidity' },
        { value: 'Temperature', label: 'Temperature' },
    ];

    const sensorIdOptions = Array.from({ length: 16 }, (_, i) => ({
        value: i,
        label: `Sensor ${i}`,
    }));

    const handleSensorTypeChange = (selectedOption) => {
        setFilters({ ...filters, sensorType: selectedOption });
    };

    const handleSensorIdChange = (selectedOption) => {
        setFilters({ ...filters, sensorId: selectedOption });
    };

    const handleDateChange = (name, date) => {
        setFilters({ ...filters, [name]: date });
    };

    const handleSortChange = (e) => {
        const { name, value } = e.target;
        setFilters({ ...filters, [name]: value });
    };

    const handleApply = () => {
        onFilterChange(filters);
    };

    useEffect(() => {
        const oneMinuteAgo = new Date(Date.now() - 60000);
        setFilters(prevFilters => ({ ...prevFilters, startDate: oneMinuteAgo }));
    }, []);

    return (
        <div style={containerStyle}>
            <div style={rowStyle}>
                <label style={labelStyle}>Sensor Type:</label>
                <Select
                    options={sensorTypeOptions}
                    value={filters.sensorType}
                    onChange={handleSensorTypeChange}
                    placeholder="Select Sensor Type"
                    isClearable
                />
            </div>
            <div style={rowStyle}>
                <label style={labelStyle}>Sensor ID:</label>
                <Select
                    options={sensorIdOptions}
                    value={filters.sensorId}
                    onChange={handleSensorIdChange}
                    placeholder="Select Sensor ID"
                    isClearable
                />
            </div>
            <div style={rowStyle}>
                <label style={labelStyle}>Start Date:</label>
                <DatePicker
                    selected={filters.startDate}
                    onChange={(date) => handleDateChange("startDate", date)}
                    placeholderText="Start Date"
                    showTimeSelect
                    timeFormat="HH:mm:ss"
                    dateFormat="yyyy-MM-dd HH:mm:ss"
                />
            </div>
            <div style={rowStyle}>
                <label style={labelStyle}>End Date:</label>
                <DatePicker
                    selected={filters.endDate}
                    onChange={(date) => handleDateChange("endDate", date)}
                    placeholderText="End Date"
                    showTimeSelect
                    timeFormat="HH:mm:ss"
                    dateFormat="yyyy-MM-dd HH:mm:ss"
                />
            </div>
            <div style={rowStyle}>
                <label style={labelStyle}>Sort By:</label>
                <select
                    name="sortBy"
                    value={filters.sortBy}
                    onChange={handleSortChange}
                    style={selectStyle}
                >
                    <option value="">Select Sort By</option>
                    <option value="Data">Data</option>
                    <option value="DateTime">DateTime</option>
                    <option value="SensorId">Sensor ID</option>
                    <option value="SensorType">Sensor Type</option>
                </select>
            </div>
            <div style={rowStyle}>
                <label style={labelStyle}>Sort Order:</label>
                <select
                    name="sortOrder"
                    value={filters.sortOrder}
                    onChange={handleSortChange}
                    style={selectStyle}
                >
                    <option value="">Select Sort Order</option>
                    <option value="asc">Ascending</option>
                    <option value="desc">Descending</option>
                </select>
            </div>
            <button onClick={handleApply}>Apply Filters</button>
        </div>
    );
};

const containerStyle = {
    display: 'flex',
    flexDirection: 'column',
    gap: '10px',
    maxWidth: '400px',
    margin: '0 auto',
    padding: '20px',
    backgroundColor: '#fff',
};

const rowStyle = {
    display: 'flex',
    flexDirection: 'column',
    gap: '5px',
};

const labelStyle = {
    fontWeight: 'bold',
    fontSize: '14px',
};

const selectStyle = {
    padding: '8px',
    borderRadius: '4px',
    border: '1px solid #ccc',
    fontSize: '14px',
};

export default FilterComponent;
