<script setup lang="ts">
import { ref, onMounted } from 'vue';
import sessionService from '@/api/services/sessionService';
import userService from '@/api/services/userService';
import type { User } from '@/types/types';
import { useRouter } from 'vue-router';
import dateUtils from '@/utils/dateUtils';

onMounted(async () => {
    await fetchUsers();
});

const router = useRouter();
const formData = ref({
    pseudo: '',
    accountId: '',
    mail: '',
    password: '',
});
const usersInfo = ref<User[]>([]);
const errorMessage = ref<string | null>(null);

const fetchUsers = async () => {
    errorMessage.value = null;
    try {
        const response = await userService.getUserList();
        usersInfo.value = response;
    } catch (error) {
        console.error('Error fetching user list:', error);
        errorMessage.value = "Impossible de récupérer la liste des utilisateurs. Veuillez réessayer plus tard.";
    }
};

const signUp = async () => {
    const { pseudo, accountId, mail, password } = formData.value;

    const isAccountIdTaken = usersInfo.value.find(user => user.accountId === accountId);
    if (isAccountIdTaken) {
        errorMessage.value = "L'identifiant est déjà pris. Veuillez en choisir un autre.";
        return;
    }

    try {
        //Returns the id of the newly created user
        const userId = await userService.createUser({
            pseudo,
            password,
            mail,
            accountId,
            date: dateUtils.formatCurrentDate(),
        });

        sessionService.setItem({
            id: userId,
            pseudo: pseudo,
            mail: mail,
            accountId: accountId,
        });

        formData.value = {
            pseudo: '',
            accountId: '',
            mail: '',
            password: '',
        };
        
        await router.push({ path: '/home' });
        router.go(0);
    } catch (error) {
        console.error('Error creating user:', error);
        errorMessage.value = "Erreur lors de la création du compte. Veuillez réessayer.";
    }
};

</script>

<template>
    <div class="min-h-screen bg-gray-800">
        <div class="flex items-center justify-center h-full">
            <div class="w-full max-w-sm">
                <h1 class="text-center text-white text-3xl mb-6">S'inscrire</h1>

                <!-- Form Start -->
                <form class="bg-gray-900 p-6 rounded-lg shadow-lg w-full" @submit.prevent="signUp">
                    <!-- Account ID Field -->
                    <div class="mb-5">
                        <label for="accountId" class="block mb-2 text-sm font-medium text-gray-300">Identifiant</label>
                        <input v-model="formData.accountId" type="text" id="accountId"
                            class="bg-gray-800 border border-gray-600 text-gray-200 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:placeholder-gray-400"
                            placeholder="@" required />
                    </div>
                    <!-- Username Field -->
                    <div class="mb-5">
                        <label for="pseudo" class="block mb-2 text-sm font-medium text-gray-300">Pseudo</label>
                        <input v-model="formData.pseudo" type="text" id="pseudo"
                            class="bg-gray-800 border border-gray-600 text-gray-200 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:placeholder-gray-400"
                            placeholder="Votre pseudo" required />
                    </div>

                    <!-- Email Field -->
                    <div class="mb-5">
                        <label for="email" class="block mb-2 text-sm font-medium text-gray-300">Adresse mail</label>
                        <input v-model="formData.mail" type="email" id="email"
                            class="bg-gray-800 border border-gray-600 text-gray-200 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:placeholder-gray-400"
                            placeholder="Votre adresse mail" required />
                    </div>

                    <!-- Password Field -->
                    <div class="mb-5">
                        <label for="password" class="block mb-2 text-sm font-medium text-gray-300">Mot de passe</label>
                        <input v-model="formData.password" type="password" id="password"
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
                        S'inscrire
                    </button>
                </form>
                <!-- Form End -->

                <!-- Already have an account? -->
                <div class="mt-4 text-center">
                    <span class="text-gray-300">Déjà un compte?</span>
                    <RouterLink to="/login" class="text-blue-500 hover:text-blue-700"> Se connecter</RouterLink>
                </div>
            </div>
        </div>
    </div>
</template>
