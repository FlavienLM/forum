<script setup lang="ts">
import sessionService from '@/api/services/sessionService';
import userService from '@/api/services/userService';
import type { Discussion, FriendRequest, User } from '@/types/types';
import dateUtils from '@/utils/dateUtils';
import { onMounted, ref, computed, watch } from 'vue';
import discussionService from '@/api/services/discussionService';
import { useRoute } from 'vue-router';

// Define props to receive the user's ID
const props = defineProps<{
    id: number
}>();

onMounted(async () => {
    currentUserId.value = props.id
    await fetchUserInfo();
});

const route = useRoute();

watch(
    () => route.params.id,
    async (newId) => {
        const parsedId = Number(newId);
        if (!isNaN(parsedId)) {
            currentUserId.value = parsedId;
            await fetchUserInfo();
        }
    }
);

const userProfile = ref<User>(); 
const editablePseudo = ref<string>(''); 
const editableProfile = ref<string>(''); 
const isEditing = ref<boolean>(false); 
const userDiscussions = ref<Discussion[]>([]);
const sortMethod = ref<string>('date'); 
const friendRequestStatus = ref<string>(''); 
const currentUserId = ref<number>();


const sendFriendRequest = async () => {
    const friendRequest: FriendRequest = {
        senderId: sessionService.getIdUser()!,
        receiverId: currentUserId.value!,
        date: dateUtils.formatCurrentDate(),
    };

    await userService.sendFriendRequest(friendRequest);
    friendRequestStatus.value = 'Pending';
};

// Function to fetch user info and discussions
const fetchUserInfo = async () => {
    userProfile.value = await userService.getUserById(currentUserId.value!); // Fetch user data
    userDiscussions.value = await discussionService.getDiscussionCreatedByUser(currentUserId.value!); // Fetch discussions

    // Set default editable values
    editablePseudo.value = userProfile.value.pseudo;
    editableProfile.value = userProfile.value.profile!; // Profile description

    // Check the status of the friend request
    if(sessionService.logged()){
        friendRequestStatus.value = await userService.getFriendRequestStatus(sessionService.getIdUser()!, currentUserId.value!);
    }
};

const saveUserProfile = async () => {
    if (!userProfile.value) return;
    userProfile.value.pseudo = editablePseudo.value;
    userProfile.value.profile = editableProfile.value;

    await userService.updateUser(userProfile.value); // Update user
    isEditing.value = false; // Exit edit mode
};

// Computed property to sort discussions
const sortedDiscussions = computed(() => {
    if (sortMethod.value === 'date') {
        return [...userDiscussions.value].sort((a, b) => new Date(b.date!).getTime() - new Date(a.date!).getTime());
    } else if (sortMethod.value === 'alphabetical') {
        return [...userDiscussions.value].sort((a, b) => a.title.localeCompare(b.title));
    }
    return userDiscussions.value; // Return unsorted if no method is specified
});

</script>

<template>
    <div class="bg-gray-700 h-full">
        <div class="mx-auto rounded shadow-xl w-[90%] md:w-1/2 overflow-hidden">
            <div class="h-[140px] bg-gradient-to-r from-cyan-500 to-blue-500"></div>
            <div class="px-5 py-2 flex flex-col gap-3 pb-6">
                <div class="h-[90px] shadow-md w-[90px] rounded-full border-5 overflow-hidden -mt-14 border-white">
                    <img class="mr-2 w-full h-full "
                        src="https://media.istockphoto.com/id/2151669184/vector/vector-flat-illustration-in-grayscale-avatar-user-profile-person-icon-gender-neutral.jpg?s=612x612&w=0&k=20&c=UEa7oHoOL30ynvmJzSCIPrwwopJdfqzBs0q69ezQoM8="
                        alt="pp placeholder" />
                </div>

                <div>
                    <h3 v-if="!isEditing" class="text-xl text-gray-200 font-bold leading-6">{{ userProfile?.pseudo }}
                    </h3>
                    <input v-else v-model="editablePseudo" class="border rounded px-2 py-1" />

                    <p class="text-sm text-gray-300">@{{ userProfile?.accountId }}</p>
                </div>

                <div class="flex gap-2">
                    <div v-if="userProfile?.id != sessionService.getIdUser()">
                        <button v-if="friendRequestStatus === 'none'" @click="sendFriendRequest"
                            class="inline-flex w-auto cursor-pointer select-none appearance-none items-center justify-center space-x-1 rounded border border-gray-200 bg-white px-3 py-2 text-sm font-medium text-gray-800 transition hover:border-gray-300 active:bg-white hover:bg-gray-100 focus:border-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-300">
                            Ajouter en ami
                        </button>

                        <button v-if="friendRequestStatus === 'Accepted'"
                            class="inline-flex w-auto cursor-pointer select-none appearance-none items-center justify-center space-x-1 rounded border border-green-200 bg-green-500 px-3 py-2 text-sm font-medium text-white">
                            Vous êtes amis
                        </button>

                        <button v-if="friendRequestStatus === 'Rejected'"
                            class="inline-flex w-auto cursor-pointer select-none appearance-none items-center justify-center space-x-1 rounded border border-red-200 bg-red-500 px-3 py-2 text-sm font-medium text-white">
                            Requête refusée
                        </button>
                        <button v-if="friendRequestStatus === 'Pending'"
                            class="disabled inline-flex w-auto cursor-pointer select-none appearance-none items-center justify-center space-x-1 rounded border border-blue-200 bg-blue-500 px-3 py-2 text-sm font-medium text-white">
                            Requête déjà envoyé
                        </button>
                    </div>

                    <div v-else>
                        <button v-if="!isEditing" @click="isEditing = true"
                            class="inline-flex w-auto cursor-pointer select-none appearance-none items-center justify-center space-x-1 rounded border border-gray-200 bg-yellow-500 px-3 py-2 text-sm font-medium text-white">
                            Modifier le profil
                        </button>
                        <button v-else @click="saveUserProfile"
                            class="inline-flex w-auto cursor-pointer select-none appearance-none items-center justify-center space-x-1 rounded border border-gray-200 bg-green-500 px-3 py-2 text-sm font-medium text-white">
                            Enregistrer
                        </button>
                    </div>
                </div>

                <h4 class="text-md font-medium leading-3 text-gray-200">Profil</h4>

                <p v-if="!isEditing" class="text-sm text-gray-300">
                    {{ userProfile?.profile || "Aucune description disponible." }}
                </p>
                <textarea v-else v-model="editableProfile" class="border rounded px-2 py-1 w-full" rows="3"></textarea>

                <h4 class="text-md font-medium leading-3 text-gray-200">Discussions</h4>

                <div class="flex justify-end">
                    <label for="sort" class="text-sm text-gray-200 mr-2">Trier par:</label>
                    <select id="sort" v-model="sortMethod"
                        class="text-sm border rounded px-2 py-1 bg-white border-gray-300 text-gray-800 focus:ring-2 focus:ring-blue-300 focus:outline-none">
                        <option value="date">Date</option>
                        <option value="alphabetical">Alphabétique</option>
                    </select>
                </div>

                <div class="flex flex-col gap-3 mt-3">
                    <div v-for="(item, index) in sortedDiscussions" :key="index"
                        class="flex items-center gap-3 px-2 py-3 bg-white rounded border w-full hover:scale-95">
                        <RouterLink :to='`/discussion/${item.id}`' class="w-full flex items-center gap-3">
                            <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24"
                                class="h-8 w-8 text-slate-500" height="1em" width="1em"
                                xmlns="http://www.w3.org/2000/svg">
                                <path fill="none" d="M0 0h24v24H0z"></path>
                                <path
                                    d="M22 12c0-5.52-4.48-10-10-10S2 6.48 2 12c0 4.84 3.44 8.87 8 9.8V15H8v-3h2V9.5C10 7.57 11.57 6 13.5 6H16v3h-2c-.55 0-1 .45-1 1v2h3v3h-3v6.95c5.05-.5 9-4.76 9-9.95z">
                                </path>
                            </svg>
                            <div class="leading-3">
                                <p class="text-sm font-bold text-slate-700">{{ item.title }}</p>
                                <span class="text-xs text-slate-600">{{ dateUtils.timeAgo(item.date!) }}</span>
                            </div>
                            <p class="text-sm text-slate-500 self-start ml-auto">{{ item.description }}</p>
                        </RouterLink>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
