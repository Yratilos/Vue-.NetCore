import { createApp } from 'vue'
import App from './App.vue'
import { createI18n } from 'vue-i18n'
import enLocale from './i18n/locales/en.json'
import zhLocale from './i18n/locales/zh.json'

const messages = {
    en: enLocale,
    zh: zhLocale
}

const i18n = createI18n({
    legacy: false,
    locale: 'en',
    messages
})

createApp(App).use(i18n).mount('#app')
