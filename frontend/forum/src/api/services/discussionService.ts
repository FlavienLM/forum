import api from "@/api/api";
import type { Discussion, DiscussionSidebarItem, FriendMessage, Message } from "@/types/types";

const discussionService = {

    async createDiscussion(discussion: Discussion) {
        return await api.handlePost("/discussion", discussion, "Error creating discussion");
    },

    async getAllDiscussions(): Promise<Discussion[]> {
        return await api.handleGet("/discussion", "Error fetching all discussions");
    },
    async getDiscussionById(discussionId: number): Promise<Discussion> {
        return await api.handleGet(`/discussion/single/${discussionId}`, "Error fetching discussion by id");
    },

    async getDiscussionCreatedByUser(userId: number): Promise<Discussion[]> {
        return await api.handleGet(`/discussion/${userId}`, "Error fetching discussions by given user");
    },

    async getMessageOfDiscussion(id: number): Promise<Message[]> {
        return (await api.handleGet(`/discussion/message/${id}`, "Error fetching message of given discussion"));
    },

    async createNewMessageInDiscussion(message: Message) {
        console.info(message);
        return await api.handlePost("/discussion/message", message, "Error creating new message in discussion");
    },

    async modifyMessage(id: number, newContent: string) {
        return await api.handlePut(`/discussion/message/${id}`, newContent, "Error modifying message of discussion");
    },

    async modifyFriendMessage(id: number, newContent: string) {
        return await api.handlePut(`/discussion/friendChat/${id}`, newContent, "Error modifying message between friends");
    },

    async deleteMessage(id: number) {
        return await api.handleDelete(`/discussion/message/${id}`, "Error deleting message");
    },

    async getDiscussionWhereUserLastParticipated(userId: number): Promise<DiscussionSidebarItem[]> {
        return await api.handleGet(`discussion/user/${userId}`, "Error getting discussion where user last parcitipated");
    },

    async getMessageFromFriendChat(user1: number, user2: number): Promise<FriendMessage[]> {
        return await api.handleGet(`discussion/friendChat/${user1}/${user2}`, "Error getting list of messages between friends");
    },

    async createNewFriendChat(message: FriendMessage) {
        return await api.handlePost(`discussion/friendChat`, message, "Error creating new message between friends");
    },
    async modifyDiscussionDescription(discussionId: number, newDescription: string) {
        return await api.handlePut(`discussion/description/${discussionId}`, newDescription, "Error modifying description of discussion");
    },
};

export default discussionService;
