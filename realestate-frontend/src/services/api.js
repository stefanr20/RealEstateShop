import axios from 'axios';

const API_URL = 'https://localhost:65148';

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Property endpoints
export const getProperties = () => api.get('/api/Property');
export const getProperty = (id) => api.get(`/api/Property/${id}`);
export const createProperty = (property) => api.post('/api/Property', property);
export const updateProperty = (property) => api.put('/api/Property', property);
export const deleteProperty = (id) => api.delete(`/api/Property/${id}`);

// Customer endpoints
export const getCustomers = () => api.get('/api/Customer');
export const getCustomer = (id) => api.get(`/api/Customer/${id}`);
export const createCustomer = (customer) => api.post('/api/Customer', customer);
export const updateCustomer = (customer) => api.put('/api/Customer', customer);
export const deleteCustomer = (id) => api.delete(`/api/Customer/${id}`);

export default api;