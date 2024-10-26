import './assets/main.css'

import App from './App.vue'
import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import { routes } from '@/router/router'

const router = createRouter({
    history: createWebHistory(),
    routes
})

const app = createApp(App);

app.use(router);
app.mount("#app");

