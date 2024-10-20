export interface User {
    id?: number;
    accountId: string;
    pseudo: string;
    mail: string;
    profile?: string;
    password?: string;
    date?: string;
}

export interface Discussion {
    id?: number,
    title: string,
    description: string,
    creatorId: number,
    creatorIdentifiant?: string
    date?: string,
    commentCount?: number
}

export interface Message {
    id?:number,
    authorId: number,
    pseudo?: string,
    discussionId: number,
    content: string;
    date: string,
    state?: number,
    lastModified?: string
}

export interface FriendMessage {
    id?: number,
    senderId: number,
    receiverId: number,
    content: string,
    state: 0,
    date: string,
    lastModified?: string
}


export interface Notification {
    id: number,
    date: string,
    notificationType: number,
    isRead: boolean,
    name: string,
    referenceId: string,
    referenceString: string,
    friendRequestId: number
}

export interface DiscussionSidebarItem  {
    id: any;
    title: string,
    lastMessage: string
    date: string
}

export interface FriendSidebarItem  {
    friendId:number
    pseudo: string,
    accountId: string,
    lastMessage: string,
    date: string
}



export interface FriendRequest {
    senderId: number,
    receiverId: number,
    date: string,
}