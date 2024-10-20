<script setup lang="ts">
import discussionService from '@/api/services/discussionService';
import sessionService from '@/api/services/sessionService';
import userService from '@/api/services/userService';
import type { FriendMessage, User } from '@/types/types';
import dateUtils from '@/utils/dateUtils';
import { onMounted, ref, watch } from 'vue';
import { useRoute, RouterLink } from 'vue-router';

const props = defineProps<{
    id: number
}>();

onMounted(async () => {
    await fetchMessages();
    await fetchFriend();
});

const route = useRoute();
watch(
    () => route.params.id,
    (newId) => {
        currentFriendId.value = Number(newId);
        fetchMessages();
        fetchFriend();
    }
);

const messages = ref<FriendMessage[]>([]);
const friend = ref<User>();
const newMessage = ref("");
const modify_post_id = ref();
const content_modify = ref("");
const currentFriendId = ref(props.id ?? 1);

const fetchMessages = async () => {
    try {
        const userId = sessionService.getIdUser();
        messages.value = await discussionService.getMessageFromFriendChat(currentFriendId.value, userId!);
    } catch (error) {
        console.error('Error fetching messages:', error);
    }
};

const fetchFriend = async () => {
    try {
        const friendId = props.id;
        friend.value = await userService.getUserById(friendId!);
    } catch (error) {
        console.error('Error fetching messages:', error);
    }
}

const sendNewMessage = async () => {
    await discussionService.createNewFriendChat({
        senderId: sessionService.getIdUser()!,
        receiverId: props.id,
        content: newMessage.value,
        date: dateUtils.formatCurrentDate(),
        state: 0
    });
    newMessage.value = "";
    fetchMessages();
};

const startModifyMessageContent = (index: number, content: string) => {
    modify_post_id.value = messages.value[index].id!;
    content_modify.value = content;
};

const updateMessageContent = async () => {
    await discussionService.modifyFriendMessage(modify_post_id.value, content_modify.value);
    modify_post_id.value = -1;
    fetchMessages();
};

const cancellMessageModification = () => {
    modify_post_id.value = -1;
    content_modify.value = "";
};

</script>

<template>
    <div class="h-full bg-gray-700">
        <h2 class="text-center text-2xl text-gray-200" v-if="messages.length == 0">Encore aucun message échangé avec {{
            friend?.pseudo }}</h2>
        <!-- Display messages -->
        <div v-for="(item, index) in messages" :key="index">
            <!-- Comments from other users -->
            <div v-if="item.senderId !== sessionService.getIdUser()">
                <article class="p-6 mb-3 ml-6 lg:ml-12 text-base bg-white rounded-lg dark:bg-gray-900">
                    <footer class="flex justify-between items-center mb-2">
                        <div class="flex items-center">
                            <RouterLink class="flex items-center" :to="`/profile/${item.senderId}`">
                                <img class="mr-2 w-6 h-6 rounded-full"
                                    src="https://media.istockphoto.com/id/2151669184/vector/vector-flat-illustration-in-grayscale-avatar-user-profile-person-icon-gender-neutral.jpg?s=612x612&w=0&k=20&c=UEa7oHoOL30ynvmJzSCIPrwwopJdfqzBs0q69ezQoM8="
                                    alt="pp placeholder" />
                                <p
                                    class="inline-flex items-center mr-3 text-sm text-gray-900 dark:text-white font-semibold">
                                    {{ friend?.pseudo }}</p>
                                <p class="text-sm text-gray-600 dark:text-gray-400">
                                    {{ dateUtils.timeAgo(item.date) }}
                                </p>
                                <p v-if="item.lastModified" class="text-xs text-gray-500 ml-3">
                                    (Dernière modification: {{ dateUtils.timeAgo(item.lastModified) }})
                                </p>
                            </RouterLink>
                        </div>
                    </footer>
                    <p class="text-gray-500 dark:text-gray-200">{{ item.content }}</p>
                </article>
            </div>

            <!-- Self-posted comments -->
            <div v-else>
                <article class="p-6 mb-3 ml-6 lg:ml-12 text-base bg-white rounded-lg dark:bg-gray-900">
                    <footer class="flex justify-between items-center mb-2">
                        <div class="flex items-center space-x-2">
                            <!-- Edit Icon -->
                            <button @click="startModifyMessageContent(index, item.content)"
                                class="p-1 rounded-md hover:bg-gray-200 dark:hover:bg-gray-800">
                                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-500"
                                    viewBox="0 0 20 20" fill="currentColor">
                                    <path
                                        d="M17.414 2.586a2 2 0 00-2.828 0l-1.828 1.828a2 2 0 000 2.828l5 5a2 2 0 002.828 0l1.828-1.828a2 2 0 000-2.828l-5-5zM6.828 9.172L3 16v3h3l6.828-6.828-6-6z" />
                                </svg>
                            </button>
                        </div>
                        <div class="flex items-center">
                            <p v-if="item.lastModified" class="text-xs text-gray-500 mr-3">
                                (Dernière modification: {{ dateUtils.timeAgo(item.lastModified) }})
                            </p>
                            <p class="text-sm text-gray-600 dark:text-gray-400">
                                <time pubdate datetime="2022-02-12" title="February 12th, 2022">{{
                                    dateUtils.timeAgo(item.date) }}</time>
                            </p>
                            <p
                                class="inline-flex items-center ml-3 text-sm text-gray-900 dark:text-white font-semibold">
                                Vous
                                <img class="ml-2 w-6 h-6 rounded-full"
                                    src="https://media.istockphoto.com/id/2151669184/vector/vector-flat-illustration-in-grayscale-avatar-user-profile-person-icon-gender-neutral.jpg?s=612x612&w=0&k=20&c=UEa7oHoOL30ynvmJzSCIPrwwopJdfqzBs0q69ezQoM8="
                                    alt="pp placeholder" />
                            </p>
                        </div>
                    </footer>

                    <!-- Message Modification Field -->
                    <div v-if="modify_post_id === item.id">
                        <textarea v-model="content_modify" class="w-full p-2 border rounded-md"
                            placeholder="Edit your message..."></textarea>
                        <button @click="updateMessageContent"
                            class="px-4 py-2 mt-2 text-white bg-blue-500 rounded-md hover:bg-blue-600">Modifier</button>
                        <button @click="cancellMessageModification"
                            class="px-4 py-2 mt-2 text-white bg-red-500 rounded-md hover:bg-red-600">Annuler</button>
                    </div>

                    <!-- Comment content -->
                    <p v-else class="text-gray-500 dark:text-gray-200 text-right">{{ item.content }}</p>
                </article>
            </div>
        </div>

        <!-- New Message Input -->
        <div v-if="sessionService.logged()" class="mt-4 w-[50%] mx-auto flex flex-col items-center">
            <textarea v-model="newMessage" placeholder="Ecrivez votre message..."
                class="w-full p-3 border rounded-lg"></textarea>
            <button @click="sendNewMessage" class="mt-2 px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600">
                Envoyer
            </button>
        </div>
    </div>
</template>
