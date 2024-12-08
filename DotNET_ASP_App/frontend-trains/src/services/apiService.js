import axios from 'axios';
import { format } from 'date-fns';

const API_BASE_URL = `${window.location.protocol}//${window.location.hostname}:8080/api`;

export const getData = async (filters) => {
    try {
        const params = {};

        if (filters.sensorType) {
            params.sensorType = filters.sensorType.value;
        }

        if (filters.sensorId) {
            params.sensorId = filters.sensorId.value;
        }

        if (filters.startDate) {
            params.startDate = format(filters.startDate, 'yyyy-MM-dd HH:mm:ss');
        }
        if (filters.endDate) {
            params.endDate = format(filters.endDate, 'yyyy-MM-dd HH:mm:ss');
        }

        if (filters.sortBy) {
            params.sortBy = filters.sortBy;
        }
        if (filters.sortOrder) {
            params.sortOrder = filters.sortOrder;
        }

        Object.keys(params).forEach(key => {
            if (params[key] === undefined || params[key] === null || params[key].length === 0) {
                delete params[key];
            }
        });

        console.log('Request Params:', params);

        const response = await axios.get(`${API_BASE_URL}/data`, { params });
        return response.data;
    } catch (error) {
        console.error("Error fetching data:", error);
        throw error;
    }
};

export const downloadData = async (filters, format) => {
    try {
        const response = await axios.get(`${API_BASE_URL}/data`, {
            params: { ...filters, dataType: format },
            responseType: 'blob',
        });
        return response;
    } catch (error) {
        console.error("Error downloading data:", error);
        throw error;
    }
};
