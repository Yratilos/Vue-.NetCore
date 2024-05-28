import { createApp } from 'vue'
import App from './App.vue'
import i18n from './config/i18n/i18n'
import axios from './config/axios'
import VueAxios from 'vue-axios'
import store from './config/store'
import router from './config/router'

const app = createApp(App)
app.use(i18n)
app.use(VueAxios, axios)
app.use(store)
app.use(router)
app.mount('#app')