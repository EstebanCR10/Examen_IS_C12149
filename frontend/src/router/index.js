import { createRouter, createWebHistory } from 'vue-router'
import DrinksView from '../components/drinks.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'drinks',
      component: DrinksView,
    },
  ],
})

export default router
