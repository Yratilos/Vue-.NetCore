import { createI18n } from 'vue-i18n'
import enLocale from './locales/en.json'
import zhLocale from './locales/zh.json'

const messages = {
    en: enLocale,
    zh: zhLocale
}

let locale;
const cookie = document.cookie
if (cookie === '.AspNetCore.Culture=c=en-US|uic=en-US') {
    locale = 'en'
} else {
    locale = 'zh'
}

const i18n = createI18n({
    legacy: false,
    locale: locale,
    messages
})

export default i18n;