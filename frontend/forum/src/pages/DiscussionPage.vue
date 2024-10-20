<script setup lang="ts">
import discussionService from '@/api/services/discussionService';
import sessionService from '@/api/services/sessionService';
import { type Discussion, type Message } from '@/types/types';
import dateUtils from '@/utils/dateUtils';
import { onMounted, ref, watch } from 'vue';
import { useRoute } from 'vue-router';
const props = defineProps<{
  id: number
}>()

onMounted(() => {
  fetchMessages();
  fetchDiscussion();
});

const route = useRoute();
watch(
  () => route.params.id, // Watch the route parameter 'id'
  (newId) => {
    currentDiscussionId.value = Number(newId);  // Update id with the new value
    fetchMessages();       // Fetch new data for the updated id
    fetchDiscussion();
  }
);

const messages = ref<Message[]>([]);
const discussion = ref<Discussion>();
const newMessage = ref("");
const modify_post_id = ref();
const content_modify = ref("");
const currentDiscussionId = ref(props.id ?? 1);

const fetchMessages = async () => {
  try {
    messages.value = await discussionService.getMessageOfDiscussion(currentDiscussionId.value);
  } catch (error) {
    console.error('Error fetching user list:', error);
  }
};

const fetchDiscussion = async () => {
  try {
    discussion.value = await discussionService.getDiscussionById(currentDiscussionId.value)!;
  } catch (error) {
    console.error('Error fetching user list:', error);
  }
}

const sendMessage = async () => {
  await discussionService.createNewMessageInDiscussion({
    content: newMessage.value,
    date: dateUtils.formatCurrentDate(),
    discussionId: props.id,
    authorId: sessionService.getIdUser()! ?? 1,
    state: 0
  });
  fetchMessages();
}

const startModifyMessageContent = (index: number, content: string) => {
  modify_post_id.value = index;
  content_modify.value = content;
}

const updateMessageContent = async () => {
  await discussionService.modifyMessage(modify_post_id.value, content_modify.value);
  modify_post_id.value = -1;
  fetchMessages();
}

const cancellMessageModification = () => {
  modify_post_id.value = -1;
  content_modify.value = "";
}









</script>


<template>
  <div class="h-full bg-gray-700">
    <div class="mb-4 p-4 border rounded-lg flex flex-col items-center text-center  justify-center ml-[25%] mr-[25%]">
      <h2 class="text-xl font-bold mb-2 text-gray-200">{{ discussion?.title }}</h2>

      <!-- SVG icon, centered -->
      <svg fill="#000000" height="48" width="48" version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg"
        xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 60 60" xml:space="preserve" class="mb-2">
        <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
        <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
        <g id="SVGRepo_iconCarrier">
          <path
            d="M30,1.5c-16.542,0-30,12.112-30,27c0,5.205,1.647,10.246,4.768,14.604c-0.591,6.537-2.175,11.39-4.475,13.689 
        c-0.304,0.304-0.38,0.769-0.188,1.153C0.276,58.289,0.625,58.5,1,58.5c0.046,0,0.093-0.003,0.14-0.01
        c0.405-0.057,9.813-1.412,16.617-5.338C21.622,54.711,25.738,55.5,30,55.5c16.542,0,30-12.112,30-27S46.542,1.5,30,1.5z">
          </path>
        </g>
      </svg>

      <p class="text-gray-200">{{ discussion?.description }}</p>
    </div>

    <h2 v-if="messages.length == 0" class="text-center text-2xl text-gray-200">
            Aucune message pour le moment
        </h2>

    <div>
      <!-- Display messages -->
      <div v-for="(item, index) in messages" :key="index">

        <!-- Comments from other users -->
        <div v-if="item.authorId !== sessionService.getIdUser()">
          <article class="p-6 mb-3 ml-6 lg:ml-12 text-base bg-white rounded-lg dark:bg-gray-900">
            <footer class="flex justify-between items-center mb-2">
              <div class="flex items-center">
                <RouterLink :to="`/profile/${item.authorId}`" class="flex items-center">
                  <img class="mr-2 w-6 h-6 rounded-full"
                    src="https://media.istockphoto.com/id/2151669184/vector/vector-flat-illustration-in-grayscale-avatar-user-profile-person-icon-gender-neutral.jpg?s=612x612&w=0&k=20&c=UEa7oHoOL30ynvmJzSCIPrwwopJdfqzBs0q69ezQoM8="
                    alt="pp placeholder" />
                  <p class="inline-flex items-center mr-3 text-sm text-gray-900 dark:text-white font-semibold">
                    {{ item.pseudo }}
                  </p>
                </RouterLink>
                <p class="text-sm text-gray-600 dark:text-gray-400">
                  {{ dateUtils.timeAgo(item.date) }}
                </p>
                <p v-if="item.lastModified" class="text-xs text-gray-500 ml-3">
                  (Dernière modification: {{ dateUtils.timeAgo(item.lastModified) }})
                </p>
              </div>
            </footer>
            <p class="text-gray-500 dark:text-gray-200"> {{ item.content }} </p>
          </article>
        </div>

        <!-- Self-posted comments -->
        <div v-else>
          <article class="p-6 mb-3 ml-6 lg:ml-12 text-base bg-white rounded-lg dark:bg-gray-900">
            <footer class="flex justify-between items-center mb-2">
              <div class="flex items-center space-x-2">
                <!-- Edit Icon -->
                <button @click="startModifyMessageContent(item.id!, item.content)"
                  class="p-1 rounded-md hover:bg-gray-200 dark:hover:bg-gray-800">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-500" viewBox="0 0 20 20"
                    fill="currentColor">
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
                  {{ dateUtils.timeAgo(item.date) }}
                </p>
                <p class="inline-flex items-center ml-3 text-sm text-gray-900 dark:text-white font-semibold">
                  Vous
                </p>
                <div class="inline-block shrink-0 ml-2">
                  <img class="mr-2 w-6 h-6 rounded-full"
                    src="https://media.istockphoto.com/id/2151669184/vector/vector-flat-illustration-in-grayscale-avatar-user-profile-person-icon-gender-neutral.jpg?s=612x612&w=0&k=20&c=UEa7oHoOL30ynvmJzSCIPrwwopJdfqzBs0q69ezQoM8="
                    alt="pp placeholder" />
                </div>
              </div>
            </footer>

            <!-- Message Modification Field -->
            <div v-if="modify_post_id === item.id">
              <textarea v-model="content_modify" class="w-full p-2 border rounded-md"></textarea>
              <button @click="updateMessageContent"
                class="px-4 py-2 mt-2 text-white bg-blue-500 rounded-md hover:bg-blue-600">Modifier</button>
              <button @click="cancellMessageModification"
                class="px-4 py-2 mt-2 text-white bg-red-500 rounded-md hover:bg-red-600">Annuler</button>
            </div>

            <!-- Comment content -->
            <p v-else class="text-gray-500 dark:text-gray-200 text-right"> {{ item.content }} </p>
          </article>
        </div>
      </div>

      <!-- New Message Input -->
      <div v-if="sessionService.logged()" class="mt-4 w-[50%] mx-auto flex flex-col items-center">
        <textarea v-model="newMessage" placeholder="Ecrivez votre message..."
          class="w-full p-3 border rounded-lg"></textarea>
        <button @click="sendMessage" class="mt-2 px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600">
          Envoyer
        </button>
      </div>

      <div v-else class="mt-4 w-[50%] mx-auto flex flex-col items-center">
        <textarea disabled v-model="newMessage" placeholder="Veillez vous connecter pour participer à la discussion"
          class="w-full p-3 border rounded-lg"></textarea>
        <RouterLink to="/login" class="mt-2 px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600">
          Se connecter
        </RouterLink>
      </div>
    </div>
  </div>
</template>
