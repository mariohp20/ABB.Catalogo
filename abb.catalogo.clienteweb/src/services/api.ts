import axios, { type InternalAxiosRequestConfig } from 'axios';

// 1. Creamos una instancia base de Axios apuntando a nuestro Backend
const api = axios.create({
    baseURL: 'https://localhost:7247/api',
    headers: {
        'Content-Type': 'application/json'
    }
});

// 2. Configuramos el Interceptor de Peticiones (Request Interceptor)
api.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        // Buscamos el token en el almacenamiento local del navegador
        const token = localStorage.getItem('token');

        // Si el token existe y tenemos cabeceras, lo inyectamos
        if (token && config.headers) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default api;