import { createI18n } from 'vue-i18n'
import enLocale from './locales/en.json'
import zhLocale from './locales/zh.json'
import { getLanguage } from '../cookie'

const messages = {
    en: enLocale,
    zh: zhLocale
}

const i18n = createI18n({
    legacy: false,
    locale: getLanguage(),
    messages
})

export default i18n;