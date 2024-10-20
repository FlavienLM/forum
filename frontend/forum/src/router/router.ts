import HomePage from "@/pages/HomePage.vue"
import LoginPage from "@/pages/LoginPage.vue"
import SignUpPage from "@/pages/SignUpPage.vue"
import DiscussionPage from "@/pages/DiscussionPage.vue"
import { createRouter, createWebHistory, type RouteLocationNormalized } from 'vue-router';
import ProfilePage from "@/pages/ProfilePage.vue";
import FriendChatPage from "@/pages/FriendChatPage.vue";

export const routes = [
    { path: '/home', component: HomePage },
    { path: '/login', component: LoginPage },
    { path: '/signin', component: SignUpPage },
    {
        path: '/home/:id?',
        component: HomePage,
        name: 'Home',
        props: (route: RouteLocationNormalized) => ({
            id: route.params.id ? Number(route.params.id) : 1 
        })
    },
    {
        path: '/discussion/:id',
        component: DiscussionPage,
        props: (route: RouteLocationNormalized) => ({
            id: Number(route.params.id)
        })
    },
    {
        path: '/friend/:id',
        component: FriendChatPage,
        props: (route: RouteLocationNormalized) => ({
            id: Number(route.params.id)
        })
    },
    {
        path: '/profile/:id',
        component: ProfilePage,
        props: (route: RouteLocationNormalized) => ({
            id: Number(route.params.id)
        })
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;