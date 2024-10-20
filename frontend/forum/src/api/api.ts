import axios, { type AxiosInstance } from "axios";

// Extend AxiosInstance to add custom methods
interface CustomAxiosInstance extends AxiosInstance {
    handleGet<T>(url: string, errorMessage: string): Promise<T>;
    handlePost<T>(url: string, data: object, errorMessage: string): Promise<T>;
    handlePut<T>(url: string, data: any, errorMessage: string): Promise<T>;
    handleDelete<T>(url: string, errorMessage: string): Promise<T>;
}

const api: CustomAxiosInstance = axios.create({
    baseURL: 'http://localhost:5135',
    headers: {
        'Content-Type': 'application/json',
    },
}) as CustomAxiosInstance;

// Helper function to handle GET requests
api.handleGet = async function <T>(url: string, errorMessage: string): Promise<T> {
    try {
        const response = await this.get<T>(url);
        return response.data;
    } catch (error) {
        console.error(errorMessage, error);
        throw error;
    }
};

// Helper function to handle POST requests
api.handlePost = async function <T>(url: string, data: object, errorMessage: string): Promise<T> {
    try {
        const response = await this.post<T>(url, JSON.stringify(data), {
            headers: { 'Content-Type': 'application/json' },
        });
        return response.data;
    } catch (error) {
        console.error(errorMessage, error);
        throw error;
    }
};

api.handlePut = async function <T>(url: string, data: any, errorMessage: string): Promise<T> {
    try {
        const response = await this.put<T>(url, typeof data === 'string' ? data : JSON.stringify(data), {
            headers: { 'Content-Type': 'application/json' },
        });
        return response.data;
    } catch (error) {
        console.error(errorMessage, error);
        throw error;
    }
};

// Helper function to handle DELETE requests
api.handleDelete = async function <T>(url: string, errorMessage: string): Promise<T> {
    try {
        const response = await this.delete<T>(url);
        return response.data;
    } catch (error) {
        console.error(errorMessage, error);
        throw error;
    }
};

export default api;
