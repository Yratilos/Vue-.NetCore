import { createStore } from 'vuex';
import userInfo from './module/userInfo'

export default createStore({
    state: {
        // 定义状态
    },
    mutations: {
        // 定义修改状态的方法
    },
    actions: {
        // 定义触发mutations的方法
    },
    getters: {
        // 定义获取state的计算属性
    },
    modules: {
        // 可以将store拆分成模块
        userInfo
    }
});