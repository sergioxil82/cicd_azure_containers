import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import vuetify from './plugins/vuetify'

const app = createApp(App)

console.log('VITE_API_WORK_TIME_URL:' + import.meta.env.VITE_API_WORK_TIME_URL)
console.log('VITE_API_AUDITORY_URL', import.meta.env.VITE_API_AUDITORY_URL)
console.log('VITE_USER_NAME', import.meta.env.VITE_USER_NAME)
console.log('VITE_FIRST_NAME', import.meta.env.VITE_FIRST_NAME)
console.log('VITE_LAST_NAME', import.meta.env.VITE_LAST_NAME)

app.use(createPinia())
app.use(router)
app.use(vuetify)
app.mount('#app')
