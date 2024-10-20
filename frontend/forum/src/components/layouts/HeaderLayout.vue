<script setup lang="ts">
import { onMounted, computed, ref } from 'vue';
import { RouterLink, useRoute } from 'vue-router';
import sessionService from '@/api/services/sessionService';
import notificationService from '@/api/services/notificationService';
import type { Notification } from '@/types/types';
import dateUtils from '@/utils/dateUtils';
import userService from '@/api/services/userService';

onMounted(async () => {
  if (isLoggedIn.value) {
    await fetchNotification();
    checkNotificationLeftUnread();
  }
})

const isLoggedIn = computed(() => sessionService.logged());
const isMobileDropdownVisible = ref(false); // Toggle state for the dropdown
const isNotificationDropdownVisible = ref(false); // Toggle state for the dropdown
const isThereNotificationLeftUnread = ref();
const notificationList = ref<Notification[]>([]);
const showReadNotification = ref(true);
const route = useRoute();

const toggleMobileDropdown = () => {
  isMobileDropdownVisible.value = !isMobileDropdownVisible.value;
};

const toggleNotificationDropdown = () => {
  checkNotificationLeftUnread();
  isNotificationDropdownVisible.value = !isNotificationDropdownVisible.value;
};

const changeNotificationState = async (id: number) => {
  const notificationToChange = notificationList.value.find(n => n.id == id);
  notificationToChange!.isRead = !notificationToChange?.isRead;
  await notificationService.changeStateNotification(id);

}

const readAll = async () => {
  notificationList.value.map(n => n.isRead = true);
  await notificationService.markAllNotificationOfUserAsRead(sessionService.getIdUser()!);
  await fetchNotification();
}

const showHideNotification = async () => {
  showReadNotification.value = !showReadNotification.value;
  await fetchNotification();
}

const fetchNotification = async () => {
  try {
    const response = await notificationService.getNotificationOfUser();
    notificationList.value = showReadNotification.value ?
      [...response.filter(item => !item.isRead), ...response.filter(item => item.isRead)]
      : response.filter(item => !item.isRead);
  } catch (error) {
    console.error('Error fetching user list:', error);
  }
}

const checkNotificationLeftUnread = () => {
  isThereNotificationLeftUnread.value = notificationList.value.find(
    n => !n.isRead
  )
}

const AcceptRequest = async (notificationId: number, requestId: number) => {
  await notificationService.changeStateNotification(notificationId);
  await userService.acceptFriendRequest(requestId);
  await fetchNotification();
}

const RefuseRequest = async (notificationId: number, requestId: number) => {
  await notificationService.changeStateNotification(notificationId);
  await userService.refuseFriendRequest(requestId);
  await fetchNotification();
}

</script>

<template>
  <nav class="bg-white border-gray-200 dark:bg-gray-900 fixed w-full h-[5rem] z-100">
    <div class="flex flex-wrap items-center justify-between mx-auto p-4">

      <!-- Replace by logo  -->
      <div class="hidden md:block"></div>

      <div v-if="isLoggedIn" class="relative order-12">
        <button @click="toggleNotificationDropdown"
          class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-left inline-flex items-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
          type="button">
          <svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="24" height="24" viewBox="0 0 24 24">
            <path
              d="M 12 2 C 11.172 2 10.5 2.672 10.5 3.5 L 10.5 4.1953125 C 7.9131836 4.862095 6 7.2048001 6 10 L 6 16 L 4.4648438 17.15625 L 4.4628906 17.15625 A 1 1 0 0 0 4 18 A 1 1 0 0 0 5 19 L 12 19 L 19 19 A 1 1 0 0 0 20 18 A 1 1 0 0 0 19.537109 17.15625 L 18 16 L 18 10 C 18 7.2048001 16.086816 4.862095 13.5 4.1953125 L 13.5 3.5 C 13.5 2.672 12.828 2 12 2 z M 10 20 C 10 21.1 10.9 22 12 22 C 13.1 22 14 21.1 14 20 L 10 20 z">
            </path>
          </svg>
          <span v-if="isThereNotificationLeftUnread"
            class="absolute top-0.5 right-0.5 grid min-h-[12px] min-w-[12px] translate-x-2/4 -translate-y-2/4 place-items-center rounded-full bg-red-600 py-1 px-1 text-xs text-white"></span>
        </button>


        <div v-if="isNotificationDropdownVisible"
          class="order-last right-0 z-10 absolute bg-white divide-y divide-gray-100 rounded-lg shadow w-44 dark:bg-gray-700 overflow-hidden">
          <div class="flex flex-col max-h-40 overflow-y-scroll">
            <div id="toast-notification"
              class="w-full max-w-xs p-4 text-gray-900 bg-white rounded-lg shadow dark:bg-gray-800 dark:text-gray-300"
              role="alert">

              <div v-if="notificationList.length == 0">Aucune nouvelles notifications</div>

              <div v-for="(item, index) in notificationList" :key="index" class="flex"
                :class="{ 'brightness-50': item.isRead }">
                <div v-if="item.notificationType != 2" class="relative inline-block shrink-0">
                  <svg fill="#000000" height="20" width="20" version="1.1" id="Capa_1"
                    xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 60 60"
                    xml:space="preserve">
                    <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
                    <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
                    <g id="SVGRepo_iconCarrier">
                      <path
                        d="M30,1.5c-16.542,0-30,12.112-30,27c0,5.205,1.647,10.246,4.768,14.604c-0.591,6.537-2.175,11.39-4.475,13.689 c-0.304,0.304-0.38,0.769-0.188,1.153C0.276,58.289,0.625,58.5,1,58.5c0.046,0,0.093-0.003,0.14-0.01 c0.405-0.057,9.813-1.412,16.617-5.338C21.622,54.711,25.738,55.5,30,55.5c16.542,0,30-12.112,30-27S46.542,1.5,30,1.5z">
                      </path>
                    </g>
                  </svg>
                  <button @click="changeNotificationState(item.id)">
                    <svg v-if="item.isRead" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"
                      stroke-width="1.5" stroke="currentColor" width="15" height="15">
                      <path stroke-linecap="round" stroke-linejoin="round"
                        d="M3.98 8.223A10.477 10.477 0 001.934 12C3.226 16.338 7.244 19.5 12 19.5c.993 0 1.953-.138 2.863-.395M6.228 6.228A10.45 10.45 0 0112 4.5c4.756 0 8.773 3.162 10.065 7.498a10.523 10.523 0 01-4.293 5.774M6.228 6.228L3 3m3.228 3.228l3.65 3.65m7.894 7.894L21 21m-3.228-3.228l-3.65-3.65m0 0a3 3 0 10-4.243-4.243m4.242 4.242L9.88 9.88" />
                    </svg>
                    <svg v-else xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                      stroke="currentColor" width="15" height="15">
                      <path stroke-linecap="round" stroke-linejoin="round"
                        d="M2.036 12.322a1.012 1.012 0 010-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178z" />
                      <path stroke-linecap="round" stroke-linejoin="round" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                    </svg>
                  </button>
                </div>
                <div v-if="item.notificationType == 0" class="ms-3 text-xs font-normal">
                  <div class="text-xs font-semibold text-gray-900 dark:text-white"> {{ item.referenceString }} </div>
                  <div class="text-xs font-normal"> Nouveau message de {{ item.referenceString }} </div>
                  <span class="text-xs font-medium text-blue-600 dark:text-blue-500"> {{ dateUtils.timeAgo(item.date)
                    }}</span>
                </div>
                <div v-if="item.notificationType == 1" class="ms-3 text-xs font-normal">
                  <div class="text-xs font-semibold text-gray-900 dark:text-white"> {{ item.referenceString }} </div>
                  <div class="text-xs font-normal"> Nouveau message dans {{ item.referenceString }} </div>
                  <span class="text-xs font-medium text-blue-600 dark:text-blue-500"> {{ dateUtils.timeAgo(item.date)
                    }}</span>
                </div>
                <div v-if="item.notificationType == 2 && !item.isRead" class="ms-3 text-xs font-normal">
                  <div class="text-xs font-semibold text-gray-900 dark:text-white"> {{ item.referenceId }} </div>
                  <div class="text-xs font-normal"> Nouvelle demande d'amis de {{ item.referenceId }} </div>
                  <button @click="AcceptRequest(item.id, item.friendRequestId)" class=" bg-blue-500 p-2 w-full rounded-lg hover:opacity-70">Accepter</button>
                  <button @click="RefuseRequest(item.id, item.friendRequestId)" class=" bg-red-500 p-2 w-full rounded-lg hover:opacity-70">Refuser</button>
                  <span class="text-xs font-medium text-blue-600 dark:text-blue-500"> {{ dateUtils.timeAgo(item.date)
                    }}</span>
                </div>
              </div>

            </div>
          </div>

          <!-- Always-present button at the bottom of the dropdown -->
          <div class="p-2 flex justify-evenly">
            <button @click="readAll" class="text-xs  text-blue-500 focus:outline-none">Tout marquer comme lu</button>
            <button @click="showHideNotification" class="text-xs  text-blue-500 focus:outline-none">
              {{ showReadNotification ? "Masquer" : "Afficher" }} les lu</button>
          </div>
        </div>
      </div>

      <div v-else class="relative order-last">aa</div>

      <button @click="toggleMobileDropdown" type="button"
        class="inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600">
        <svg class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 17 14">
          <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
            d="M1 1h15M1 7h15M1 13h15" />
        </svg>
        <span class="sr-only">Open main menu</span>
      </button>

      <!-- Dropdown / Navigation Links -->
      <div :class="isMobileDropdownVisible ? 'block order-last' : 'hidden'"
        class="items-center justify-between w-full text-right md:flex md:w-auto" id="navbar-search">

        <!-- Navigation Links -->
        <ul
          class="flex flex-col p-4 mt-4 font-medium border border-gray-100 rounded-lg bg-gray-50 md:space-x-8 rtl:space-x-reverse md:flex-row md:mt-0 md:border-0 md:bg-white dark:bg-gray-800 md:dark:bg-gray-900 dark:border-gray-700">
          <li>
            <RouterLink to="/home"
              :class="route.path === '/home' ? 'block py-2 px-3 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500'
                : 'block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700'">
              Accueil</RouterLink>
          </li>
          <li v-if="isLoggedIn">
            <RouterLink :to='"/profile/" + sessionService.getIdUser()'
              :class="route.path.startsWith('/profile') ? 'block py-2 px-3 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500'
                : 'block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700'">
              Profile</RouterLink>
          </li>
          <li v-if="!isLoggedIn">
            <RouterLink to="/login"
              :class="route.path === '/login' ? 'block py-2 px-3 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500'
                : 'block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700'">
              Se connecter</RouterLink>
          </li>
          <li v-if="!isLoggedIn">
            <RouterLink to="/signin"
              :class="route.path === '/signin' ? 'block py-2 px-3 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500'
                : 'block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700'">
              S'inscrire</RouterLink>
          </li>
          <li v-if="isLoggedIn">
            <RouterLink to="/login"
              class="block py-2 px-3 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700"
              @click="sessionService.logout">Se déconnecter</RouterLink to="/login">
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>
