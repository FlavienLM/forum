import api from "@/api/api";
import type { FriendRequest, User } from "@/types/types";
import dateUtils from "@/utils/dateUtils";
import type { FriendSidebarItem  } from '../../types/types';

const userService = {
    // Create a new user
    async createUser(user: User): Promise<any> {
        try {
            return await api.handlePost("/user", user, "Error creating user");
        } catch (error) {
            console.error('Error in createUser:', error);
            throw error;
        }
    },

    // Fetch the user list
    async getUserList(): Promise<User[]> {
        try {
            return await api.handleGet("/user", "Error fetching user list");
        } catch (error) {
            console.error('Error in getUserList:', error);
            throw error;
        }
    },

    async getUserById(userId: number): Promise<User> {
        try {
            return await api.handleGet(`user/${userId}`, "Error fetching user list");
        } catch (error) {
            console.error('Error in getUserById:', error);
            throw error;
        }
    },

    async getFriendsOfUser(userId: number): Promise<FriendSidebarItem []> {
        try {
            return await api.handleGet(`friend/${userId}`, "Error fetching user list");
        } catch (error) {
            console.error('Error in getFriendsOfUser:', error);
            throw error;
        }
    },
    

    async getFriendWithWhomUserLastInteracted(userId: number): Promise<FriendSidebarItem []> {
        return await api.handleGet(`/user/friend/${userId}`, "Error getting friend with whom giver user last interacted");
    },

    async sendFriendRequest(request: FriendRequest): Promise<any> {
        try {
            return await api.handlePost("/friend", request, "Error creating new friend request");
        } catch (error) {
            console.error('Error in sendFriendRequest:', error);
            throw error;
        }
    },

    async acceptFriendRequest(id: number): Promise<any> {
        const accepte = {
            id: id,
            date: dateUtils.formatCurrentDate(),
        };
        console.info(accepte);
        try {
            return await api.handlePost(`/friend/accept`, accepte, "Error accepting friend request");
        } catch (error) {
            console.error('Error in acceptFriendRequest:', error);
            throw error;
        }
    },

    async refuseFriendRequest(id: number): Promise<any> {
        const refuse = {
            id: id,
            date: dateUtils.formatCurrentDate(),
        };
        try {
            return await api.handlePost(`/friend/decline`, refuse, "Error declining friend request");
        } catch (error) {
            console.error('Error in refuseFriendRequest:', error);
            throw error;
        }
    },

    async updateUser(user: User): Promise<any> {
        try {
            return await api.handlePut(`/user`, user, "Error Updating user info");
        } catch (error) {
            console.error('Error in updateUser:', error);
            throw error;
        }
    },

    async getFriendRequestStatus(user1Id: number, user2Id: number): Promise<any> {
        try {
            return await api.handleGet(`/friend/${user1Id}/${user2Id}`, "Error getting status of friendRequest");
        } catch (error) {
            console.error('Error in getFriendRequestStatus:', error);
            throw error;
        }
    },


};

export default userService;
