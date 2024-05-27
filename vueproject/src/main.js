import { createApp } from 'vue'
import App from './App.vue'
import i18n from './i18n/i18n'
import axios from './config/axios'
import VueAxios from 'vue-axios'
import store from './config/store'

const app = createApp(App)
app.use(i18n)
app.use(VueAxios, axios)
app.use(store)
app.mount('#app')