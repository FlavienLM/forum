<script setup lang="ts">
import { ref } from 'vue';
import discussionService from '@/api/services/discussionService';
import sessionService from '@/api/services/sessionService';
import dateUtils from '@/utils/dateUtils';

const props = defineProps<{
    isModalOpen : boolean;
    toggleModale: () => void;
}>();

const formData = ref({
    title: "",
    description: "",
});

const errorMessage = ref<string | null>(null);

// Validation and form submission
const validate = () => {
    errorMessage.value = null;
    if (!formData.value.title || !formData.value.description) {
        errorMessage.value = "Il faut un titre et une description !";
        return;
    }

    // Create discussion request
    discussionService.createDiscussion({
        title: formData.value.title,
        description: formData.value.description,
        creatorId: sessionService.getIdUser()!,
        date: dateUtils.formatCurrentDate(),
    }).then(() => {
        formData.value.title = "";
        formData.value.description = "";
        props.toggleModale(); // Close modal on success
    }).catch(error => {
        errorMessage.value = "Failed to create discussion: " + error.message;
    });
};

</script>

<template>

    <!-- Main modal -->
    <div v-if="isModalOpen " class="fixed inset-0 z-50 flex justify-center items-center w-full h-full bg-black bg-opacity-50">
        <div class="relative p-4 w-full max-w-md max-h-full">
            <!-- Modal content -->
            <div class="relativerounded-lg shadow bg-gray-700">
                <!-- Modal header -->
                <div class="flex items-center justify-between p-4 md:p-5 border-b rounded-t dark:border-gray-600">
                    <h3 class="text-xl font-semibold text-gray-900 dark:text-white">
                        Nouvelle Discussion
                    </h3>
                    <button @click="toggleModale" type="button"
                        class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white">
                        <svg class="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none"
                            viewBox="0 0 14 14">
                            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
                        </svg>
                        <span class="sr-only">Close modal</span>
                    </button>
                </div>

                <!-- Modal body -->
                <div class="p-4 md:p-5">
                    <form @submit.prevent="validate" class="space-y-4">
                        <div>
                            <label for="title"
                                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Titre</label>
                            <input v-model="formData.title" type="text" name="title" id="title"
                                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white"
                                placeholder="Titre de la discussion" required />
                        </div>
                        <div>
                            <label for="description"
                                class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Description</label>
                            <input v-model="formData.description" type="text" name="description" id="description"
                                placeholder="Description"
                                class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white"
                                required />
                        </div>

                        <div v-if="errorMessage" class="text-red-600">
                            {{ errorMessage }}
                        </div>

                        <button type="submit"
                            class="w-full text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Valider</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

