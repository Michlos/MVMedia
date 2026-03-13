import axios from 'axios';

const api = axios.create({
    baseURL: 'http://mvmediaapi.railway.internal:5069', // Base URL da sua API
});

//const API_BASE_URL = 'http://mvmediaapi.railway.internal:5069'; // Base URL da sua API
//adiona um ternrceptador de requisição;
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token){
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) =>{
        return Promise.reject(error);
    }
);

export default api;
export const baseURL = 'http://mvmediaapi.railway.internal:5069'; // Base URL da sua API