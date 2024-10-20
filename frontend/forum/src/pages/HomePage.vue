<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import { RouterLink, useRoute } from 'vue-router';
import Modal from '@/components/Modal.vue';
import discussionService from '@/api/services/discussionService';
import type { Discussion } from '@/types/types';
import router from '@/router/router';
import dateUtils from '@/utils/dateUtils';
import sessionService from '@/api/services/sessionService';

const props = defineProps<{
    id: number;
}>();

onMounted(() => {
    fetchDiscussion();
});

const route = useRoute();
watch(
    () => route.params.id,
    (newId) => {
        currentPage.value = Number(newId);
        fetchDiscussion();
    }
);

const discussionsInfo = ref<Discussion[]>([]);
const filteredDiscussions = ref<Discussion[]>([]);
const isModalOpen = ref(false);
const searchInput = ref('');
const sortCriteria = ref('date-asc');

// Pagination logic
const itemsPerPage = ref(3);
const currentPage = ref(props.id ?? 1);

const fetchDiscussion = async () => {
    try {
        const response = await discussionService.getAllDiscussions();
        discussionsInfo.value = response;
        filteredDiscussions.value = discussionsInfo.value;
    } catch (error) {
        console.error('Error fetching discussions list:', error);
    }
};

const toggleModal = () => {
    isModalOpen.value = !isModalOpen.value;
};

const filterDiscussions = () => {
    const searchQuery = searchInput.value.toLowerCase().trim();
    const searchWords = searchQuery.split(' '); // Split query into words for substring matching

    // Filter discussions
    filteredDiscussions.value = discussionsInfo.value.filter(item => {
        const title = item.title.toLowerCase();
        // Check if all words from the search query are present in the title
        return searchWords.every(word => title.includes(word));
    });

    // Sort discussions based on selected criteria
    sortDiscussions();

    // Reset pagination to page 1 after filtering
    currentPage.value = 1;
};

// Sorting function
const sortDiscussions = () => {
    if (sortCriteria.value.includes('date')) {
        filteredDiscussions.value.sort((a, b) => {
            const dateA = new Date(a.date!).getTime();
            const dateB = new Date(b.date!).getTime();
            return sortCriteria.value === 'date-asc' ? dateA - dateB : dateB - dateA;
        });
    } else if (sortCriteria.value.includes('title')) {
        filteredDiscussions.value.sort((a, b) => {
            return sortCriteria.value === 'title-asc'
                ? a.title.localeCompare(b.title)
                : b.title.localeCompare(a.title);
        });
    }
};

// Calculate total number of pages based on filtered discussions
const totalPages = computed(() => {
    return Math.ceil(filteredDiscussions.value.length / itemsPerPage.value);
});

// Get paginated items for the current page
const paginatedDiscussions = computed(() => {
    const start = (currentPage.value - 1) * itemsPerPage.value;
    const end = start + itemsPerPage.value;
    return filteredDiscussions.value.slice(start, end);
});

// Function to move to the previous page
const previousPage = () => {
    if (currentPage.value > 1) {
        currentPage.value--;
        const id = currentPage.value;
        router.push({ name: 'Home', params: { id } });
    }
};

// Function to move to the next page
const nextPage = () => {
    if (currentPage.value < totalPages.value) {
        currentPage.value++;
        const id = currentPage.value;
        router.push({ name: 'Home', params: { id } });
    }
};

// Function to go to a specific page
const goToPage = (id: number) => {
    router.push({ name: 'Home', params: { id } });
    currentPage.value = id;
};

</script>

<template>
    <div class="bg-gray-700 h-[100%] pl-10 pr-10">
        <h1 class="text-2xl font-bold mb-4 w-full text-center text-gray-200 flex flex-col">Page d'accueil</h1>

        <!-- Search Form with Filters -->
        <form @submit.prevent="filterDiscussions" class="mb-4 flex flex-col md:flex-row items-center">
            <label for="searchInput" class="sr-only">Rechercher des discussions</label>
            <input id="searchInput" type="text" v-model="searchInput" placeholder="Recherchez des discussions"
                class="border w-full md:w-[50%] rounded p-2 mb-2 md:mb-0" />

            <label for="sortCriteria" class="sr-only">Trier par</label>
            <select v-model="sortCriteria" id="sortCriteria"
                class="border rounded p-2 w-full md:w-auto mb-2 md:mb-0 ml-2 mr-2">
                <option value="date-asc">Date Croissante</option>
                <option value="date-desc">Date Décroissante</option>
                <option value="title-asc">Titre Croissant</option>
                <option value="title-desc">Titre Décroissant</option>
            </select>

            <button type="submit"
                class="bg-blue-500 hover:bg-blue-600 text-white rounded p-2 w-full md:w-auto">Rechercher</button>
        </form>

        <!-- Modal Trigger -->
        <button v-if="sessionService.logged()" @click="toggleModal" class="bg-green-500 text-white rounded p-2 mb-4">Nouvelle discussion</button>
        <Modal :isModalOpen="isModalOpen" :toggleModale="toggleModal" />

        <h2 v-if="paginatedDiscussions.length == 0" class="text-center text-2xl text-gray-200">
            Aucune discussion correspondante
        </h2>

        <!-- Paginated and Filtered Discussions List -->
        <div>
            <div v-for="(item, index) in paginatedDiscussions" :key="index">
                <article class="rounded-xl border-2 border-gray-100 bg-gray-500 mb-4 shadow-md">
                    <div class="flex items-start gap-4 p-4 sm:p-6 lg:p-8">
                        <RouterLink :to="`/profile/${item.creatorId}`" class="block shrink-0">
                            <img alt="avatar"
                                src="https://i0.wp.com/unleash-gods-dream.com/wp-content/uploads/2023/03/placeholder-image-blue-square.png?ssl=1"
                                class="w-14 h-14 rounded-lg object-cover" />
                        </RouterLink>

                        <div>
                            <h3 class="font-medium sm:text-lg text-gray-100">
                                <RouterLink class="hover:underline" :to="`/discussion/${item.id}`">
                                    {{ item.title }}
                                </RouterLink>
                            </h3>

                            <p class="line-clamp-2 text-sm text-gray-200">{{ item.description }}</p>

                            <div class="mt-2 flex items-center gap-2">
                                <div class="flex items-center gap-1 text-gray-200">
                                    <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" fill="none"
                                        viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                                        <path stroke-linecap="round" stroke-linejoin="round"
                                            d="M17 8h2a2 2 0 012 2v6a2 2 0 01-2 2h-2v4l-4-4H9a1.994 1.994 0 01-1.414-.586m0 0L11 14h4a2 2 0 002-2V6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2v4l.586-.586z" />
                                    </svg>
                                    <p class="text-xs">{{ item.commentCount }} comments</p>
                                </div>

                                <span>&middot;</span>

                                <p class="text-xs text-gray-200">
                                    Posted by
                                    <RouterLink :to="`/profile/${item.creatorId}`"
                                        class="font-medium underline hover:text-gray-700">
                                        {{ item.creatorIdentifiant }}
                                    </RouterLink>
                                </p>
                                <span>&middot;</span>
                                <p class="block text-xs text-gray-200">{{ dateUtils.timeAgo(item.date!) }}</p>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
        </div>

        <!-- Pagination Controls -->
        <div v-if="paginatedDiscussions.length > 0" class="flex justify-center items-center mt-6 space-x-4">
            <!-- Previous Button -->
            <button @click="previousPage" :disabled="currentPage === 1"
                class="bg-gray-300 text-gray-700 rounded p-2 transition-colors duration-200 hover:bg-gray-400 disabled:bg-slate-500 disabled:cursor-not-allowed">
                Précédent
            </button>

            <!-- Page Numbers -->
            <div class="flex space-x-2">
                <button v-for="page in totalPages" :key="page" @click="goToPage(page)" :class="['border rounded p-2 transition-colors duration-200 w-10 h-10', {
                    'bg-blue-500 text-white': currentPage === page,
                    'bg-white text-gray-700 hover:bg-blue-100': currentPage !== page
                }]" class="page-button">
                    {{ page }}
                </button>
            </div>

            <!-- Next Button -->
            <button @click="nextPage" :disabled="currentPage === totalPages"
                class="bg-gray-300 text-gray-700 rounded p-2 transition-colors duration-200 hover:bg-gray-400 disabled:bg-slate-500 disabled:cursor-not-allowed">
                Suivant
            </button>
        </div>
    </div>
</template>
