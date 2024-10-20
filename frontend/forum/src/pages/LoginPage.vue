<script setup lang="ts">
import { ref, onMounted } from 'vue';
import sessionService from '@/api/services/sessionService';
import userService from '@/api/services/userService';
import type { User } from '@/types/types';
import router from '@/router/router';

onMounted(() => {
    fetchUsers();
});

const formData = ref({
    username: '',
    password: ''
});
const usersInfo = ref<User[]>([]);
const errorMessage = ref<string | null>(null);

// Fetch user list from the server
const fetchUsers = async () => {
    errorMessage.value = null;
    try {
        const response = await userService.getUserList();
        usersInfo.value = response;
    } catch (error) {
        console.error('Error fetching user list:', error);
        errorMessage.value = "Could not fetch user list. Please try again later.";
    }
};

const login = async () => {
    const { username, password } = formData.value;

    const searchUser = usersInfo.value.find(user => user.accountId === username && user.password === password);

    if (searchUser) {
        sessionService.setItem({
            pseudo: searchUser.pseudo,
            mail: searchUser.mail,
            accountId: searchUser.accountId,
            id: searchUser.id,
            date: ''
        });
        
        await router.push({ path: '/home' });
        router.go(0);
    } else {
        errorMessage.value = "Mauvais mot de passe ou pseudo";
    }
};

</script>

<template>
    <div class="min-h-screen bg-gray-800 flex items-center justify-center">
        <div class="w-full max-w-sm">
            <h1 class="text-center text-white text-3xl mb-6">Se connecter</h1>

            <!-- Form Start -->
            <form class="bg-gray-900 p-6 rounded-lg shadow-lg w-full" @submit.prevent="login">
                <!-- Username Field -->
                <div class="mb-5">
                    <label for="pseudo" class="block mb-2 text-sm font-medium text-gray-300">Identifiant</label>
                    <input type="text" id="pseudo" v-model="formData.username"
                        class="bg-gray-800 border border-gray-600 text-gray-200 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:placeholder-gray-400"
                        placeholder="@" required />
                </div>

                <!-- Password Field -->
                <div class="mb-5">
                    <label for="password" class="block mb-2 text-sm font-medium text-gray-300">Mot de passe</label>
                    <input type="password" id="password" v-model="formData.password"
                        class="bg-gray-800 border border-gray-600 text-gray-200 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:placeholder-gray-400"
                        required />
                </div>

                <!-- Error Message Display -->
                <div v-if="errorMessage" class="mb-4 text-red-500 text-sm">
                    {{ errorMessage }}
                </div>

                <!-- Submit Button -->
                <button type="submit"
                    class="w-full text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                    Se connecter
                </button>
            </form>
            <!-- Form End -->

            <!-- Create Account Link -->
            <div class="mt-4 text-center">
                <span class="text-gray-300">Pas encore de compte?</span>
                <RouterLink to="/signin" class="text-blue-500 hover:text-blue-700"> Crée un compte</RouterLink>
            </div>
        </div>
    </div>
</template>
