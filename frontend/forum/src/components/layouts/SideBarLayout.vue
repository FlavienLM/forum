<script setup lang="ts">
import discussionService from '@/api/services/discussionService';
import sessionService from '@/api/services/sessionService';
import type { DiscussionSidebarItem , FriendSidebarItem, User  } from '@/types/types';
import { computed, onMounted, ref, watch } from 'vue';
import dateUtils from '@/utils/dateUtils';
import userService from '@/api/services/userService';

onMounted(async () => {
  if(sessionService.logged()){
    fetchs();
  }
})


const isLoggedIn = computed(() => sessionService.logged());
watch(
  isLoggedIn,
  (isLogged) => {
    if(isLogged){
      fetchs();
    }
  }
);

const activeTab = ref<number>(0);
const recentDiscussion = ref<DiscussionSidebarItem []>();
const recentFriendChat = ref<FriendSidebarItem []>();
const FriendOfUSerList = ref<FriendSidebarItem []>();
const user = ref<User>();
const sidebarOpen = ref<boolean>(true);const searchFriend = ref<string>(''); // State for searching friends

const fetchs = async () => {
    recentDiscussion.value = await discussionService.getDiscussionWhereUserLastParticipated(sessionService.getIdUser()!);
    recentFriendChat.value = await userService.getFriendWithWhomUserLastInteracted(sessionService.getIdUser()!);
    FriendOfUSerList.value = await userService.getFriendsOfUser(sessionService.getIdUser()!);
    user.value = sessionService.getItem()!;
}

const setActiveTab = (number: number) => {
  activeTab.value = number;
}

const handleToggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value;
};

const filteredFriendList = computed(() => {
  const searchTerm = searchFriend.value.trim().toLowerCase();

  if (searchTerm === '') {
    return recentFriendChat.value; // Return all friends when no search term
  } else {
    return recentFriendChat.value?.filter(friend =>
      friend.pseudo.toLowerCase().includes(searchTerm) ||
      friend.accountId.toString().includes(searchTerm) 
    );
  }
});

</script>

<template>
  <div v-if="isLoggedIn">
    <div class="flex fixed pt-[5rem]">
      <!-- Sidebar -->

      <nav
        class="h-[100vh]  bg-blue-500">
        <div
          :class="[sidebarOpen ? 'translate-x-0' : '-translate-x-[95%]', 'w-[220px]  bg-blue-500 transition-transform duration-300 ease-in-out flex-shrink-0']"
          class="absolute">
          <!-- Profile card -->
          <RouterLink :to='"/profile/" + sessionService.getIdUser()'>
            <div class="flex cursor-pointer hover:scale-95 mx-auto  w-[90%] mt-2 p-2 border-2 bg-blue-400 rounded-lg border-blue-700"> 
              <div class="relative inline-block shrink-0">
                <img class="ml-2 mt-2 w-12 h-12 rounded-full" src="https://media.istockphoto.com/id/2151669184/vector/vector-flat-illustration-in-grayscale-avatar-user-profile-person-icon-gender-neutral.jpg?s=612x612&w=0&k=20&c=UEa7oHoOL30ynvmJzSCIPrwwopJdfqzBs0q69ezQoM8=" alt="pp placeholder" />
              </div>
              <div class="ms-3 text-xs font-normal">
                <div class="text-xs font-semibold text-gray-900 dark:text-white"> {{ user?.accountId }} </div>
                <div class="text-xs font-normal text-gray-900 dark:text-white"> @{{ user?.accountId }} </div>
              </div>
            </div>
          </RouterLink>
          <div class="h-full pt-4 flex flex-col justify-between">
            <!-- Tabs Section -->
            <ul
              class="flex flex-wrap text-sm font-medium text-center justify-center text-gray-500 border-b border-gray-200 dark:border-gray-700 dark:text-gray-400">
              <!-- Tab: Discussion -->
              <li class="me-2">
                <a v-bind:class="{
                  'text-blue-600 bg-gray-100 active dark:bg-gray-800 dark:text-blue-500': activeTab === 0,
                  'hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300': activeTab !== 0
                }" class="inline-block p-4 rounded-t-lg cursor-pointer" @click="setActiveTab(0)">
                  Discussion
                </a>
              </li>

              <!-- Tab: Amis -->
              <li class="me-2">
                <a v-bind:class="{
                  'text-blue-600 bg-gray-100 active dark:bg-gray-800 dark:text-blue-500': activeTab === 1,
                  'hover:text-gray-600 hover:bg-gray-50 dark:hover:bg-gray-800 dark:hover:text-gray-300': activeTab !== 1
                }" class="inline-block p-4 rounded-t-lg cursor-pointer" @click="setActiveTab(1)">
                  Amis
                </a>
              </li>

            </ul>

            <!-- Tab Content Section -->
            <!-- Discussions -->
            <div class="tab-content mt-4 overflow-y-scroll h-[calc(100vh-14rem)] no-scrollbar">
              <div v-if="activeTab === 0">
                <div v-for="(item, index) in recentDiscussion" :key="index" class="flex p-2 mx-auto w-[80%] border-2 rounded-lg border-blue-700 mb-2 hover:scale-95">
                  <RouterLink class="flex" :to='`/discussion/${item.id}`'>
                  <div class="relative inline-block shrink-0" >
                    <img alt="avatar"
                                src="https://i0.wp.com/unleash-gods-dream.com/wp-content/uploads/2023/03/placeholder-image-blue-square.png?ssl=1"
                                class="w-12 h-12 rounded-lg object-cover" />
                  </div>
                  <div class="ms-3 text-xs font-normal">
                    <div class="text-xs font-semibold text-gray-900 dark:text-white"> {{ item.title }} </div>
                    <div class="text-xs font-normal"> {{ item.lastMessage }} </div>
                    <div class="text-xs font-normal"> {{ dateUtils.timeAgo(item.date) }} </div>
                  </div>
                  </RouterLink>
                </div>
              </div>

              <!-- Friends -->
              <div v-if="activeTab === 1">
                <p class="text-white pl-4">Rechercher amis</p>
                <input type="text" v-model="searchFriend" placeholder="Rechercher amis"
                  class="mt-2 p-1 rounded ml-4 mb-3" />
                <div v-for="(item, index) in filteredFriendList" :key="index" class="flex p-2 mx-auto w-[80%] border-2 rounded-lg border-blue-700 mb-2 hover:scale-95">
                  <RouterLink class="flex" :to='`/friend/${item.friendId}`'>
                    <div class="relative inline-block shrink-0">
                      <RouterLink class="flex" :to='`/profile/${item.friendId}`'>
                        <img class="ml-2 w-8 h-8 rounded-full hover:border-blue-700 border-2" src="https://media.istockphoto.com/id/2151669184/vector/vector-flat-illustration-in-grayscale-avatar-user-profile-person-icon-gender-neutral.jpg?s=612x612&w=0&k=20&c=UEa7oHoOL30ynvmJzSCIPrwwopJdfqzBs0q69ezQoM8=" alt="pp placeholder" />
                      </RouterLink>
                    </div>
                  <div class="ms-3 text-xs font-normal">
                    <div class="text-xs font-semibold text-gray-900 dark:text-white"> {{ item.pseudo }} </div>
                    <div class="text-xs font-semibold text-gray-900 dark:text-white">@{{ item.accountId }}</div>
                    <div class="text-xs font-normal"> {{ item.lastMessage }} </div>
                    <div class="text-xs font-normal"> {{ dateUtils.timeAgo(item.date) }} </div>
                  </div>
                </RouterLink>
                </div>
              </div>
            </div>
          </div>
        </div>
        <!-- Toggle Button at the intersection -->
        <div
          :class="[sidebarOpen ? 'w-[220px]' : 'w-[10px]', 'duration-300']">
        <div
          class="absolute top-1/2 transform -translate-y-1/2 right-0 cursor-pointer bg-blue-500 p-2 rounded-full translate-x-[50%]"
          @click="handleToggleSidebar" :class="sidebarOpen ? 'translate-x-[100%]' : ''">
          <div class="transform transition-transform duration-300" :class="sidebarOpen ? '-rotate-180' : ''">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-white" fill="none" viewBox="0 0 24 24"
              stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
            </svg>
          </div>
        </div>
        </div>
      </nav>
    </div>
    <!-- Shadow div that replicate the sidebar width, as the sidebar is absolute -->
    <div class="h-[100vh] bg-blue-500 -z-10 transition-width duration-300 ease-in-out flex-shrink-0 lg:relative absolute"
      :class="[sidebarOpen ? 'w-[220px]' : 'w-[10px]']">
    </div>
  </div>
</template>
