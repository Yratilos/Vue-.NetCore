import { createApp } from 'vue'
import App from './App.vue'
import { createI18n } from 'vue-i18n'
import enLocale from './i18n/locales/en.json'
import zhLocale from './i18n/locales/zh.json'
import axios from 'axios'
import VueAxios from 'vue-axios'

const messages = {
    en: enLocale,
    zh: zhLocale
}

const i18n = createI18n({
    legacy: false,
    locale: 'en',
    messages
})

// 添加请求拦截器
axios.interceptors.request.use(function (config) {
    // 在发送请求之前做些什么
    return config;
}, function (error) {
    // 对请求错误做些什么
    return Promise.reject(error);
});

// 添加响应拦截器
axios.interceptors.response.use(function (response) {
    // 2xx 范围内的状态码都会触发该函数。
    const code = response.status
    if (code === 200) {
        const res = response.data
        if (!res.Status) {
            return res.data
        }
    }
    return Promise.reject(error);
}, function (error) {
    console.log(error)
    // 超出 2xx 范围的状态码都会触发该函数。
    // 对响应错误做点什么
    return Promise.reject(error);
});

const app = createApp(App)
app.use(i18n)
app.use(VueAxios, axios)
app.mount('#app')
