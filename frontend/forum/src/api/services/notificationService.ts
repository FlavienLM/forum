import api from "@/api/api";
import type { Notification, User } from "@/types/types";
import sessionService from "./sessionService";

const notificationService = {

    async createNotification(user: User): Promise<any> {
        const newUser = {
            pseudo: user.pseudo,
            mail: user.mail,
            password: user.password
        };
        try {
            return await api.handlePost("/user", newUser, "Error creating user");
        } catch (error) {
            console.error('Error in createUser:', error);
            throw error;
        }
    },

    async getNotificationOfUser(): Promise<Notification[]> {
        try {
            const id = sessionService.getIdUser();
            return await api.handleGet(`/notification/discussion/${id}`, "Error fetching user list");
        } catch (error) {
            console.error('Error in getUserList:', error);
            throw error;
        }
    },

    async changeStateNotification(id: number):Promise<any>  {
        try {
            const response = await api.put("/notification", id, {
                headers: { 'Content-Type': 'application/json' },
            });
            return response.data;
        } catch (error) {
            console.info(error);
            throw error;
        }
    },

    async markAllNotificationOfUserAsRead(id:number): Promise<any> {
        try {
            const response = await api.put(`/notification/user/${id}`);
            return response.data;
        } catch (error) {
            console.info(error);
            throw error;
        }
    }

};

export default notificationService;
