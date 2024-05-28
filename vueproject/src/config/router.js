import { createRouter, createWebHistory } from 'vue-router';
import HelloWorld from '../components/HelloWorld.vue';
import Home from '../components/Home.vue';
import About from '../components/About.vue';

const routes = [
    { path: '/', component: HelloWorld },
    { path: '/home', component: Home },
    { path: '/about', component: About }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;